using UnityEngine;
using System.Collections;

public class BaseMovement : MonoBehaviour {
    public GameObject horzPedal = null;
    public GameObject vertPedal = null;
    public float horzRot, vertRot;
    public float pedalFactor = -10;
    public float rotFactor = 30f;

    void Start () {
        //Get References to the game pedals for applying a rotation in Update()
		horzPedal = GameObject.Find("HorzPedal");
		vertPedal = GameObject.Find("VertPedal");

        //initialize all values for use
    }

    // Update is called once per frame
    void Update()
    {
        //Get Input from Input.GetAxis
        //Stretch it from 0-1 to 0 - maxRotFactor

        //Apply a part of the rotation to this (and children) to rotate the plafield
        //Use Quaternion.Slerp und Quaternion.Euler for doing it
        //REM: Maybe you have to invert one axis to get things right visualy

        //Apply an exaggerated ammount of rotation to the pedals to visualize the players input
        //Make the rotation look right

    }
}
