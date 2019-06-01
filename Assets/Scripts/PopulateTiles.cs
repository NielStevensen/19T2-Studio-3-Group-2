using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateTiles : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public GameObject tile;
    Vector3 placementPos;
    public enum TileStates {magenta = 0,red = 1,cyan =2,green = 3,black = 4,blank = 5 }
    public TileStates[,] tileArray;
    public GameObject[,] savedTiles;
    TileStates Temp; // useed to hold data while transfered
    float lastMouseX;
    public Camera cam;
    public int[] selected;

    // Start is called before the first frame update
    void Start()
    {
        selected = new int[2];
        tileArray = new TileStates[gridX, gridY];
        savedTiles  = new GameObject[gridX, gridY];
        for (float x = 0; x <gridX; x++)
        {
            for (float y = 0; y < gridY; y++)
            {
                // nightmare equation for equal distribution on 8*8 grid
                Renderer Renderer = GetComponent<Renderer>(); // makes next line shorter and easdier to understand
                placementPos = new Vector3(transform.position.x - (Renderer.bounds.size.x/2) +(Renderer.bounds.size.x *(((2 * x) + 1) / (gridX *2))), transform.position.y - (Renderer.bounds.size.y / 2) + (Renderer.bounds.size.y * (((2 * y) + 1) / (gridY*2))));
                tileArray[Mathf.FloorToInt(x), Mathf.FloorToInt(y)] = (TileStates)Random.Range(0, 6); // spawn with random values and save to array of tile types
                savedTiles[Mathf.FloorToInt(x), Mathf.FloorToInt(y)] = Instantiate(tile, placementPos, Quaternion.identity,transform); // create a ytile and save to arry of tiles as ojects
                // asign colours update to change spties instead once avaliable
                AssignColor(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
            }
        }  
    }

    // Update is called once per frame
    void Update()
    {
        // falling code
        if (Input.GetButtonDown("Jump"))
        {
            for (float x = 0; x < gridX; x++)
            {
                for (float y = 1; y < gridY; y++)
                {
                    if (tileArray[Mathf.FloorToInt(x), Mathf.FloorToInt(y - 1)] == TileStates.blank)
                    {
                        Temp = tileArray[Mathf.FloorToInt(x), Mathf.FloorToInt(y)];
                        tileArray[Mathf.FloorToInt(x), Mathf.FloorToInt(y - 1)] = tileArray[Mathf.FloorToInt(x), Mathf.FloorToInt(y)];
                        tileArray[Mathf.FloorToInt(x), Mathf.FloorToInt(y)] = TileStates.blank;

                    } 
                }
            }
        }
        //movement
        Vector3 selectionPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) // set position on firsat frame button is held
        {
            for (int x = 1; x < gridX; x++)
            {
                for (int y = 0; y < gridY; y++)
                {
                    if (CheckBounds(x, y, selectionPoint))
                    {
                        lastMouseX = Input.mousePosition.x;
                        selected[0] = x;
                        selected[1] = y;
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0)) //check each consescutive frame
        {
            for (int x = 0; x < gridX; x++)
            {
                for (int y = 0; y < gridY; y++)
                {
                    if (CheckBounds(x, y, selectionPoint))
                    {
                        if (x != selected[0])
                        {
                            MouseSwap(x, y);
                        }
                    }
                }
            }
        }

        //redraw grid
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                AssignColor(x, y);
            }
        }
    }

    void AssignColor(int x, int y)
    {
        switch (tileArray[x, y])
        {
            case TileStates.magenta:
                {
                    savedTiles[x, y].GetComponent<SpriteRenderer>().color = Color.magenta;
                    break;
                }
            case TileStates.red:
                {
                    savedTiles[x, y].GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                }
            case TileStates.cyan:
                {
                    savedTiles[x, y].GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                }
            case TileStates.green:
                {
                    savedTiles[x, y].GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                }
            case TileStates.black:
                {
                    savedTiles[x, y].GetComponent<SpriteRenderer>().color = Color.black;
                    break;
                }
            case TileStates.blank:
                {
                    savedTiles[x, y].GetComponent<SpriteRenderer>().color = Color.clear;
                    break;
                }
        }
    } // update colours

    void CursorSwap(int x, int y)
    {
        Temp = tileArray[x,y];
        tileArray[x,y] = tileArray[x,y];
        tileArray[x,y] = Temp;
    }

    void MouseSwap(int x, int y)
    {
        if (Input.mousePosition.x > lastMouseX)
        {
            Temp = tileArray[x, y];
            tileArray[x, y] = tileArray[x - 1, y];
            tileArray[x - 1, y] = Temp;

        }
        if (Input.mousePosition.x < lastMouseX)
        {
            Temp = tileArray[x, y];
            tileArray[x, y] = tileArray[x + 1, y];
            tileArray[x + 1, y] = Temp;

        }
        lastMouseX = Input.mousePosition.x;
        selected[0] = x;

    }

    private bool CheckBounds(int x, int y, Vector3 selectionPoint)
    {
        bool flag = false;
        if (selectionPoint.x > savedTiles[x, y].transform.position.x - savedTiles[x, y].GetComponent<SpriteRenderer>().bounds.extents.x)
        {
            if (selectionPoint.x < savedTiles[x, y].transform.position.x + savedTiles[x, y].GetComponent<SpriteRenderer>().bounds.extents.x)
            {
                if (selectionPoint.y < savedTiles[x, y].transform.position.y + savedTiles[x, y].GetComponent<SpriteRenderer>().bounds.extents.y)
                {
                    if (selectionPoint.y > savedTiles[x, y].transform.position.y - savedTiles[x, y].GetComponent<SpriteRenderer>().bounds.extents.y)
                    {
                        flag = true;
                    }
                }
            }
        }
        return flag;
    }
}
