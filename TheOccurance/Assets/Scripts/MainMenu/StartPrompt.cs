using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPrompt : MonoBehaviour
{
    public GameObject menu;

    public float oscillateSpeed = 1.0f;
    float timer = 0.0f;

    // Update is called once per frame
    void Update ()
    {
		if (Input.anyKeyDown)
        {
            menu.SetActive(true);
            gameObject.SetActive(false);
        }

        gameObject.GetComponent<Text>().color = new Color(gameObject.GetComponent<Text>().color.r, gameObject.GetComponent<Text>().color.g, gameObject.GetComponent<Text>().color.b, Mathf.Cos(timer));
        timer += Time.deltaTime * oscillateSpeed;
    }
}
