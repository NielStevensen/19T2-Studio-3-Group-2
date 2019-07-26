using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

[System.Serializable]
public class SpriteSheet
{
	[Tooltip("Name of this sprite sheet.")]
	public string name = "Sheet";
	[Tooltip("Array of sprites in the list.\nOrder: Fire, Lightning, Nature, Water, healing.")]
	public Sprite[] spriteSheet = new Sprite[5];
}

[System.Serializable]
public class ComboDetails
{
	//Index of the chainDetails this will contribute to
	public int chainIndex = -1;
	//Total blocks combo'd
	public int[] comboNumbers = new int[5];

	public ComboDetails(int index, int[] numbers)
	{
		chainIndex = index;
		comboNumbers = numbers;
	}
}

[System.Serializable]
public class ChainDetails
{
	//Index of the swap that is causing the chain this is detailing
	public int swapIndex = -1;
	//Number of matches in the detailed chain
	public int chainNumber = -1;
	//IDs of all blocks that are currently falling due to this chain
	public List<int> involvedIDs = new List<int>();

	//If one of the two blocks swapped does not drop any blocks, set to true
	//If the second does not drop any blocks, this will be true. At this point, set chainNumber to -1;
	public bool shouldClear = false;

	public ChainDetails(int index, int number)
	{
		swapIndex = index;
		chainNumber = number;
	}
}

public class BlockManager : NetworkBehaviour
{
	#region Variables
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
	[HideInInspector]
	public GameObject[,] allBlocks;
	[HideInInspector]
	public BlockDetails[] allBlocksStatic;

	[Space(10)]

	//block movement speeds
	[Tooltip("The time taken to swap 2 blocks.")]
	public float swapTime = 0.5f;
	[Tooltip("The fall speed of all blocks.")]
	public float fallSpeed = 2.5f;

	[Space(10)]

	//Is the player using mouse or cursor control?
	[Tooltip("Is the player using cursor control?")]
	public bool isCursorControl = true;

	[Space(10)]

	//Cursor
	[Tooltip("Cursor prefab.")]
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
	private List<GameObject>[] undroppedBlocks;

	//Combos and chains
	private List<ComboDetails> comboDetails = new List<ComboDetails>();
	private List<ChainDetails> chainDetails = new List<ChainDetails>();

	[Space(10)]

	//Sprite sheets
	[Tooltip("All sprite sheets.")]
	public SpriteSheet[] spriteSheets = new SpriteSheet[4];
	[Tooltip("The index of the sprite sheet the player is using.")]
	public int spriteSheetIndex = 0;

	//UI objects
	private MatchUI mui;
    private CombatHandler atkBar;
	#endregion

	//Initialise arrays
	private void Awake()
	{
		allBlocks = new GameObject[(int)blockCount.x, (int)blockCount.y];
		allBlocksStatic = new BlockDetails[(int)(blockCount.x * blockCount.y)];
	}

