using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BoundingBox
{
	[Tooltip("Left-most bound.")]
	public float left;
	[Tooltip("Top-most bound.")]
	public float top;
	[Tooltip("Right-most bound.")]
	public float right;
	[Tooltip("Bottom-most bound.")]
	public float bottom;

	public BoundingBox(float newLeft, float newTop, float newRight, float newBottom)
	{
		left = newLeft;
		top = newTop;
		right = newRight;
		bottom = newBottom;
	}
}

public class BlockManager : NetworkBehaviour
{
	//Play field bounds
	[Tooltip("The bounds of the play field.")]
	public BoundingBox playFieldBounds = new BoundingBox(-4, 4, 4, -4);
	private Vector2 bottomLeftExtreme;
	private Vector2 topRightExtreme;

	[Space(10)]

	//Block prefab
	[Tooltip("Block prefab.")]
	public GameObject blockPrefab;

	//Block size
	[Tooltip("Size of blocks.")]
	public float blockSize = 1.0f;
	private Vector3 displacement3D;
	private Vector2 displacement2D;
	private Vector2 selectedBlockPos;

	//Block count in the field
	[Tooltip("The number of blocks horizontally (x) and vertically (y) across the field.")]
	public Vector2 blockCount = new Vector2(8, 8);
	private Vector2 blockMax;

	[Space(10)]

	//Block rarity
	[Tooltip("The rarity of each block type.")]
	public int[] blockRarities = new int[5] { 200, 200, 200, 200, 200};

    //All blocks
    [SerializeField]
    private SyncListInt generatedTypes = new SyncListInt();
	private GameObject[,] allBlocks;

	[Space(10)]

	//block movement speeds
	[Tooltip("The time taken to swap 2 blocks.")]
	public float swapTime = 0.5f;
	[Tooltip("The fall speed of all blocks.")]
	public float fallSpeed = 2.5f;

	[Space(10)]

	//Is the player using mouse or cursor control?
	public bool isCursorControl = true;

	[Space(10)]

	//Cursor
	public GameObject cursorPrefab;
	private GameObject cursor;
	private Vector2 cursorPos;
	private SpriteRenderer cursorRenderer;
	private bool canSwap = true;
	
	//Selected block
	private GameObject selectedBlock;
	private BlockDetails selectedDetails;
	
	//Mouse position
	private Vector3 clickPos;
	private Vector3 releasePos;
	
	//Block dropping values
	private GameObject[] highestBlocks; //Highest blocks in each column. Used to set spawned block height to prevent overlap
	private Coroutine[] dropCoroutines;

	//Synced types. Used to network type changes
	[SerializeField]
	public SyncListInt syncedTypes = new SyncListInt(); 

	//Chain counting list
	//[HideInInspector]
	private List<int> allChains = new List<int>();

	//UI objects
	private MatchUI mui;
    private CombatHandler atkBar;
	
	public Sprite[] spriteSheet;

	//Setup
	void Start()
    {
        if (isCursorControl)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

		bottomLeftExtreme = new Vector2(transform.position.x + playFieldBounds.left, transform.position.y + playFieldBounds.bottom);
		topRightExtreme = new Vector2(transform.position.x + playFieldBounds.right, transform.position.y + playFieldBounds.top);

		displacement3D = new Vector3(blockSize / 2, blockSize / 2, 0);
		displacement2D = new Vector2(blockSize / 2, blockSize / 2);

		blockMax = blockCount - new Vector2(1, 1);
		
		if (isCursorControl)
		{
			cursorPos = new Vector2(Mathf.FloorToInt(blockCount.x / 2) - 1, Mathf.Min(2, blockCount.y));
			
			cursor = Instantiate(cursorPrefab, CoordToPosition((int)cursorPos.x, (int)cursorPos.y, true), Quaternion.identity, gameObject.transform);

			cursorRenderer = cursor.GetComponent<SpriteRenderer>();
		}
		
		highestBlocks = new GameObject[(int)blockCount.x];
		dropCoroutines = new Coroutine[(int)blockCount.x];
		
		mui = gameObject.GetComponent<MatchUI>();
        atkBar = gameObject.GetComponent<CombatHandler>();

        if (isServer)
        {
            RandomiseTypes();
        }

		SetupBlocks();
    }

