using System.Collections.Generic;
using UnityEngine;
using System;

// https://www.youtube.com/watch?v=yOgIncKp0BE&list=PLFt_AvWsXl0eZgMK_DT5_biRkWXftAOf9&index=2

public class CaveGeneration : MonoBehaviour
{
    public int width;
    public int height;
    public int passes;

    public string seed; // store a  seed 
    public bool useRandomSeed;

    public GameObject dirtBlock;

    private GameObject container;

    [Range(0, 100)]//percentage 
    public int randomFillPercent; // how much is going to be filled with wall compared to space 

    int[,] map; //container 
    private List<Vector3> spawnPositions = new();

    void Start()
    {
        container = new GameObject();
        GenerateMap(); // calling the function as soon as this script is loaded
        DrawElements();
        int index = UnityEngine.Random.Range(0, spawnPositions.Count);
        GameManager.Instance.SpawnPlayer(spawnPositions[index]);
    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))// when mouse is pressed call function
    //    {
    //        GenerateMap();
    //    }
    //}

    void GenerateMap()
    {
        Debug.Log("This is starting generation");
        map = new int[width, height]; // seting the parameters for how big we want the map
        RandomFillMap();

        for (int i = 0; i < passes; i++) //set number of passes 
        {
            SmoothMap();
        }
    }
   
    void RandomFillMap()
    {
        Debug.Log("This is generating the map");
        if (useRandomSeed || string.IsNullOrWhiteSpace(seed))// if it is null create a random seed 
        {
            Debug.Log("Generate Randomly");
            seed = DateTime.Now.ToString("YYYY-MM-DD HH:mm:ss.ffffff"); //determines the calendar time at the currrent time in seconds
        }

        System.Random prng = new System.Random(seed.GetHashCode()); //converting string into an integer 

        for (int x = 0; x < width; x++)  //setting up size of map width 
        {
            for (int y = 0; y < height; y++) //setting up size of map height
            {
                if (x==0||x==width-1 || y==0 ||y==height-1) 
                {
                    map[x, y] = 1;// If pixel is near the borders of our map then they are automatticaly set to black (wall)
                }
                else // else continue generating as normal 
                {
                    // map[x, y] = (prng.Next(0, 100) < randomFillPercent) ? 1 : 0;// the random fill percentage if the number is greate then it it is a wall if it is not it is open space 
                    int randomNum = prng.Next(0, 100);
                    if (randomNum< randomFillPercent)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = 0;
                    }
                }               
            }   
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)  //check through each tile 
        {
            for (int y = 0; y < height; y++) //check through each tile 
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);//shows us the amount of wall tiles around the given tile 

                if (neighbourWallTiles > 4) { 
                
                    map[x, y] = 1; // if there are more than 4 walls surrounding then the tile that we is at the center becomes a wall
                }
                else if (neighbourWallTiles <4)
                {
                    map[x, y] = 0;// if there are less than 4 walls surrounding then the tile that is at the center becomes open space 
                }
                
            }
        }
    }

    int GetSurroundingWallCount(int gridX,int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1;neighbourX <= gridX +1;neighbourX++)// iterating through a 3x3 grid centered on grid [x.y]
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) //looking at all neighbours 
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)//Check that x and y are within the map and not -1 for example 
                {
                    if (neighbourX != gridX || neighbourY != gridY)//when neighbour y != gridy and neighbourx != grid x then we will be look at neighboring tiles
                    {
                        wallCount += map[neighbourX, neighbourY]; //wall count up by one when map[neighbourX, neighbourY] = wall
                    }
                }
                else
                {
                    wallCount++;
                }
            }    
        }
        return wallCount;
    }

    void DrawElements()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);

                    if (map[x, y] == 1)
                    {
                        var go = Instantiate(dirtBlock, position, Quaternion.identity, container.transform);
                        go.name = $"Block: {x},{y}";
                    }
                    else {
                        // If its in range of out block
                        // && and if the line below block is a solid block
                        if (y - 1 > 0 && map[x, y - 1] == 1) 
                        {
                            spawnPositions.Add(position);    
                        }
                    }
                }
            }           
        }
    }
    
    //void OnDrawGizmos()
    //{
    //    if (map != null)
    //    {
    //        for (int x = 0; x < width; x++)  // going through each pixel and drawing a cube 
    //        {
    //            for (int y = 0; y < height; y++) 
    //            {
    //                Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white; // if the cube given coordinate is equal to 1 then we set it as a wall set as black to indicate wall else set as white for open spaxce
    //                Vector3 pos = new Vector3(-width / 2 + x + .5F, -height / 2 + y + .5f, 0);//where is the cube
    //                Gizmos.DrawCube(pos, Vector3.one);// put cube there 
    //            }
    //        }
    //    }
    //}
}
