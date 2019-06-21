using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum BlockTypes { A = 0, B = 1, C = 2, D = 3, E = 4};

public class BlockDetails : NetworkBehaviour
{
	[Tooltip("Coordinates of the block.")]
	public Vector2 coords = Vector2.zero;

	[Tooltip("Block type.")]
    [SyncVar(hook = "OnUpdateColour")]
	public BlockTypes type = BlockTypes.A;

	[Tooltip("Can the block be clicked on and used?")]
	public bool isInteractable = true;

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

        //UpdateColour();

        StartCoroutine(DelayedColourUpdate());
    }

    IEnumerator DelayedColourUpdate()
    {
        yield return new WaitForEndOfFrame();

        UpdateColour();
    }

    void OnUpdateColour(BlockTypes newType)
    {
        type = newType;

        UpdateColour();
    }

	//Change colour to suit type
	public void UpdateColour()
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
}