	//Generate the first set of randomised blocks. Prevent spawning matching blocks in lines of 3 or more
    void RandomiseTypes()
    {
        int yCoord;

        for (int y = 0; y < blockCount.y; y++)
        {
            yCoord = y * (int)blockCount.x;

            for (int x = 0; x < blockCount.x; x++)
            {
                bool[] typeInclusivity = new bool[5] { true, true, true, true, true };
                int tempType;

                if (x > 1)
                {
                    tempType = generatedTypes[yCoord + x - 1];

                    if (tempType == generatedTypes[yCoord + x - 2])
                    {
                        typeInclusivity[(int)tempType] = false;
                    }
                }

                if (y > 1)
                {
                    tempType = generatedTypes[yCoord - (int)blockCount.x + x];

                    if (tempType == generatedTypes[yCoord - (int)blockCount.x * 2 + x])
                    {
                        typeInclusivity[(int)tempType] = false;
                    }
                }
				
				generatedTypes.Add((int)GenerateRandomType(typeInclusivity));
            }
        }

		for(int i = 0; i < blockCount.x * blockCount.y; i++)
		{
			syncedTypes.Add(-1);
		}
    }

	//Get a random type based on rarity and block availability
	BlockTypes GenerateRandomType(bool[] typeInclusivity)
	{
		int maxChance = 0;

		for (int i = 0; i < typeInclusivity.Length; i++)
		{
			if (typeInclusivity[i])
			{
				maxChance += blockRarities[i];
			}
		}

		int randNum = Random.Range(0, maxChance);
		int chanceStage = 0;

		for (int i = 0; i < typeInclusivity.Length; i++)
		{
			if (typeInclusivity[i])
			{
				chanceStage += blockRarities[i];

				if (randNum < chanceStage)
				{
					return (BlockTypes)i;
				}
			}
		}

		return BlockTypes.A;
	}

	//Setup initial blocks
	void SetupBlocks()
	{
		allBlocks = new GameObject[(int)blockCount.x, (int)blockCount.y];

		float xBase = transform.position.x + playFieldBounds.left;
		float yBase = transform.position.y + playFieldBounds.bottom;
		
		float yPos;

        int yCoord;
		
		for (int y = 0; y < blockCount.y; y++)
		{
			yPos = yBase + y * blockSize;

            yCoord = y * (int)blockCount.x;

            for (int x = 0; x < blockCount.x; x++)
			{
				Vector3 pos = new Vector3(xBase + x * blockSize, yPos, 0) + displacement3D;

				allBlocks[x, y] = Instantiate(blockPrefab, pos, Quaternion.identity, transform);
                //NetworkServer.Spawn(allBlocks[x, y]);

				BlockDetails details = allBlocks[x, y].GetComponent<BlockDetails>();

				details.coords = new Vector2(x, y);
                details.type = (BlockTypes)generatedTypes[yCoord + x];
				details.spriteSheet = spriteSheet;
            }
		}
	}
	
    //Handle input and chain counting
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
		
