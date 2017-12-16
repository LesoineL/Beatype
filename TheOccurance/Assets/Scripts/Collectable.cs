using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    Manager gMan;
    Vector3 foward;
    RaycastHit hit;

	// Use this for initialization
	void Start ()
    {
        gMan = GameObject.Find("GameManager").GetComponent<Manager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        foward = transform.TransformDirection(Vector3.forward);

        if(Physics.Raycast(transform.position, foward,out hit, 1.5f))
        {
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Collectable")
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        gMan.IncreaseCollectedItems(); 
                        GameObject.Destroy(hit.collider.gameObject); 
                    }
                }
            }
        }
	}


}
