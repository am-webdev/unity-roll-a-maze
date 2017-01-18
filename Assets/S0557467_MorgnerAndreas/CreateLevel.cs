using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CreateLevel : MonoBehaviour
{
    //The tiles have been modeled as 4x4 unity unit squares
    private const float tileSize = 4;

    private GameObject root, floor, environment, ball;
    public int xHalfExt = 1;
    public int zHalfExt = 1;

    public GameObject outerWall;

    public GameObject innerWall;
    public GameObject exitTile;
    public GameObject[] floorTiles;

    private int xExt, zExt;     // represents the dimensions of the grounding floor of the maze
    private int start, end;     // should be the index of an entry from a List<GameObject> that refers to the starting/ending point of the maze
    private float offset;
    private float scaleFac;
    private List<GameObject> floormazetiles = new List<GameObject>();

    // Use this for initialization
    void Awake()
    {
        //Gather together all refrences you will need later on
		floor 			= GameObject.Find ("DSBasementFloor");
		environment 	= GameObject.Find ("Environment");
		ball 			= GameObject.Find ("DSPlayerBall");
        root 			= GameObject.Find ("MovablePlayfield");


        //Build the values for xExt and zExt from xHalfExt and zHalfExt
        xExt = (2 * xHalfExt) + 1;
		zExt = (2 * zHalfExt) + 1;

        //Build an offset for the dyn playfield from the BasePlatform e.g. the bigger halfExtent value in unity units
        if (xHalfExt > zHalfExt)
        {
            offset = xExt;
        } else
        {
            offset = zExt;
        }

        root.transform.position += new Vector3(0, offset, 0);

        //Calculate a scale factor for scaling the non-movable environment (and therefore the camera) and the BasePlatform 
        // the factors that the environment are scaled for right now are for x/zHalfExt =1, scale accordingly
        //i.e. the playfield/environment should be as big as the dynamic field

        //Scale Environment
        scaleFac = 0.8f*(xExt)/tileSize;
        floor.transform.localScale = new Vector3(xExt * zExt - tileSize / 2, 1, xExt * zExt - tileSize / 2);

        environment.transform.localScale = new Vector3(scaleFac, scaleFac, scaleFac);

        if (root != null)
        {
            //Create the outer walls for given extXZ
            for (int i = 0; i < xExt; i++)
            {
                GameObject wallsnippetN = Instantiate(outerWall, new Vector3(-(tileSize/2) + (-xHalfExt * tileSize), offset, (float)i * tileSize - (xHalfExt * tileSize)), Quaternion.Euler(0, 90, 0), root.transform) as GameObject;
                GameObject wallsnippetS = Instantiate(outerWall, new Vector3((tileSize/2) + (xHalfExt * tileSize), offset, (float)i * tileSize - (xHalfExt * tileSize)), Quaternion.Euler(0, 90, 0), root.transform) as GameObject;

            }
            for (int i = 0; i < zExt; i++)
            {
                GameObject wallsnippetE = Instantiate(outerWall, new Vector3((float)i * tileSize - (xHalfExt * tileSize), offset, (tileSize / 2) + (xHalfExt * tileSize)), Quaternion.identity, root.transform) as GameObject;
                GameObject wallsnippetW = Instantiate(outerWall, new Vector3((float)i * tileSize - (xHalfExt * tileSize), offset, (-tileSize / 2) + (-xHalfExt * tileSize)), Quaternion.identity, root.transform) as GameObject;

            }

            //create a maze
            //Build the maze floorfrom the given set of prefabs

            int startX, startZ, endX, endZ;

            startX = UnityEngine.Random.Range(0, xExt);
            startZ = UnityEngine.Random.Range(0, zExt);
            start = startX * startZ;

            endX = UnityEngine.Random.Range(0, xExt);
            endZ = UnityEngine.Random.Range(0, zExt);
            end = endX * endZ;

            while (end == start)
            {
                Debug.Log("prevent end == start...");
                endX = UnityEngine.Random.Range(0, xExt);
                endZ = UnityEngine.Random.Range(0, zExt);
                end = endX * endZ;
            }
            Debug.Log("Start: "+ startX + ":"+startZ);
            Debug.Log("End: " + endX + ":" + endZ);


            for (int i = 0; i < xExt; i++)
            {
                for (int j = 0; j < zExt; j++)
                {
                    GameObject mazefloor;
                    if (i == endX && j == endZ)
                    {
                        // Add exitTile to floor
                        mazefloor = Instantiate(exitTile, 
                            new Vector3(i * tileSize - xExt / 2 * tileSize, offset, j * tileSize - zExt / 2 * tileSize), Quaternion.identity, root.transform) as GameObject;
                        mazefloor.tag = "Finish";
                    }
                    else if (i == startX && j == startZ)    // prevent holes on start tile
                    {
                        Debug.Log("Hole on start..");
                        mazefloor = Instantiate(floorTiles[0],
                            new Vector3(i * tileSize - xExt / 2 * tileSize, offset, j * tileSize - zExt / 2 * tileSize), Quaternion.identity, root.transform) as GameObject;
                    }
                    else
                    {
                        mazefloor = Instantiate(floorTiles[UnityEngine.Random.Range(0, floorTiles.Length)],
                            new Vector3(i * tileSize - xExt / 2 * tileSize, offset, j * tileSize - zExt / 2 * tileSize), Quaternion.identity, root.transform) as GameObject;                        
                    }
                    mazefloor.name = "Floor (" + i + "," + j + ")";
                    floormazetiles.Add(mazefloor);
                }
            }

            //Set the walls for the maze (place only one wall between two cells, not two!)

            //Place the PlayerBall above the playfiel
            placeBallStart();
        }
    }

    //You might need this more than once...
    void placeBallStart()
    {
        //Reset Physics
        if (ball != null) ball.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //Place the ball
        ball.transform.position = floormazetiles[start].transform.position;
        ball.transform.position += new Vector3(0, 4, 0);
    }

    public void EndzoneTrigger(GameObject other)
    {
        //Check if ball first...
        //Player has fallen onto ground plane, reset
        if (other.tag == "Respawn")
        {
            placeBallStart();
        }
    }
    public void winTrigger(GameObject other)
    {
        //Check if ball first...
        if (other.tag == "Finish")
        {

            //Destroy this maze
            //Generate new maze by reload the whole application - can be improved by regenerate the Maze itself (and increse the dimensions)
            Application.LoadLevel(0);
            //Player has fallen onto ground plane, reset
            placeBallStart();

        }
    }
}
	