		//Handle swap input
        if (isCursorControl)
		{
			if (Input.GetButtonDown("Horizontal"))
			{
				cursorPos.x = Mathf.Clamp(cursorPos.x + 1 * Mathf.Sign(Input.GetAxisRaw("Horizontal")), 0, blockMax.x);

				cursor.transform.position = CoordToPosition((int)cursorPos.x, (int)cursorPos.y, true);
			}

			if (Input.GetButtonDown("Vertical"))
			{
				cursorPos.y = Mathf.Clamp(cursorPos.y + 1 * Mathf.Sign(Input.GetAxisRaw("Vertical")), 0, blockMax.y);

				cursor.transform.position = CoordToPosition((int)cursorPos.x, (int)cursorPos.y, true);
			}

			if (canSwap)
			{
				if (Input.GetButtonDown("SwapH"))
				{
					int direction = 1 * Mathf.FloorToInt(Mathf.Sign(Input.GetAxisRaw("SwapH")));
					int swapIndex = (int)cursorPos.x + direction;
					
					if (0 <= swapIndex && swapIndex <= blockMax.x)
					{
						GameObject blockThis = allBlocks[(int)cursorPos.x, (int)cursorPos.y];
						GameObject blockThat = allBlocks[swapIndex, (int)cursorPos.y];

						BlockDetails detailsThis = blockThis.GetComponent<BlockDetails>();
						BlockDetails detailsThat = blockThat.GetComponent<BlockDetails>();

						if (detailsThis.isInteractable && detailsThat.isInteractable)
						{
							HandleSwap(new Vector2Int((int)cursorPos.x, (int)cursorPos.y), new Vector2Int(swapIndex, (int)cursorPos.y));
						}
					}
				}
				else if (Input.GetButtonDown("SwapV"))
				{
					int direction = 1 * Mathf.FloorToInt(Mathf.Sign(Input.GetAxisRaw("SwapV")));
					int swapIndex = (int)cursorPos.y + direction;
					
					if (0 <= swapIndex && swapIndex <= blockMax.y)
					{
						GameObject blockThis = allBlocks[(int)cursorPos.x, (int)cursorPos.y];
						GameObject blockThat = allBlocks[(int)cursorPos.x, swapIndex];

						BlockDetails detailsThis = blockThis.GetComponent<BlockDetails>();
						BlockDetails detailsThat = blockThat.GetComponent<BlockDetails>();

						if (detailsThis.isInteractable && detailsThat.isInteractable)
						{
							HandleSwap(new Vector2Int((int)cursorPos.x, (int)cursorPos.y), new Vector2Int((int)cursorPos.x, swapIndex));
						}
					}
				}
			}
		}
		else
		{
			if (Input.GetButtonDown("Select") && selectedBlock == null)
			{
				clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				if (IsInBounds(new Vector2(clickPos.x, clickPos.y)))
				{
					Vector2 fixedPos = new Vector2(clickPos.x, clickPos.y) - bottomLeftExtreme;

					Vector2 index = new Vector2(Mathf.FloorToInt(fixedPos.x / blockSize), Mathf.FloorToInt(fixedPos.y / blockSize));

					GameObject targetBlock = allBlocks[(int)index.x, (int)index.y];

					if (targetBlock.GetComponent<BlockDetails>().isInteractable)
					{
						selectedBlock = targetBlock;
						selectedDetails = selectedBlock.GetComponent<BlockDetails>();

						Vector3 pos = CoordToPosition((int)index.x, (int)index.y);

						selectedBlockPos = new Vector2(pos.x, pos.y);
					}
				}
			}
			else if(canSwap && Input.GetButton("Select") && selectedBlock != null)
			{
				if (selectedDetails.isInteractable)
				{
					Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

					if (!IsInBounds(mousePos, selectedBlockPos - displacement2D, selectedBlockPos + displacement2D))
					{
						Vector2Int selectedCoords = new Vector2Int((int)selectedDetails.coords.x, (int)selectedDetails.coords.y);
						Vector2Int swapDirection = Vector2Int.zero;
						Vector2Int targetCoords = Vector2Int.zero;

						float relativeAngle = Mathf.Atan2(mousePos.y - selectedBlockPos.y, mousePos.x - selectedBlockPos.x) * Mathf.Rad2Deg + 180;

						if(315 >= relativeAngle && relativeAngle > 225)
						{
							swapDirection = new Vector2Int(0, 1);
						}
						else if (225 >= relativeAngle && relativeAngle > 135)
						{
							swapDirection = new Vector2Int(1, 0);
						}
						else if (135 >= relativeAngle && relativeAngle > 45)
						{
							swapDirection = new Vector2Int(0, -1);
						}
						else
						{
							swapDirection = new Vector2Int(-1, 0);
						}

						targetCoords = selectedCoords + swapDirection;

						if (IsInBounds(targetCoords, new Vector2Int(-1, -1), blockCount))
						{
							BlockDetails detailsThat = allBlocks[targetCoords.x, targetCoords.y].GetComponent<BlockDetails>();

							if (detailsThat.isInteractable)
							{
								HandleSwap(selectedCoords, targetCoords);

								Vector3 pos = CoordToPosition((int)selectedDetails.coords.x, (int)selectedDetails.coords.y);

								selectedBlockPos = new Vector2(pos.x, pos.y);
							}
						}
					}
				}
			}
			else if (Input.GetButtonUp("Select") && selectedBlock != null)
			{
				selectedBlock = null;
			}
		}

