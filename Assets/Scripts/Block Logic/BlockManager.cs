using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

public class BlockManager : MonoBehaviour
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
	private Vector3 displacement;

	//Block count in the field
	[Tooltip("The number of blocks horizontally (x) and vertically (y) across the field.")]
	public Vector2 blockCount = new Vector2(8, 8);
	private Vector2 blockMax;

	[Space(10)]

	//Block rarity
	[Tooltip("The rarity of each block type.")]
	public int[] blockRarities = new int[5] { 200, 200, 200, 200, 25};

	//All blocks
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
	private bool canCursorSwap = true;
	
	//Selected block
	private GameObject selectedBlock;

	//Movement coroutines
	private Coroutine[,] blockCoroutines;

	//Mouse position
	private Vector3 clickPos;
	private Vector3 releasePos;

	[Space(10)]

	//Deadzone
	[Tooltip("The distance movement must exceed to count as a move.")]
	public float deadzone = 0.25f;
	
	//Chain counting list
	private List<int> allChains = new List<int>();

	private MatchUI mui;

	//Setup
	void Start()
    {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		bottomLeftExtreme = new Vector2(transform.position.x + playFieldBounds.left, transform.position.y + playFieldBounds.bottom);
		topRightExtreme = new Vector2(transform.position.x + playFieldBounds.right, transform.position.y + playFieldBounds.top);

		displacement = new Vector3(blockSize / 2, blockSize / 2, 0);

		blockMax = blockCount - new Vector2(1, 1);

		blockCoroutines = new Coroutine[(int)blockCount.x, (int)blockCount.y];
		
		if (isCursorControl)
		{
			cursorPos = new Vector2(Mathf.FloorToInt(blockCount.x / 2) - 1, Mathf.Min(2, blockCount.y));
			
			cursor = Instantiate(cursorPrefab, CoordToPosition((int)cursorPos.x, (int)cursorPos.y, true), Quaternion.identity, gameObject.transform);

			cursorRenderer = cursor.GetComponent<SpriteRenderer>();
		}

		mui = gameObject.GetComponent<MatchUI>();

		SetupBlocks();
    }

	//Setup initial blocks
	void SetupBlocks()
	{
		allBlocks = new GameObject[(int)blockCount.x, (int)blockCount.y];

		float xBase = transform.position.x + playFieldBounds.left;
		float yBase = transform.position.y + playFieldBounds.bottom;
		
		float yPos;
		
		for (int y = 0; y < blockCount.y; y++)
		{
			yPos = yBase + y * blockSize;

			for (int x = 0; x < blockCount.x; x++)
			{
				Vector3 pos = new Vector3(xBase + x * blockSize, yPos, 0) + displacement;

				allBlocks[x, y] = Instantiate(blockPrefab, pos, Quaternion.identity, transform);
				
				BlockDetails details = allBlocks[x, y].GetComponent<BlockDetails>();

				details.coords = new Vector2(x, y);

				//input code here
				bool[] typeInclusivity = new bool[5] { true, true, true, true, true };
				BlockTypes tempType;

				if(x > 1)
				{
					tempType = allBlocks[x - 1, y].GetComponent<BlockDetails>().type;

					if(tempType == allBlocks[x - 2, y].GetComponent<BlockDetails>().type)
					{
						typeInclusivity[(int)tempType] = false;
					}
				}

				if (y > 1)
				{
					tempType = allBlocks[x, y - 1].GetComponent<BlockDetails>().type;

					if (tempType == allBlocks[x, y - 2].GetComponent<BlockDetails>().type)
					{
						typeInclusivity[(int)tempType] = false;
					}
				}
				
				details.type = GenerateRandomType(typeInclusivity);
			}
		}
	}

	void GenerateAllBlocks()
	{

	}

	BlockTypes GenerateRandomType(bool[] typeInclusivity)
	{
		int maxChance = 0;

		for(int i = 0; i < typeInclusivity.Length; i++)
		{
			if (typeInclusivity[i])
			{
				maxChance += blockRarities[i];
			}
		}

		int randNum = Random.Range(0, maxChance);
		int chanceStage = 0;

		for(int i = 0; i < typeInclusivity.Length; i++)
		{
			if (typeInclusivity[i])
			{
				chanceStage += blockRarities[i];

				if(randNum < chanceStage)
				{
					return (BlockTypes)i;
				}
			}
		}

		return BlockTypes.A;
	}

    // Update is called once per frame
    void Update()
    {
		if (isCursorControl)
		{
			if (Input.GetButtonDown("Horizontal"))
			{
				cursorPos.x = Mathf.Clamp(cursorPos.x + 1 * Mathf.Sign(Input.GetAxisRaw("Horizontal")), 0, blockMax.x - 1);

				cursor.transform.position = CoordToPosition((int)cursorPos.x, (int)cursorPos.y, true);
			}

			if (Input.GetButtonDown("Vertical"))
			{
				cursorPos.y = Mathf.Clamp(cursorPos.y + 1 * Mathf.Sign(Input.GetAxisRaw("Vertical")), 0, blockMax.y);

				cursor.transform.position = CoordToPosition((int)cursorPos.x, (int)cursorPos.y, true);
			}

			if (canCursorSwap && Input.GetButtonDown("Select"))
			{
				GameObject blockThis = allBlocks[(int)cursorPos.x, (int)cursorPos.y];
				GameObject blockThat = allBlocks[(int)cursorPos.x + 1, (int)cursorPos.y];

				BlockDetails detailsThis = blockThis.GetComponent<BlockDetails>();
				BlockDetails detailsThat = blockThat.GetComponent<BlockDetails>();

				if(detailsThis.isInteractable && detailsThat.isInteractable)
				{
					Coroutine relevantCoroutine;
					
					Vector3 posThis = blockThis.transform.position;
					Vector3 posThat = blockThat.transform.position;

					detailsThis.isInteractable = false;
					detailsThis.coords = detailsThat.coords;
					allBlocks[(int)detailsThis.coords.x, (int)detailsThis.coords.y] = blockThis;

					relevantCoroutine = blockCoroutines[(int)detailsThis.coords.x, (int)detailsThis.coords.y];

					if (relevantCoroutine != null)
					{
						StopCoroutine(relevantCoroutine);
					}

					blockCoroutines[(int)detailsThis.coords.x, (int)detailsThis.coords.y] = StartCoroutine(SwapBlock(blockThis, detailsThis, posThis, posThat));

					detailsThat.isInteractable = false;
					detailsThat.coords = detailsThis.coords - new Vector2(1, 0);
					allBlocks[(int)detailsThat.coords.x, (int)detailsThat.coords.y] = blockThat;

					relevantCoroutine = blockCoroutines[(int)detailsThat.coords.x, (int)detailsThat.coords.y];

					if (relevantCoroutine != null)
					{
						StopCoroutine(relevantCoroutine);
					}

					blockCoroutines[(int)detailsThat.coords.x, (int)detailsThat.coords.y] = StartCoroutine(SwapBlock(blockThat, detailsThat, posThat, posThis));

					StartCoroutine(CursorCooldown());
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

					selectedBlock = allBlocks[(int)index.x, (int)index.y];
				}
			}
			else if (Input.GetButtonUp("Select") && selectedBlock != null)
			{
				releasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				BlockDetails detailsThis = selectedBlock.GetComponent<BlockDetails>();
				BlockDetails detailsThat = null;

				Coroutine relevantCoroutine;

				if (detailsThis.isInteractable)
				{
					if (Mathf.Abs(clickPos.x - releasePos.x) > deadzone)
					{
						int alt = 0;

						if (releasePos.x < clickPos.x)
						{
							if (detailsThis.coords.x - 1 >= 0)
							{
								detailsThat = allBlocks[(int)detailsThis.coords.x - 1, (int)detailsThis.coords.y].GetComponent<BlockDetails>();

								if (detailsThis.coords.x > 0 && detailsThat.isInteractable)
								{
									alt = -1;
								}
							}
						}
						else
						{
							if (detailsThis.coords.x + 1 <= blockMax.x)
							{
								detailsThat = allBlocks[(int)detailsThis.coords.x + 1, (int)detailsThis.coords.y].GetComponent<BlockDetails>();

								if (detailsThis.coords.x < blockMax.x && detailsThat.isInteractable)
								{
									alt = 1;
								}
							}
						}

						if (alt != 0)
						{
							GameObject otherBlock = detailsThat.gameObject;

							Vector3 posThis = selectedBlock.transform.position;
							Vector3 posThat = otherBlock.transform.position;

							detailsThis.isInteractable = false;
							detailsThis.coords = detailsThat.coords;
							allBlocks[(int)detailsThis.coords.x, (int)detailsThis.coords.y] = selectedBlock;

							relevantCoroutine = blockCoroutines[(int)detailsThis.coords.x, (int)detailsThis.coords.y];

							if (relevantCoroutine != null)
							{
								StopCoroutine(relevantCoroutine);
							}

							blockCoroutines[(int)detailsThis.coords.x, (int)detailsThis.coords.y] = StartCoroutine(SwapBlock(selectedBlock, detailsThis, posThis, posThat));

							detailsThat.isInteractable = false;
							detailsThat.coords = detailsThis.coords - new Vector2(1, 0) * alt;
							allBlocks[(int)detailsThat.coords.x, (int)detailsThat.coords.y] = otherBlock;

							relevantCoroutine = blockCoroutines[(int)detailsThat.coords.x, (int)detailsThat.coords.y];

							if (relevantCoroutine != null)
							{
								StopCoroutine(relevantCoroutine);
							}

							blockCoroutines[(int)detailsThat.coords.x, (int)detailsThat.coords.y] = StartCoroutine(SwapBlock(detailsThat.gameObject, detailsThat, posThat, posThis));
						}
					}
				}

				selectedBlock = null;
			}
		}

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

		if (Input.GetKeyDown(KeyCode.P))
		{
			SceneManager.LoadScene("main");
		}
	}

	//Get a random type
	BlockTypes GetRandomType(int input)
	{
		switch (input)
		{
			case 0:
				return BlockTypes.A;
			case 1:
				return BlockTypes.B;
			case 2:
				return BlockTypes.C;
			case 3:
				return BlockTypes.D;
			case 4:
				return BlockTypes.E;
			default:
				return BlockTypes.A;
		}
	}

	//Are the supplied co ordinates within the bounds
	bool IsInBounds(Vector2 input)
	{
		return bottomLeftExtreme.x < input.x && input.x < topRightExtreme.x && bottomLeftExtreme.y < input.y && input.y < topRightExtreme.y;
	}
	
	//Cursor cooldown
	IEnumerator CursorCooldown()
	{
		canCursorSwap = false;

		cursorRenderer.color = Color.black;

		yield return new WaitForSeconds(swapTime);

		canCursorSwap = true;

		cursorRenderer.color = Color.white;
	}

	//Horizontally swap blocks
	IEnumerator SwapBlock(GameObject obj, BlockDetails details, Vector3 origin, Vector3 destination)
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

		CheckForMatches(details.coords, details.type, -1);
	}
	
	//Control block falling and align if necessary
	IEnumerator DropBlock(GameObject obj, BlockDetails details, Vector3 destination, int chainIndex)
	//IEnumerator DropBlock(GameObject obj, BlockDetails details, Vector3 destination, int chainIndex, bool resetBlock)
	{
		/*float elapsedTime = 0.0f;

		BlockDetails blockBelow = allBlocks[(int)details.coords.x, Mathf.Max((int)details.coords.y - 1, 0)].GetComponent<BlockDetails>();

		if (!details.isFalling)
		{
			while(elapsedTime < 0.5f || !blockBelow.isFalling)
			{
				elapsedTime += Time.deltaTime;

				yield return new WaitForEndOfFrame();
			}
		}

		details.isInteractable = false;



		if (details.chainIndex == -1)
		{
			details.isFalling = true;
			details.chainIndex = chainIndex;
		}*/

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

		yield return new WaitForEndOfFrame();

		CheckForMatches(details.coords, details.type, chainIndex);
	}

	//Checks for matches and move blocks accordingly
	void CheckForMatches(Vector2 pos, BlockTypes relevantType, int chainIndex)
	{
		BlockDetails details;
		Coroutine relevantCoroutine;

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
				newChainIndex = allChains.Count;
				
				allChains.Add(1);
			}
			else if(chainIndex > -1)
			{
				allChains[chainIndex] += 1;
			}

			//print(allChains[newChainIndex]);
			mui.UpdateChains(allChains[newChainIndex]);
			mui.UpdateCombo(comboCount);

			//to retrieve chain count for current swap, use allChains[chainIndex]
		}
		else if(matchingHorizontalIndices.Count < 2 || matchingVerticalIndices.Count < 3)
		{
			if(chainIndex > -1)
			{
				bool areStillFalling = false;

				details = allBlocks[(int)pos.x, (int)pos.y].GetComponent<BlockDetails>();

				details.isFalling = false;
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

		if (matchingHorizontalIndices.Count >= 2)
		{
			int blockX;
			int xFallCount = (int)blockMax.y - (int)pos.y;

			GameObject oldBlock = null;
			Vector3 blockPos;
			
			for (int x = 0; x < matchingHorizontalIndices.Count; x++)
			{
				blockX = matchingHorizontalIndices[x];
				
				details = allBlocks[blockX, (int)pos.y].GetComponent<BlockDetails>();

				oldBlock = allBlocks[blockX, (int)blockMax.y];

				details.isInteractable = false;
				details.coords = new Vector2(blockX, blockMax.y);
				allBlocks[blockX, (int)blockMax.y] = details.gameObject;
				//details.type = GetRandomType(Random.Range(0, 5));
				details.type = GenerateRandomType(new bool[5] { true, true, true, true, true});
				details.UpdateColour();
				blockPos = oldBlock.transform.position;
				blockPos = CoordToPosition(blockX, (int)blockMax.y);
				details.gameObject.transform.position = blockPos + new Vector3(0, blockSize, 0);
				
				relevantCoroutine = blockCoroutines[(int)details.coords.x, (int)details.coords.y];

				if (relevantCoroutine != null)
				{
					StopCoroutine(relevantCoroutine);
				}

				blockCoroutines[(int)details.coords.x, (int)details.coords.y] = StartCoroutine(DropBlock(details.gameObject, details, blockPos, newChainIndex));
				
				//StartCoroutine(FadeBlock(details.gameObject));

				if(details.chainIndex == -1)
				{
					details.isFalling = true;
					details.chainIndex = newChainIndex;
				}

				if (pos.y < blockMax.y)
				{
					for (int y = (int)blockMax.y; y > pos.y; y--)
					{
						details = oldBlock.GetComponent<BlockDetails>();
						
						if(y - 1 > pos.y)
						{
							oldBlock = allBlocks[blockX, y - 1];
						}
						
						details.isInteractable = false;
						details.coords = new Vector2(blockX, y - 1);
						allBlocks[blockX, (int)details.coords.y] = details.gameObject;
						
						relevantCoroutine = blockCoroutines[(int)details.coords.x, (int)details.coords.y];
						
						if (relevantCoroutine != null)
						{
							StopCoroutine(relevantCoroutine);
						}
						
						blockCoroutines[(int)details.coords.x, (int)details.coords.y] = StartCoroutine(DropBlock(details.gameObject, details, CoordToPosition(blockX, y - 1), newChainIndex));
						
						if (details.chainIndex == -1)
						{
							details.isFalling = true;
							details.chainIndex = newChainIndex;
						}
					}
				}
			}
		}
		
		if (matchingVerticalIndices.Count >= 3)
		{
			int verticalMatchCount = matchingVerticalIndices.Count;
			
			List<int> oldIndices = new List<int>();
			List<int> newIndexOrder = new List<int>();
			List<GameObject> newBlockOrder = new List<GameObject>();
			
			matchingVerticalIndices.Sort();
			
			if(matchingVerticalIndices[verticalMatchCount - 1] < blockMax.y)
			{
				for (int i = (int)blockMax.y; i > matchingVerticalIndices[verticalMatchCount - 1]; i--)
				{
					oldIndices.Add(i);
				}
			}
			
			oldIndices.Sort();
			
			for(int i = 0; i < oldIndices.Count; i++)
			{
				newIndexOrder.Add(oldIndices[i]);
			}

			for(int i = 0; i < verticalMatchCount; i++)
			{
				newIndexOrder.Add(matchingVerticalIndices[i]);
			}

			for(int i = 0; i < newIndexOrder.Count; i++)
			{
				newBlockOrder.Add(allBlocks[(int)pos.x, newIndexOrder[i]]);
			}

			int targetIndex;

			int resetBlockCount = 1;

			for (int i = 0; i < newIndexOrder.Count; i++)
			{
				targetIndex = (int)blockMax.y - newIndexOrder.Count + i + 1;
				
				details = newBlockOrder[i].GetComponent<BlockDetails>();
				
				details.isInteractable = false;
				details.coords = new Vector2((int)pos.x, targetIndex);
				allBlocks[(int)pos.x, targetIndex] = details.gameObject;

				if(i > oldIndices.Count - 1)
				{
					//details.type = GetRandomType(Random.Range(0, 5));
					details.type = GenerateRandomType(new bool[5] { true, true, true, true, true });
					details.UpdateColour();
					
					details.gameObject.transform.position = CoordToPosition((int)pos.x, (int)blockMax.y + resetBlockCount);

					resetBlockCount++;
				}

				relevantCoroutine = blockCoroutines[(int)details.coords.x, (int)details.coords.y];
				
				if (relevantCoroutine != null)
				{
					StopCoroutine(relevantCoroutine);
				}
				
				blockCoroutines[(int)details.coords.x, (int)details.coords.y] = StartCoroutine(DropBlock(details.gameObject, details, CoordToPosition((int)pos.x, targetIndex), newChainIndex));

				if (details.chainIndex == -1)
				{
					details.isFalling = true;
					details.chainIndex = newChainIndex;
				}
			}
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
		float extraDisplacement = blockSize / 2;
		float extraZ = -1.25f;

		if (!useExtraDisplacement)
		{
			extraDisplacement = 0;
			extraZ = 0;
		}

		return transform.position + new Vector3((x - (blockCount.x / 2)) * blockSize + extraDisplacement, (y - (blockCount.y / 2)) * blockSize, extraZ) + displacement;
	}

	//temp. fade
	IEnumerator FadeBlock(GameObject obj)
	{
		SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
		Color newColour = sr.color;

		float elapsedTime = 0.0f;

		while(elapsedTime < 0.5f)
		{
			elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, 0.5f);

			newColour.a = 1 - (elapsedTime / 0.5f);

			sr.color = newColour;

			yield return new WaitForEndOfFrame();
		}
	}
}