	//Setup
	void Start()
    {
		ManagerIndentifier identifier = FindObjectOfType<ManagerIndentifier>();

		if (isLocalPlayer)
		{
			identifier.clientManager = this;

			gameObject.name = "Client manager";
		}
		else
		{
			identifier.projectedManager = this;

			gameObject.name = "Projected manager";
		}

		if (isCursorControl)
        {
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }

		bottomLeftExtreme = new Vector2(transform.position.x + playFieldBounds.left, transform.position.y + playFieldBounds.bottom);
		topRightExtreme = new Vector2(transform.position.x + playFieldBounds.right, transform.position.y + playFieldBounds.top);

		displacement3D = new Vector3(blockSize / 2, blockSize / 2, 0);
		displacement2D = new Vector2(blockSize / 2, blockSize / 2);

		blockMax = blockCount - new Vector2(1, 1);
		
		if (isLocalPlayer && isCursorControl)
		{
			cursorPos = new Vector2(Mathf.FloorToInt(blockCount.x / 2) - 1, Mathf.Min(2, blockCount.y));
			
			cursor = Instantiate(cursorPrefab, CoordToPosition((int)cursorPos.x, (int)cursorPos.y, true), Quaternion.identity, gameObject.transform);

			cursorRenderer = cursor.GetComponent<SpriteRenderer>();
		}
		
		highestBlocks = new GameObject[(int)blockCount.x];
		undroppedBlocks = new List<GameObject>[(int)blockCount.x];

		for(int i = 0; i < blockCount.x; i++)
		{
			undroppedBlocks[i] = new List<GameObject>();
		}
		
        if (isServer)
        {
            RandomiseTypes();
        }

		if (isLocalPlayer)
		{
			int index = 0;

			if (isServer)
			{
				index = 1;
			}

			CmdSetupBlocks(transform.position.x + playFieldBounds.left, transform.position.y + playFieldBounds.bottom, index);
		}

		mui = gameObject.GetComponent<MatchUI>();
		atkBar = gameObject.GetComponent<CombatHandler>();
	}

	#region Initial block setup
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
	
	//Spawn blocks over the network
	[Command]
	void CmdSetupBlocks(float xBase, float yBase, int index)
	{
		float yPos;
		
		int yCoord;
		
		for (int y = 0; y < blockCount.y; y++)
		{
			yPos = yBase + y * blockSize;
			
			yCoord = y * (int)blockCount.x;
			
			for (int x = 0; x < blockCount.x; x++)
			{
				Vector3 pos = new Vector3(xBase + x * blockSize, yPos, 0) + displacement3D;
				
				int ID = y * (int)blockCount.x + x;
				GameObject block = Instantiate(blockPrefab, pos, Quaternion.identity, transform);
				
				allBlocks[x, y] = block;
				
				BlockDetails details = allBlocks[x, y].GetComponent<BlockDetails>();

				allBlocksStatic[ID] = details;

				details.managerSearchIndex = index;
				details.blockID = ID;
				details.coords = new Vector2(x, y);
				details.type = (BlockTypes)generatedTypes[yCoord + x];
				details.spriteSheet = spriteSheets[0].spriteSheet;
				
				NetworkServer.SpawnWithClientAuthority(block, connectionToClient);
			}
		}
	}
	#endregion

	//Handle input and chain counting
	void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
		
		#region Swap input
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

						if(blockThis != null && blockThat != null)
						{
							BlockDetails detailsThis = blockThis.GetComponent<BlockDetails>();
							BlockDetails detailsThat = blockThat.GetComponent<BlockDetails>();

							if (detailsThis.isInteractable && detailsThat.isInteractable)
							{
								HandleSwap(new Vector2Int((int)cursorPos.x, (int)cursorPos.y), new Vector2Int(swapIndex, (int)cursorPos.y));
							}
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

						if (blockThis != null && blockThat != null)
						{
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

					if(allBlocks[(int)index.x, (int)index.y] != null)
					{
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
							if(allBlocks[targetCoords.x, targetCoords.y] != null)
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
			}
			else if (Input.GetButtonUp("Select"))
			{
				selectedBlock = null;
			}
		}
		#endregion
		
		#region Debug stuff
		//temp debug. get if any blocks are in the wrong state. hold control to force into the right state
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			BlockDetails details;

			bool isError = false;
			bool isNull = false;

			bool shouldForce = Input.GetKey(KeyCode.RightControl);

			for(int x = 0; x < blockCount.x; x++)
			{
				for(int y = 0; y < blockCount.y; y++)
				{
					if(allBlocks[x, y] != null)
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
					else
					{
						isNull = true;

						print("Null reference at: " + x + ", " + y);
					}
				}
			}

			if (!isError && !isNull)
			{
				print("No state issues or null references");
			}
			else if(isError)
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
		#endregion
	}

	//Combo/chain counting
	private void LateUpdate()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		#region Combo counting
		//If there are any combos, process the data
		int comboCount = comboDetails.Count;

