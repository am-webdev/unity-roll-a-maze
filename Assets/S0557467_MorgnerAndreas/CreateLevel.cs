using UnityEngine;
using System.Collections;

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

        environment.transform.localScale = new Vector3(scaleFac, scaleFac, scaleFac);

        //Scale  + position BasePlate

        Instantiate(floorTiles[1], new Vector3(0, offset, 0), Quaternion.Euler(0,90, 0), root.transform);


        if (root != null)
        {
            //Create the outer walls for given extXZ
            //GameObject outerwall = Instantiate(outerWall, new Vector3(0, 0, 0), Quaternion.Euler(0, 90, 0), root.transform) as GameObject;

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

            //GameObject outerwallE = Instantiate(outerWall, new Vector3(0, 0, 0), Quaternion.identity, root.transform) as GameObject;
            //GameObject outerwallS = Instantiate(outerWall, new Vector3(0, 0, 0), Quaternion.Euler(0, 90, 0), root.transform) as GameObject;
            //GameObject outerwallW = Instantiate(outerWall, new Vector3(0, 0, 0), Quaternion.identity, root.transform) as GameObject;

            //create a maze
            //Build the maze from the given set of prefabs
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
        ball.transform.position = floorTiles[start].transform.position;
    }

    public void EndzoneTrigger(GameObject other)
    {
        //Check if ball first...
        //Player has fallen onto ground plane, reset
    }
    public void winTrigger(GameObject other)
    {
        //Check if ball first...

        //Destroy this maze
        //Generate new maze by reload the whole application - can be improved by regenerate the Maze itself (and increse the dimensions)
        Application.LoadLevel(0);
        //Player has fallen onto ground plane, reset
        placeBallStart();
    }
}
	

