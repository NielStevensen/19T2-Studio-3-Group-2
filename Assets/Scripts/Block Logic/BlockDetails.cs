using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockTypes { A = 0, B = 1, C = 2, D = 3, E = 4};

public class BlockDetails : MonoBehaviour
{
	[Tooltip("Coordinates of the block.")]
	public Vector2 coords = Vector2.zero;

	[Tooltip("Block type.")]
	public BlockTypes type = BlockTypes.A;

	[Tooltip("Can the block be clicked on and used?")]
	public bool isInteractable = true;

	//Movement coroutine
	[HideInInspector]
	public Coroutine movementCoroutine;
	
	//Sprite renderer reference
	private SpriteRenderer spriteRenderer;

	//Is the block falling? Used to count chains
	[HideInInspector]
	public bool isFalling = false;

	//Index of current chain
	//[HideInInspector]
	public int chainIndex = -1;

	//current temp
	public Sprite[] spriteSheet;

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
	
	//Change colour to suit type
	public void UpdateType()
	{
		spriteRenderer.sprite = spriteSheet[(int)type];
	}
}
