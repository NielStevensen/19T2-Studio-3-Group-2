using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	
	//All blocks
	private GameObject[,] allBlocks;

	//Selected block
	public GameObject selectedBlock;

	//Mouse position
	private Vector3 clickPos;
	private Vector3 releasePos;

	//Deadzone
	[Space(10)]
	[Tooltip("The distance movement must exceed to count as a move.")]
	public float deadzone = 0.25f;
	
	//Setup
    void Start()
    {
		bottomLeftExtreme = new Vector2(transform.position.x + playFieldBounds.left, transform.position.y + playFieldBounds.bottom);
		topRightExtreme = new Vector2(transform.position.x + playFieldBounds.right, transform.position.y + playFieldBounds.top);

		displacement = new Vector3(blockSize / 2, blockSize / 2, 0);

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
				details.type = GetRandomType(Random.Range(0, 5));
			}
		}
	}

    // Update is called once per frame
    void Update()
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
		else if(Input.GetButtonUp("Select") && selectedBlock != null)
		{
			releasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			BlockDetails detailsThis = selectedBlock.GetComponent<BlockDetails>();
			BlockDetails detailsThat = null;

			if (detailsThis.isInteractable)
			{
				if (Mathf.Abs(clickPos.x - releasePos.x) > deadzone)
				{
					int alt = 0;

					if (releasePos.x < clickPos.x)
					{
						if(detailsThis.coords.x - 1 >= 0)
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
						if(detailsThis.coords.x + 1 <= 7)
						{
							detailsThat = allBlocks[(int)detailsThis.coords.x + 1, (int)detailsThis.coords.y].GetComponent<BlockDetails>();

							if (detailsThis.coords.x < 7 && detailsThat.isInteractable)
							{
								alt = 1;
							}
						}
					}

					if(alt != 0)
					{
						GameObject otherBlock = detailsThat.gameObject;

						Vector3 posThis = selectedBlock.transform.position;
						Vector3 posThat = otherBlock.transform.position;
						
						detailsThis.isInteractable = false;
						detailsThis.coords = detailsThat.coords;
						allBlocks[(int)detailsThis.coords.x, (int)detailsThis.coords.y] = selectedBlock;
						StartCoroutine(MoveBlock(selectedBlock, detailsThis, posThis, posThat));

						detailsThat.isInteractable = false;
						detailsThat.coords = detailsThis.coords - new Vector2(1, 0) * alt;
						allBlocks[(int)detailsThat.coords.x, (int)detailsThat.coords.y] = otherBlock;
						StartCoroutine(MoveBlock(detailsThat.gameObject, detailsThat, posThat, posThis));
					}
				}
			}
			
			selectedBlock = null;
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

	//Default move blocks wrapper
	IEnumerator MoveBlock(GameObject obj, BlockDetails details, Vector3 origin, Vector3 destination)
	{
		StartCoroutine(MoveBlock(obj, details, origin, destination, 0.5f));

		yield return new WaitForEndOfFrame();
	}

	//Move blocks
	IEnumerator MoveBlock(GameObject obj, BlockDetails details, Vector3 origin, Vector3 destination, float time)
	{
		float elapsedTime = 0.0f;

		Vector3 direction = destination - origin;

		while (elapsedTime < time)
		{
			elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, time);

			obj.transform.position = origin + direction * Mathf.Sin(Mathf.Deg2Rad * 90 * (elapsedTime / time));

			yield return new WaitForEndOfFrame();
		}

		details.isInteractable = true;

		yield return new WaitForEndOfFrame();

		CheckForMatches(details.coords, details.type);
	}

	//Checks for matches and move blocks accordingly
	void CheckForMatches(Vector2 pos, BlockTypes relevantType)
	{
		BlockDetails details;

		List<int> matchingHorizontalIndices = new List<int>();
		List<int> matchingVerticalIndices = new List<int>() { (int)pos.y };
		
		for (int i = 1; i < 8; i++)
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
		
		for (int i = 1; i < 8; i++)
		{
			if (pos.x + i > 7)
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

		for (int i = 1; i < 8; i++)
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

		for (int i = 1; i < 8; i++)
		{
			if (pos.y + i > 7)
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

		if(matchingHorizontalIndices.Count >= 2 && matchingVerticalIndices.Count < 3)
		{
			matchingHorizontalIndices.Add((int)pos.x);
		}

		if (matchingHorizontalIndices.Count >= 2)
		{
			int blockX;
			int xFallCount = 7 - (int)pos.y;

			GameObject oldBlock = null;
			Vector3 blockPos;
			
			for (int x = 0; x < matchingHorizontalIndices.Count; x++)
			{
				blockX = matchingHorizontalIndices[x];
				
				details = allBlocks[blockX, (int)pos.y].GetComponent<BlockDetails>();

				oldBlock = allBlocks[blockX, 7];

				details.isInteractable = false;
				details.coords = new Vector2(blockX, 7);
				allBlocks[blockX, 7] = details.gameObject;
				details.type = GetRandomType(Random.Range(0, 5));
				details.UpdateColour();
				blockPos = oldBlock.transform.position;
				StartCoroutine(MoveBlock(details.gameObject, details, blockPos + new Vector3(0, blockSize, 0), blockPos));

				if (pos.y < 7)
				{
					for (int y = 7; y > pos.y; y--)
					{
						details = oldBlock.GetComponent<BlockDetails>();

						if(y - 1 > pos.y)
						{
							oldBlock = allBlocks[blockX, y - 1];
						}
						
						details.isInteractable = false;
						details.coords = new Vector2(blockX, y - 1);
						allBlocks[blockX, (int)details.coords.y] = details.gameObject;
						blockPos = details.gameObject.transform.position;
						StartCoroutine(MoveBlock(details.gameObject, details, blockPos, blockPos - new Vector3(0, blockSize, 0)));
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
			
			if(matchingVerticalIndices[verticalMatchCount - 1] < 7)
			{
				for (int i = 7; i > matchingVerticalIndices[verticalMatchCount - 1]; i--)
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

			for (int i = 0; i < newIndexOrder.Count; i++)
			{
				details = newBlockOrder[i].GetComponent<BlockDetails>();
				
				details.isInteractable = false;
				details.coords = new Vector2((int)pos.x, 8 - newIndexOrder.Count + i);
				allBlocks[(int)pos.x, 8 - newIndexOrder.Count + i] = details.gameObject;

				if(i > oldIndices.Count)
				{
					details.type = GetRandomType(Random.Range(0, 5));
					details.UpdateColour();
				}
				
				StartCoroutine(MoveBlock(details.gameObject, details, new Vector3((pos.x - 4) * blockSize, 2.24f + (i + 1) * blockSize, 0) + displacement, 
					new Vector3((pos.x - 4) * blockSize, 2.24f - (newIndexOrder.Count - i - 0.5f) * blockSize, 0) + displacement, 0.5f + newIndexOrder.Count * 0.125f));
			}
		}
	}
}
