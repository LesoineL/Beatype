using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Hitting the 'F' key toggles the flashlight on and off
        if (Input.GetKeyDown(KeyCode.F))
            gameObject.GetComponent<Light>().enabled = !gameObject.GetComponent<Light>().enabled;
	}
}