		//Chain counting
		bool isStillChaining = false;

		for (int i = 0; i < allChains.Count; i++)
		{
			if (allChains[i] > -1)
			{
				isStillChaining = true;
			}
		}

		if (!isStillChaining)
		{
			allChains.Clear();
		}

		//temp debug. return to lobby scene
		if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.Q))
		{
			SceneManager.LoadScene("Lobby");
		}

		//temp debug. get if any blocks are in the wrong state. hold control to force into the right state
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			BlockDetails details;

			bool isError = false;

			bool shouldForce = Input.GetKey(KeyCode.RightControl);

			for(int x = 0; x < blockCount.x; x++)
			{
				for(int y = 0; y < blockCount.y; y++)
				{
					details = allBlocks[x, y].GetComponent<BlockDetails>();

					if (!details.isInteractable || details.isFalling)
					{
						print(details.coords.x + ", " + details.coords.y + ": " + details.isInteractable.ToString() + ", " + details.isFalling.ToString());
						
						isError = true;

						if (shouldForce)
						{
							details.isInteractable = true;
							details.isFalling = false;
						}
					}
				}
			}

			if (!isError)
			{
				print("no issues");
			}
			else
			{
				if (shouldForce)
				{
					print("issues were fixed");
				}
			}
		}

		//temp debug. force the type of the block the mouse is over
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			DebugForceType(0);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			DebugForceType(1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			DebugForceType(2);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			DebugForceType(3);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			DebugForceType(4);
		}
	}

	//temp debug. force type
	void DebugForceType(int type)
	{
		clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (IsInBounds(new Vector2(clickPos.x, clickPos.y)))
		{
			Vector2 fixedPos = new Vector2(clickPos.x, clickPos.y) - bottomLeftExtreme;

			Vector2 index = new Vector2(Mathf.FloorToInt(fixedPos.x / blockSize), Mathf.FloorToInt(fixedPos.y / blockSize));

			BlockDetails details = allBlocks[(int)index.x, (int)index.y].GetComponent<BlockDetails>();

			details.type = (BlockTypes)type;
			details.UpdateType();
		}
	}

	//Are the supplied co ordinates within the board extremes
	bool IsInBounds(Vector2 input)
	{
		return bottomLeftExtreme.x < input.x && input.x < topRightExtreme.x && bottomLeftExtreme.y < input.y && input.y < topRightExtreme.y;
	}

	//Are the supplied co ordinates within the supplied bounds
	bool IsInBounds(Vector2 input, Vector2 bottomLeft, Vector2 topRight)
	{
		return bottomLeft.x < input.x && input.x < topRight.x && bottomLeft.y < input.y && input.y < topRight.y;
	}
	
	//Swap cooldown
	IEnumerator CursorCooldown()
	{
		canSwap = false;

		if (isCursorControl)
		{
			cursorRenderer.color = Color.black;
		}
		else
		{
			
		}

		yield return new WaitForSeconds(swapTime);
		yield return new WaitForEndOfFrame();

		canSwap = true;

		if (isCursorControl)
		{
			cursorRenderer.color = Color.white;
		}
		else
		{
			
		}
	}

	//Handle chaging values, then swap blocks
	void HandleSwap(Vector2Int blockSelected, Vector2Int blockToSwap)
	{
		if (hasAuthority)
		{
			CmdSwapBlocks(blockSelected.x, blockSelected.y, blockToSwap.x, blockToSwap.y);
		}

		int chainCount = allChains.Count;

		Coroutine relevantCoroutine;
		
		GameObject blockThis = allBlocks[blockSelected.x, blockSelected.y];
		GameObject blockThat = allBlocks[blockToSwap.x, blockToSwap.y];
		
		BlockDetails detailsThis = blockThis.GetComponent<BlockDetails>();
		BlockDetails detailsThat = blockThat.GetComponent<BlockDetails>();
		
		Vector2 swapDirection = detailsThat.coords - detailsThis.coords;
		
		detailsThis.isInteractable = false;
		detailsThis.coords += swapDirection;
		allBlocks[blockToSwap.x, blockToSwap.y] = blockThis;

		relevantCoroutine = detailsThis.movementCoroutine;

		if (relevantCoroutine != null)
		{
			StopCoroutine(relevantCoroutine);
		}

		detailsThis.movementCoroutine =  StartCoroutine(SwapBlock(blockThis, detailsThis, blockThis.transform.position, CoordToPosition(blockToSwap.x, blockToSwap.y), chainCount));
		
		detailsThat.isInteractable = false;
		detailsThat.coords -= swapDirection;
		allBlocks[blockSelected.x, blockSelected.y] = blockThat;

		relevantCoroutine = detailsThat.movementCoroutine;

		if (relevantCoroutine != null)
		{
			StopCoroutine(relevantCoroutine);
		}

		detailsThat.movementCoroutine = StartCoroutine(SwapBlock(blockThat, detailsThat, blockThat.transform.position, CoordToPosition(blockSelected.x, blockSelected.y), chainCount));
		
		StartCoroutine(CursorCooldown());
	}

	//Horizontally swap blocks
	IEnumerator SwapBlock(GameObject obj, BlockDetails details, Vector3 origin, Vector3 destination, int chainCount)
	{
        float elapsedTime = 0.0f;

		Vector3 direction = destination - origin;

		while (elapsedTime < swapTime)
		{
			elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, swapTime);

			obj.transform.position = origin + direction * Mathf.Sin(Mathf.Deg2Rad * 90 * (elapsedTime / swapTime));

			yield return new WaitForEndOfFrame();
		}

		details.isInteractable = true;

		yield return new WaitForEndOfFrame();

		CheckForMatches(details.coords, details.type, chainCount, -1);
	}

	//Tell all clients which blocks to swap
    [Command]
    void CmdSwapBlocks(int selectedX, int selectedY, int toSwapX, int toSwapY)
    {
		RpcSwapBlocks(selectedX, selectedY, toSwapX, toSwapY);
    }
	
	//Receive instructions to swap
    [ClientRpc]
    void RpcSwapBlocks(int selectedX, int selectedY, int toSwapX, int toSwapY)
    {
        if (hasAuthority)
        {
            return;
        }
		
		HandleSwap(new Vector2Int(selectedX, selectedY), new Vector2Int(toSwapX, toSwapY));
    }
	
	//Control block falling and align if necessary
	IEnumerator DropBlock(GameObject obj, BlockDetails details, Vector3 destination, int chainIndex, bool shouldWaitForType)
	{
		Vector3 blockPos = obj.transform.position;

		float diff = destination.x - obj.transform.position.x;
		int alt = (int)Mathf.Sign(diff);
		
		while (obj.transform.position.y > destination.y)
		{
			blockPos.y = Mathf.Max(blockPos.y - fallSpeed * Time.deltaTime, destination.y);

			if (Mathf.Abs(diff) > 0)
			{
				if (alt == 1)
				{
					diff = Mathf.Max(0, diff - 0.05f);
				}
				else
				{
					diff = Mathf.Min(0, diff + 0.05f);
				}
				
				blockPos.x = destination.x - diff;
			}

			obj.transform.position = blockPos;

			yield return new WaitForEndOfFrame();
		}

		obj.transform.position = destination;
		
		details.isInteractable = true;
		details.isFalling = false;

		yield return new WaitForEndOfFrame();

		CheckForMatches(details.coords, details.type, -1, chainIndex);
	}

	//Handle the logic before and after dropping blocks
	IEnumerator HandleBlockDrop(int xCoord, List<int> yCoords, int chainIndex)
	{
		//Set values and references
		int yCount = yCoords.Count;
		int highestYCoord = yCoords[yCount - 1];

		List<GameObject> droppingBlocks = new List<GameObject>();
		List<BlockDetails> droppingDetails = new List<BlockDetails>();

		for(int i = 0; i < yCount; i++)
		{
			droppingBlocks.Add(allBlocks[xCoord, yCoords[i]]);
			droppingDetails.Add(droppingBlocks[i].GetComponent<BlockDetails>());
			
			droppingDetails[i].isInteractable = false;
		}
		
		//Wait for break
		yield return new WaitForSeconds(0.5f);

		//Find the lowest coords to move blocks to
		Vector3 newPos = CoordToPosition(xCoord, (int)blockCount.y);
		
		if (highestBlocks[xCoord] != null)
		{
			newPos.y = Mathf.Max(newPos.y, highestBlocks[xCoord].transform.position.y + blockSize);
		}
		
		//Move blocks to the top// and change their colour if they have authority
		for (int i = 0; i < yCount; i++)
		{
			droppingBlocks[i].transform.position = newPos + new Vector3(0, blockSize, 0) * i;
		}
		
		highestBlocks[xCoord] = droppingBlocks[yCount - 1];
		
		//Tell blocks above to fall
		int newY = 0;
		
		BlockDetails details;
		Coroutine relevantCoroutine;

		for (int i = highestYCoord + 1; i < blockCount.y; i++)
		{
			details = allBlocks[xCoord, i].GetComponent<BlockDetails>();

			while(!(details.isFalling || details.isInteractable))
			{
				yield return new WaitForEndOfFrame();
			}
			
			newY = i - yCount;
					
			details.isInteractable = false;
			details.isFalling = true;
			details.coords = new Vector2(xCoord, newY);
			allBlocks[xCoord, newY] = details.gameObject;

			relevantCoroutine = details.movementCoroutine;

			if (relevantCoroutine != null)
			{
				StopCoroutine(relevantCoroutine);
			}

			details.movementCoroutine = StartCoroutine(DropBlock(details.gameObject, details, CoordToPosition(xCoord, newY), chainIndex, false));
				
			if (details.chainIndex == -1)
			{
				details.chainIndex = chainIndex;
			}
		}

		//Drop blocks at the top and change type
		List<int> newYCoords = new List<int>();

		for (int i = 0; i < yCount; i++)
		{
			details = droppingDetails[i];
			
			newY = (int)blockMax.y - yCount + i + 1;

			newYCoords.Add(newY);
					
			details.isInteractable = false;
			details.isFalling = true;
			details.coords = new Vector2(xCoord, newY);
			allBlocks[xCoord, newY] = details.gameObject;

			relevantCoroutine = details.movementCoroutine;

			if (relevantCoroutine != null)
			{
				StopCoroutine(relevantCoroutine);
			}

			details.movementCoroutine = StartCoroutine(DropBlock(details.gameObject, details, CoordToPosition(xCoord, newY), chainIndex, true));
					
			if (details.chainIndex == -1)
			{
				details.chainIndex = chainIndex;
			}

			if (isLocalPlayer)
			{
				BlockTypes newType = GenerateRandomType(new bool[5] { true, true, true, true, true });

				details.type = newType;
				details.UpdateType();

				CmdUpdateSyncedTypes((int)details.coords.x, (int)details.coords.y, (int)newType);
			}
		}

		if (!isLocalPlayer)
		{
			StartCoroutine(NetworkTypeUpdate(xCoord, newYCoords));
		}
	}
	
	//Update synced typed
	[Command]
	void CmdUpdateSyncedTypes(int x, int y, int t)
	{
		syncedTypes[(int)(y * blockCount.x + x)] = t;
	}

	//Update type on client
	IEnumerator NetworkTypeUpdate(int xCoord, List<int> yCoords)
	{
		BlockDetails details;

		for (int i = 0; i < yCoords.Count; i++)
		{
			int syncIndex = (int)(yCoords[i] * blockCount.x + xCoord);

			while (syncedTypes[syncIndex] == -1)
			{
				yield return new WaitForEndOfFrame();
			}

			details = allBlocks[xCoord, yCoords[i]].GetComponent<BlockDetails>();

			details.type = (BlockTypes)syncedTypes[(int)(yCoords[i] * blockCount.x + xCoord)];
			details.UpdateType();

			CmdUpdateSyncedTypes(xCoord, yCoords[i], -1);
		}
	}

	//Checks for matches and move blocks accordingly
	void CheckForMatches(Vector2 pos, BlockTypes relevantType, int chainCount, int chainIndex)
	{
		BlockDetails details;

		List<int> matchingHorizontalIndices = new List<int>();
		List<int> matchingVerticalIndices = new List<int>() { (int)pos.y };
		
		//Count matching blocks
		for (int i = 1; i < blockCount.x; i++)
		{
			if(pos.x - i < 0)
			{
				break;
			}
			else
			{
				details = allBlocks[(int)pos.x - i, (int)pos.y].GetComponent<BlockDetails>();

				if (details.type == relevantType && details.isInteractable)
				{
					matchingHorizontalIndices.Add((int)pos.x - i);
				}
				else
				{
					break;
				}
			}
		}
		
		for (int i = 1; i < blockCount.x; i++)
		{
			if (pos.x + i > blockMax.x)
			{
				break;
			}
			else
			{
				details = allBlocks[(int)pos.x + i, (int)pos.y].GetComponent<BlockDetails>();


				if (details.type == relevantType && details.isInteractable)
				{
					matchingHorizontalIndices.Add((int)pos.x + i);
				}
				else
				{
					break;
				}
			}
		}

		for (int i = 1; i < blockCount.y; i++)
		{
			if (pos.y - i < 0)
			{
				break;
			}
			else
			{
				details = allBlocks[(int)pos.x, (int)pos.y - i].GetComponent<BlockDetails>();

				if (details.type == relevantType && details.isInteractable)
				{
					matchingVerticalIndices.Add((int)pos.y - i);
				}
				else
				{
					break;
				}
			}
		}

		for (int i = 1; i < blockCount.y; i++)
		{
			if (pos.y + i > blockMax.y)
			{
				break;
			}
			else
			{
				details = allBlocks[(int)pos.x, (int)pos.y + i].GetComponent<BlockDetails>();

				if (details.type == relevantType && details.isInteractable)
				{
					matchingVerticalIndices.Add((int)pos.y + i);
				}
				else
				{
					break;
				}
			}
		}

		//If no vertical match was made, add the swapped block to horizontal matches
		if(matchingHorizontalIndices.Count >= 2 && matchingVerticalIndices.Count < 3)
		{
			matchingHorizontalIndices.Add((int)pos.x);
		}

		//Calculate combos and chains
		int comboCount = 0;
		int newChainIndex = chainIndex;
		
		if(matchingHorizontalIndices.Count >= 2 || matchingVerticalIndices.Count >= 3)
		{
			if(hasAuthority && !isCursorControl && selectedDetails.coords == pos)
			{
				selectedBlock = null;
			}
			
			int horizontalMatchCount = matchingHorizontalIndices.Count;
			int verticalMatchCount = matchingVerticalIndices.Count;

			if (horizontalMatchCount > 2 && verticalMatchCount < 3)
			{
				comboCount = horizontalMatchCount;
			}
			else if (horizontalMatchCount < 2 && verticalMatchCount > 2)
			{
				comboCount = verticalMatchCount;
			}
			else if (horizontalMatchCount > 1 && verticalMatchCount > 2)
			{
				comboCount = horizontalMatchCount + verticalMatchCount;
			}

			//to retrieve combo count for current, use comboCount
			
			if (chainIndex == -1)
			{
				newChainIndex = chainCount;

				if (allChains.Count == chainCount)
				{
					allChains.Add(1);
				}
			}
			else if(chainIndex > -1)
			{
				//allChains[chainIndex] += 1;
			}

            //print(allChains[newChainIndex]);
            //mui.UpdateChains(allChains[newChainIndex]);
            //mui.UpdateCombo(comboCount);

            //atkBar.FillBar(relevantType, allChains[newChainIndex], comboCount);
            atkBar.FillBar(relevantType, 1, comboCount);

            //to retrieve chain count for current swap, use allChains[chainIndex]
        }
		else if(matchingHorizontalIndices.Count < 2 || matchingVerticalIndices.Count < 3)
		{
			if(chainIndex > -1)
			{
				bool areStillFalling = false;

				details = allBlocks[(int)pos.x, (int)pos.y].GetComponent<BlockDetails>();
				
				details.chainIndex = -1;

				for(int x = 0; x < blockCount.x; x++)
				{
					for(int y = 0; y < blockCount.y; y++)
					{
						details = allBlocks[x, y].GetComponent<BlockDetails>();

						if(details.chainIndex == chainIndex)
						{
							if (details.isFalling)
							{
								areStillFalling = true;
							}
						}
					}
				}

				if (!areStillFalling)
				{
					if(chainIndex < allChains.Count)
					{
						allChains[chainIndex] = -1;
					}
				}
			}
		}
		
		//Break matched blocks and drop new blocks
		Coroutine relevantCoroutine;

		if (matchingHorizontalIndices.Count >= 2)
		{
			for (int x = 0; x < matchingHorizontalIndices.Count; x++)
			{
				relevantCoroutine = dropCoroutines[matchingHorizontalIndices[x]];

				if(relevantCoroutine != null)
				{
					StopCoroutine(relevantCoroutine);
				}

				dropCoroutines[matchingHorizontalIndices[x]] = StartCoroutine(HandleBlockDrop(matchingHorizontalIndices[x], new List<int>() {(int)pos.y}, newChainIndex));
			}
		}

		if (matchingVerticalIndices.Count >= 3)
		{
			matchingVerticalIndices.Sort();

			relevantCoroutine = dropCoroutines[(int)pos.x];

			if (relevantCoroutine != null)
			{
				StopCoroutine(relevantCoroutine);
			}

			dropCoroutines[(int)pos.x] = StartCoroutine(HandleBlockDrop((int)pos.x, matchingVerticalIndices, newChainIndex));
		}
	}
	
	//Wrapper for a default location
	Vector3 CoordToPosition(int x, int y)
	{
		return CoordToPosition(x, y, false);
	}

	//Produce a vector 3 based on the coordinates provides
	Vector3 CoordToPosition(int x, int y, bool useExtraDisplacement)
	{
		float extraZ = -1.25f;

		if (!useExtraDisplacement)
		{
			extraZ = 0;
		}

		return transform.position + new Vector3((x - (blockCount.x / 2)) * blockSize, (y - (blockCount.y / 2)) * blockSize, extraZ) + displacement3D;
	}
}
