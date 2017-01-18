using UnityEngine;
using System.Collections;

public class YouLooseTest : MonoBehaviour
{

    CreateLevel gameCtrl;

    // Use this for initialization
    void Start()
    {
        gameCtrl = (CreateLevel)FindObjectOfType(typeof(CreateLevel));
    }
   
    void OnTriggerEnter(Collider other)
    {
        //call a method of gameCtrl
        Debug.Log(name + tag);
        gameCtrl.EndzoneTrigger(gameObject);
    }
}
