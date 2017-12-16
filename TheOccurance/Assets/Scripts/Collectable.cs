using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Collectable : MonoBehaviour
{
    Manager gMan;
    Vector3 foward;
    RaycastHit hit;

    public Canvas canvas;
    RawImage currImage; 
    public Texture2D collect0;
    public Texture2D collect1;
    public Texture2D collect2;
    public Texture2D collect3;
    public Texture2D collect4;
    public Texture2D collect5;

    // Use this for initialization
    void Start ()
    {
        gMan = GameObject.Find("GameManager").GetComponent<Manager>();
        currImage = canvas.GetComponent<RawImage>();
        currImage.enabled = false; 
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
                        if(hit.collider.name == "collect0")
                        {
                            currImage.texture = collect0;
                            currImage.enabled = true;
                        }
                        else if (hit.collider.name == "collect1")
                        {
                            currImage.texture = collect1;
                            currImage.enabled = true;
                        }
                        else if (hit.collider.name == "collect2")
                        {
                            currImage.texture = collect2;
                            currImage.enabled = true;
                        }
                        else if (hit.collider.name == "collect3")
                        {
                            currImage.texture = collect3;
                            currImage.enabled = true;
                        }
                        else if (hit.collider.name == "collect4")
                        {
                            currImage.texture = collect4;
                            currImage.enabled = true;
                        }
                        else if (hit.collider.name == "collect5")
                        {
                            currImage.texture = collect5;
                            currImage.enabled = true;
                        }
                        gMan.IncreaseCollectedItems();
                        GameObject.Destroy(hit.collider.gameObject); 
                    }
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            currImage.enabled = false; 
        }
	}

}