		if (comboCount > 0)
		{
			int comboTotal = 0;

			for (int i = 0; i < comboCount; i++)
			{
				for(int j = 0; j < 5; j++)
				{
					comboTotal += comboDetails[i].comboNumbers[j];
				}

				chainDetails[comboDetails[i].chainIndex].chainNumber++;

				print("Combo total: " + comboTotal);
				print("Chain: " + chainDetails[comboDetails[i].chainIndex].chainNumber);
				print("");
				
				//Parse combo numbers for type specific number of blocks broken
				//Parse comboTotal for total number of blocks broken in the combo
				//Parse chainDetails[comboDetails[i].chainIndex].chainNumber for current chain number
			}

			//Memory management. Clear the list after processing the data
			comboDetails.Clear();
		}
		#endregion

		#region Chain counting
		//Determine if there are still chains going
		bool isStillChaining = false;

		for (int i = 0; i < chainDetails.Count; i++)
		{
			if (chainDetails[i].chainNumber > -1)
			{
				isStillChaining = true;

				break;
			}
		}

		//Memory management. If none are still going, clear the list
		if (!isStillChaining)
		{
			chainDetails.Clear();
		}
		#endregion
	}

	#region Debug stuff
	//temp debug. force type
	void DebugForceType(int type)
	{
		clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (IsInBounds(new Vector2(clickPos.x, clickPos.y)))
		{
			Vector2 fixedPos = new Vector2(clickPos.x, clickPos.y) - bottomLeftExtreme;

			Vector2 index = new Vector2(Mathf.FloorToInt(fixedPos.x / blockSize), Mathf.FloorToInt(fixedPos.y / blockSize));

			if(allBlocks[(int)index.x, (int)index.y] != null)
			{
				BlockDetails details = allBlocks[(int)index.x, (int)index.y].GetComponent<BlockDetails>();

				details.type = (BlockTypes)type;
				details.UpdateType();
			}
		}
	}
	#endregion

	#region IsInBounds
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
	#endregion

	#region Swap logic
	//Swap cooldown
	IEnumerator CursorCooldown()
	{
		canSwap = false;

		if (isCursorControl)
		{
			cursorRenderer.color = Color.black;
		}

		yield return new WaitForSeconds(swapTime);
		yield return new WaitForEndOfFrame();

		canSwap = true;

		if (isCursorControl)
		{
			cursorRenderer.color = Color.white;
		}
	}

	//Handle chaging values, then swap blocks
	void HandleSwap(Vector2Int blockSelected, Vector2Int blockToSwap)
	{
		int swapIndex = chainDetails.Count;
		chainDetails.Add(new ChainDetails(swapIndex, 0));
		
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

		detailsThis.movementCoroutine =  StartCoroutine(SwapBlock(blockThis, detailsThis, blockThis.transform.position, CoordToPosition(blockToSwap.x, blockToSwap.y), swapIndex));
		
		detailsThat.isInteractable = false;
		detailsThat.coords -= swapDirection;
		allBlocks[blockSelected.x, blockSelected.y] = blockThat;

		relevantCoroutine = detailsThat.movementCoroutine;

		if (relevantCoroutine != null)
		{
			StopCoroutine(relevantCoroutine);
		}

		detailsThat.movementCoroutine = StartCoroutine(SwapBlock(blockThat, detailsThat, blockThat.transform.position, CoordToPosition(blockSelected.x, blockSelected.y), swapIndex));
		
		StartCoroutine(CursorCooldown());
	}

	//Swap blocks
	IEnumerator SwapBlock(GameObject obj, BlockDetails details, Vector3 origin, Vector3 destination, int swapIndex)
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

		if (isLocalPlayer)
		{
			CheckForMatches(details.coords, details.type, swapIndex, -1);
		}
	}
	#endregion

	#region Drop logic
	//Handle the logic before and after dropping blocks
	IEnumerator HandleBlockDrop(int xCoord, List<int> yCoords, int chainIndex)
	{
		int yCount = yCoords.Count;
		List<GameObject> droppingBlocks = new List<GameObject>();

		#region Deal with breaking blocks
		//If there are blocks to drop, handle the logic for dropping
		if (yCount > 0)
		{
			//Set values and references
			List<BlockDetails> droppingDetails = new List<BlockDetails>();
			string breakIDs = "";

			for (int i = 0; i < yCount; i++)
			{
				droppingBlocks.Add(allBlocks[xCoord, yCoords[i]]);
				droppingDetails.Add(droppingBlocks[i].GetComponent<BlockDetails>());

				droppingDetails[i].isInteractable = false;
				
				droppingDetails[i].spriteRenderer.color = Color.grey;

				breakIDs += droppingDetails[i].blockID + "/";
			}

			CmdBreakProjectedBlocks(breakIDs);

			//Wait for break
			yield return new WaitForSeconds(0.375f);

			//Find the lowest coords to move blocks to
			Vector3 newPos = CoordToPosition(xCoord, (int)blockCount.y);

			if (highestBlocks[xCoord] != null)
			{
				newPos.y = Mathf.Max(newPos.y, highestBlocks[xCoord].transform.position.y + blockSize);
			}

			//Move blocks to the top and set null references
			for (int i = 0; i < yCount; i++)
			{
				droppingBlocks[i].transform.position = newPos + new Vector3(0, blockSize, 0) * i;
				allBlocks[xCoord, yCoords[i]] = null;
				
				droppingDetails[i].spriteRenderer.color = Color.white;
			}

			highestBlocks[xCoord] = droppingBlocks[yCount - 1];
		}
		#endregion

		#region Drop blocks above
		//Tell blocks above to fall
		GameObject block;
		BlockDetails details;
		Coroutine relevantCoroutine;

		int nullCount = 0;

		bool shouldContinue = true;

		for(int y = 0; y < blockCount.y; y++)
		{
			block = allBlocks[xCoord, y];

			if (block == null)
			{
				nullCount++;
			}
			else
			{
				if(nullCount > 0)
				{
					details = block.GetComponent<BlockDetails>();

					if (!(details.isFalling || details.isInteractable))
					{
						shouldContinue = false;

						break;
					}

					details.isInteractable = false;
					details.isFalling = true;

					allBlocks[xCoord, (int)details.coords.y] = null;
					details.coords.y -= nullCount;
					allBlocks[xCoord, (int)details.coords.y] = details.gameObject;
					
					if (details.chainIndex == -1)
					{
						details.chainIndex = chainIndex;

						chainDetails[chainIndex].involvedIDs.Add(details.blockID);
					}

					relevantCoroutine = details.movementCoroutine;

					if (relevantCoroutine != null)
					{
						StopCoroutine(relevantCoroutine);
					}

					details.movementCoroutine = StartCoroutine(DropBlock(details.gameObject, details, CoordToPosition(xCoord, (int)details.coords.y), chainIndex));
				}
			}
		}
		#endregion
		
		//If no uninteractable blocks were encountered, drop the blocks sent to the top
		if (shouldContinue && nullCount > 0)
		{
			List<int> targetIDs = new List<int>();

			List<GameObject> blocksToDrop = new List<GameObject>();
			List<int> duplicateIndices = new List<int>();

			#region Deal with undropped blocks
			//If there are undropped blocks, add them to the list to drop
			int undroppedCount = undroppedBlocks[xCoord].Count;

			if (undroppedBlocks[xCoord].Count > 0)
			{
				for (int i = 0; i < undroppedBlocks[xCoord].Count; i++)
				{
					blocksToDrop.Add(undroppedBlocks[xCoord][i]);
				}
				
				undroppedBlocks[xCoord].Clear();
			}

			for (int i = 0; i < yCount; i++)
			{
				blocksToDrop.Add(droppingBlocks[i]);
			}
			#endregion

			#region Deal with duplicates
			//Check for duplicates references in the blocks to drop
			for (int i = 0; i < blocksToDrop.Count; i++)
			{
				for (int j = i + 1; j < blocksToDrop.Count; j++)
				{
					if (blocksToDrop[i] == blocksToDrop[j])
					{
						bool isAlreadyNoted = false;

						for (int a = 0; a < duplicateIndices.Count; a++)
						{
							if(duplicateIndices[a] == j)
							{
								isAlreadyNoted = true;
							}
						}

						if (!isAlreadyNoted)
						{
							duplicateIndices.Add(j);
						}
					}
				}
			}
			
			int duplicateCount = duplicateIndices.Count;

			//If there are duplicates, remove them
			if (duplicateCount > 0)
			{
				for(int i = 0; i < undroppedCount; i++)
				{
					Vector2 xy = blocksToDrop[i].GetComponent<BlockDetails>().coords;
				}
				
				for (int i = undroppedCount; i < blocksToDrop.Count; i++)
				{
					Vector2 xy = blocksToDrop[i].GetComponent<BlockDetails>().coords;
				}
				
				duplicateIndices.Sort();
				duplicateIndices.Reverse();

				int dropCount = blocksToDrop.Count;

				for (int i = 0; i < duplicateCount; i++)
				{
					blocksToDrop.RemoveAt(duplicateIndices[i]);
				}
			}
			#endregion

			//Sync values
			string changeIDs = "";
			string changeTypes = "";

			#region Drop blocks above the board
			//Drop the blocks
			for (int i = 0; i < blocksToDrop.Count; i++)
			{
				details = blocksToDrop[i].GetComponent<BlockDetails>();
					
				details.isFalling = true;

				int newY = (int)blockCount.y - nullCount;
				
				if(newY > 9)
				{
					break;
				}

				details.coords = new Vector2(xCoord, newY);
				allBlocks[xCoord, newY] = details.gameObject;
				targetIDs.Add(details.blockID);
				
				BlockTypes newType = GenerateRandomType(new bool[5] { true, true, true, true, true });

				details.type = newType;
				details.UpdateType();

				changeIDs += details.blockID + "/";
				changeTypes += (int)newType + "/";

				if (details.chainIndex == -1)
				{
					details.chainIndex = chainIndex;

					chainDetails[chainIndex].involvedIDs.Add(details.blockID);
				}

				relevantCoroutine = details.movementCoroutine;

				if (relevantCoroutine != null)
				{
					StopCoroutine(relevantCoroutine);
				}

				details.movementCoroutine = StartCoroutine(DropBlock(details.gameObject, details, CoordToPosition(xCoord, newY), chainIndex));
				
				nullCount--;
			}

			if(changeIDs.Length > 0)
			{
				CmdUpdateProjectedBlocks(changeIDs, changeTypes);
			}
			#endregion
		}
		else
		{
			//If there were no empty spaces, start process of clearing a chainDetail
			if(nullCount == 0)
			{
				if(chainDetails[chainIndex].involvedIDs.Count == 0)
				{
					if (chainDetails[chainIndex].shouldClear)
					{
						chainDetails[chainIndex].chainNumber = -1;
					}
					else
					{
						chainDetails[chainIndex].shouldClear = true;
					}
				}
			}

			//If an uninteractable block was encountered, add the blocks sent to the top to a list
			for (int i = 0; i < yCount; i++)
			{
				undroppedBlocks[xCoord].Add(droppingBlocks[i]);
			}
		}
	}

	#region Sync Breaking
	//Command projected blocks to break
	[Command]
	void CmdBreakProjectedBlocks(string listIDs)
	{
		RpcBreakProjectedBlocks(listIDs);
	}

	//Start breaking projected blocks
	[ClientRpc]
	void RpcBreakProjectedBlocks(string listIDs)
	{
		if (isLocalPlayer)
		{
			return;
		}

		StartCoroutine(BreakProjectedBlocks(listIDs));
	}

	//Break and restore projected blocks
	IEnumerator BreakProjectedBlocks(string listIDs)
	{
		string[] stringIDs = listIDs.Split('/');
		BlockDetails[] allDetails = new BlockDetails[stringIDs.Length - 1];

		for (int i = 0; i < stringIDs.Length - 1; i++)
		{
			allDetails[i] = allBlocksStatic[int.Parse(stringIDs[i])];

			allDetails[i].spriteRenderer.color = Color.grey;
		}

		yield return new WaitForSeconds(0.375f);

		foreach(BlockDetails details in allDetails)
		{
			details.spriteRenderer.color = Color.white;
		}
	}
	#endregion

	#region Sync new types
	//Command projected blocks to break
	[Command]
	void CmdUpdateProjectedBlocks(string listIDs, string listTypes)
	{
		RpcUpdateProjectedBlocks(listIDs, listTypes);
	}

	//Start breaking projected blocks
	[ClientRpc]
	void RpcUpdateProjectedBlocks(string listIDs, string listTypes)
	{
		if (isLocalPlayer)
		{
			return;
		}

		StartCoroutine(UpdateProjectedBlocks(listIDs, listTypes));
	}

	//Break and restore projected blocks
	IEnumerator UpdateProjectedBlocks(string listIDs, string listTypes)
	{
		string[] stringIDs = listIDs.Split('/');
		string[] stringTypes = listTypes.Split('/');
		
		yield return new WaitForEndOfFrame();

		for (int i = 0; i < stringIDs.Length - 1; i++)
		{
			BlockDetails details = allBlocksStatic[int.Parse(stringIDs[i])];

			details.type = (BlockTypes)int.Parse(stringTypes[i]);
			details.UpdateType();
		}
	}
	#endregion
	
	//Control block falling and align if necessary
	IEnumerator DropBlock(GameObject obj, BlockDetails details, Vector3 destination, int chainIndex)
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
	#endregion

	#region Match check logic
	//Checks for matches and move blocks accordingly
	void CheckForMatches(Vector2 pos, BlockTypes relevantType, int swapIndex, int chainIndex)
	{
		if(!allBlocks[(int)pos.x, (int)pos.y].GetComponent<BlockDetails>().isInteractable)
		{
			return;
		}

		#region Check for matches
		List<int> matchingHorizontalIndices = new List<int>();
		List<int> matchingVerticalIndices = new List<int>() { (int)pos.y };
		
		//Count matching blocks
		for (int i = (int)pos.x - 1; i > -1; i--)
		{
			if(CheckBlock(i, (int)pos.y, relevantType))
			{
				matchingHorizontalIndices.Add(i);
			}
			else
			{
				break;
			}
		}
		
		for (int i = (int)pos.x + 1; i < blockCount.x; i++)
		{
			if (CheckBlock(i, (int)pos.y, relevantType))
			{
				matchingHorizontalIndices.Add(i);
			}
			else
			{
				break;
			}
		}
		
		for (int i = (int)pos.y - 1; i > -1; i--)
		{
			if (CheckBlock((int)pos.x, i, relevantType))
			{
				matchingVerticalIndices.Add(i);
			}
			else
			{
				break;
			}
		}
		
		for (int i = (int)pos.y + 1; i < blockCount.y; i++)
		{
			if (CheckBlock((int)pos.x, i, relevantType))
			{
				matchingVerticalIndices.Add(i);
			}
			else
			{
				break;
			}
		}
		#endregion

		//If no vertical match was made, add the swapped block to horizontal matches
		if (matchingHorizontalIndices.Count >= 2 && matchingVerticalIndices.Count < 3)
		{
			matchingHorizontalIndices.Add((int)pos.x);
		}

		#region Calculate combos and chains
		//Calculate combos and chains
		int comboCount = 0;
		int newChainIndex = chainIndex;
		
		if(matchingHorizontalIndices.Count >= 2 || matchingVerticalIndices.Count >= 3)
		{
			//If a match was made and the player is dragging the matched block, deselect the block
			if (isLocalPlayer && !isCursorControl && selectedDetails.coords == pos)
			{
				selectedBlock = null;
			}

			#region Combo counting
			//Count matching blocks to determine combo count
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

			//If this was caused by a swap, make the combo contribute to the swap
			if (chainIndex == -1)
			{
				newChainIndex = swapIndex;
			}

			//Determine if there is already a combo from this chain goining on
			bool doesComboExist = false;
			int comboIndex = -1;

			for(int i = 0; i < comboDetails.Count; i++)
			{
				if(comboDetails[i].chainIndex == newChainIndex)
				{
					doesComboExist = true;
					comboIndex = i;

					break;
				}
			}

			//If it does exist, add to it
			if (doesComboExist)
			{
				comboDetails[comboIndex].comboNumbers[(int)relevantType] += comboCount;
			}
			//Else, create it
			else
			{
				int[] comboInput = new int[5] { 0, 0, 0, 0, 0 };
				comboInput[(int)relevantType] = comboCount;

				comboDetails.Add(new ComboDetails(newChainIndex, comboInput));
			}
			#endregion
			
			//will this stuff still be used? Either way, it won't be used here
			#region Old UI stuff
			//print(allChains[newChainIndex]);
			//mui.UpdateChains(allChains[newChainIndex]);
			//mui.UpdateCombo(comboCount);
			#endregion
			
			//put this elsewhere
			atkBar.FillBar(relevantType, chainDetails[newChainIndex].chainNumber, comboCount);
        }
		else if(matchingHorizontalIndices.Count < 2 || matchingVerticalIndices.Count < 3)
		{
			int index = swapIndex;

			if(index == -1)
			{
				index = chainIndex;
			}

			StartCoroutine(HandleBlockDrop((int)pos.x, new List<int>(), index));

			//If this block is part of a chain, check if the chain should end
			if(chainIndex > -1)
			{
				BlockDetails details = allBlocks[(int)pos.x, (int)pos.y].GetComponent<BlockDetails>();

				int relevantIndex = details.chainIndex;

				chainDetails[relevantIndex].involvedIDs.Remove(details.blockID);
				details.chainIndex = -1;

				if(chainDetails[relevantIndex].involvedIDs.Count == 0)
				{
					//retrieve the total chain number from the relevant chain here

					print(chainDetails[relevantIndex].chainNumber);

					chainDetails[relevantIndex].chainNumber = -1;
				}
			}
		}
		#endregion

		#region Handle breaking and dropping
		//Break matched blocks and drop new blocks
		if (matchingHorizontalIndices.Count >= 2)
		{
			for (int x = 0; x < matchingHorizontalIndices.Count; x++)
			{
				StartCoroutine(HandleBlockDrop(matchingHorizontalIndices[x], new List<int>() { (int)pos.y }, newChainIndex));
			}
		}

		if (matchingVerticalIndices.Count >= 3)
		{
			matchingVerticalIndices.Sort();
			
			StartCoroutine(HandleBlockDrop((int)pos.x, matchingVerticalIndices, newChainIndex));
		}
		#endregion
	}

	//Check if the block at the provided indices is not null and is the right type
	bool CheckBlock(int x, int y, BlockTypes type)
	{
		if (allBlocks[x, y] == null)
		{
			return false;
		}

		BlockDetails details = allBlocks[x, y].GetComponent<BlockDetails>();

		if (details.type == type && details.isInteractable)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	#endregion

	#region CoordToPosition
	//Wrapper for a default location
	Vector3 CoordToPosition(int x, int y)
	{
		return CoordToPosition(x, y, false);
	}

	//might try use a parameter to specify z
	//is this overload necessary
	//might be useful if the old cursor style is restored
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
	#endregion
}
