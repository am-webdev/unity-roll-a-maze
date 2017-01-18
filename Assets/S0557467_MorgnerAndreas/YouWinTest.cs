using UnityEngine;
using System.Collections;

public class YouWinTest : MonoBehaviour {
    CreateLevel gameCtrl;

    // Use this for initialization
    void Start()
    {
        //Get a reference
        gameCtrl = (CreateLevel)FindObjectOfType(typeof(CreateLevel));
    }

    void OnTriggerEnter(Collider other)
    {
        //call a method of gameCtrl indicating a possible win situation
        //Is ist good to test for Player here?
        Debug.Log(name + tag);
        gameCtrl.winTrigger(gameObject);
    }
}
