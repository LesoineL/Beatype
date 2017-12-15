using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    Manager gMan;
    GameObject playerObject;

	// Use this for initialization
	void Start ()
    {
        gMan = GameObject.Find("GameManager").GetComponent<Manager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
            playerObject = GameObject.FindGameObjectWithTag("Player");
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == playerObject)
        {
            gMan.IncreaseCollectedItems();
            Object.Destroy(this.gameObject);
        }
    }
}
