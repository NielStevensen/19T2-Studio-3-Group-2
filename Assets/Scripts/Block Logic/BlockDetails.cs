using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum BlockTypes { A = 0, B = 1, C = 2, D = 3, E = 4};

public class BlockDetails : NetworkBehaviour
{
	//Which manager the client should look for. 0: client, 1: opponent's view
	[HideInInspector]
	[SyncVar]
	public int managerSearchIndex = -1;

	[Tooltip("Block ID. Unchanged throughout play session.")]
	[SyncVar]
	public int blockID = -1;

	[Space(10)]

	[Tooltip("Coordinates of the block.")]
	[SyncVar]
	public Vector2 coords = Vector2.zero;
	[Tooltip("Block type.")]
	[SyncVar]
	public BlockTypes type = BlockTypes.A;

	//Movement coroutine
	[HideInInspector]
	public Coroutine movementCoroutine;

	[Space(10)]

	[Tooltip("Can the block be clicked on and used?")]
	public bool isInteractable = true;
	
	[Tooltip("Is the block falling?")]
	public bool isFalling = false;
	
	[Tooltip("The chain this block will contribute to.")]
	public int chainIndex = -1;

	//Sprite renderer reference
	[HideInInspector]
	public SpriteRenderer spriteRenderer;

	//Sprite sheet for current tile set
	[HideInInspector]
	public Sprite[] spriteSheet;

	//Setup
    void Awake()
    {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		
        StartCoroutine(DelayedTypeUpdate());
    }

	//Set references
	private void Start()
	{
		BlockManager master = null;

		if (managerSearchIndex == 0)
		{
			master = Camera.main.GetComponent<ManagerIndentifier>().clientManager;
		}
		else if(managerSearchIndex == 1)
		{
			master = Camera.main.GetComponent<ManagerIndentifier>().projectedManager;
		}

		if(master == null)
		{
			return;
		}

		if(master.allBlocksStatic[blockID] != null)
		{
			return;
		}

		transform.SetParent(master.gameObject.transform);
		
		master.allBlocks[(int)coords.x, (int)coords.y] = gameObject;
		master.allBlocksStatic[blockID] = this;
		spriteSheet = master.spriteSheets[master.spriteSheetIndex].spriteSheet;
	}

	#region Type update
	//Delay updating the colour by a frame. Used for initial sync
	IEnumerator DelayedTypeUpdate()
    {
        yield return new WaitForEndOfFrame();

        UpdateType();
    }
	
	//Change sprite to suit type
	public void UpdateType()
	{
		spriteRenderer.sprite = spriteSheet[(int)type];
	}
	#endregion
}
