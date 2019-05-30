using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateTiles : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public GameObject tile;
    Vector3 placementPos;
    public int[,] tileArray;
    public GameObject[,] savedTiles;

    // Start is called before the first frame update
    void Start()
    {
        tileArray = new int[gridX, gridY];
        savedTiles  = new GameObject[gridX, gridY];
        for (float x = 0; x <gridX; x++)
        {
            for (float y = 0; y < gridY; y++)
            {
                // nightmare equation for equal distribution on 8*8 grid
                Renderer Renderer = GetComponent<Renderer>(); // makes next line shorter and easdier to understand
                placementPos = new Vector3(transform.position.x - (Renderer.bounds.size.x/2) +(Renderer.bounds.size.x *(((2 * x) + 1) / (gridX *2))), transform.position.y - (Renderer.bounds.size.y / 2) + (Renderer.bounds.size.y * (((2 * y) + 1) / (gridY*2))));
                tileArray[Mathf.FloorToInt(x), Mathf.FloorToInt(y)] = Random.Range(0, 5); // spawn with random values and save to arrayof tile types
                savedTiles[Mathf.FloorToInt(x), Mathf.FloorToInt(y)] = Instantiate(tile, placementPos, Quaternion.identity,transform); // create a ytile and save to arry of tiles as ojects
                // asign colours update to change spties instead once avaliable
                switch (tileArray[Mathf.FloorToInt(x), Mathf.FloorToInt(y)])
                {
                    case 0:
                        {
                            savedTiles[Mathf.FloorToInt(x), Mathf.FloorToInt(y)].GetComponent<SpriteRenderer>().color = Color.magenta;
                            break;
                        }
                    case 1:
                        {
                            savedTiles[Mathf.FloorToInt(x), Mathf.FloorToInt(y)].GetComponent<SpriteRenderer>().color = Color.red;
                            break;
                        }

                    case 2:
                        {
                            savedTiles[Mathf.FloorToInt(x), Mathf.FloorToInt(y)].GetComponent<SpriteRenderer>().color = Color.cyan;
                            break;
                        }
                    case 3:
                        {
                            savedTiles[Mathf.FloorToInt(x), Mathf.FloorToInt(y)].GetComponent<SpriteRenderer>().color = Color.green;
                            break;
                        }
                    case 4:
                        {
                            savedTiles[Mathf.FloorToInt(x), Mathf.FloorToInt(y)].GetComponent<SpriteRenderer>().color = Color.black;
                            break;
                        }
                }
                }
        }  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
