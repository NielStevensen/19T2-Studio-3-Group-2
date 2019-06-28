using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum BlockTypes { A = 0, B = 1, C = 2, D = 3, E = 4};

//public class BlockDetails : MonoBehaviour
public class BlockDetails : NetworkBehaviour
{
	[Tooltip("Coordinates of the block.")]
	public Vector2 coords = Vector2.zero;

	[Tooltip("Block type.")]
	//[SyncVar(hook = "OnTypeChanged")]
	public BlockTypes type = BlockTypes.A;
	//[HideInInspector]
	[SyncVar(hook = "OnTypeChanged")]
	public int intType = 0;

	[Tooltip("Can the block be clicked on and used?")]
	public bool isInteractable = true;

	//Movement coroutine
	[HideInInspector]
	public Coroutine movementCoroutine;

	//Is the block swapping?
	[HideInInspector]
	public bool isSwapping = false;

	//Sprite renderer reference
	private SpriteRenderer spriteRenderer;

	//Is the block falling? Used to count chains
	[HideInInspector]
	public bool isFalling = false;

	//Index of current chain
	[HideInInspector]
	public int chainIndex = -1;

	//Setup
    void Awake()
    {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		
        StartCoroutine(DelayedTypeUpdate());
    }

	//Delay updating the colour by a frame. Used for initial sync
    IEnumerator DelayedTypeUpdate()
    {
        yield return new WaitForEndOfFrame();

        UpdateType();
    }
	
	//Hook to change then update type
	public void OnTypeChanged(int newType)
	{
		//Debug.Log(coords);

		/*if (isServer)
		{
			Debug.Log("host: " + newType);
		}
		else
		{
			Debug.Log("client: " + newType);
		}*/

		type = (BlockTypes)newType;

		UpdateType();
	}

	//Change colour to suit type
	public void UpdateType()
	{
		switch (type)
		{
			case BlockTypes.A:
				spriteRenderer.color = Color.red;

				break;
			case BlockTypes.B:
				spriteRenderer.color = Color.yellow;

				break;
			case BlockTypes.C:
				spriteRenderer.color = Color.green;

				break;
			case BlockTypes.D:
				spriteRenderer.color = Color.blue;

				break;
			case BlockTypes.E:
				spriteRenderer.color = Color.magenta;

				break;
		}
	}

	//Can the block be matched, i.e. is it moving or already matched?
	public bool IsMatchable()
	{
		return !(isInteractable || isSwapping || isFalling);
	}
}
