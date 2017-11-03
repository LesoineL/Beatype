using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    void OnMouseEnter()
    {
        if (GameObject.Find("SceneManager").GetComponent<MenuScript>().currentMenu != MenuScript.MenuState.quit)
            gameObject.transform.localScale = new Vector3(1.25f, 1.25f);
    }

    void OnMouseExit()
    {
        gameObject.transform.localScale = Vector3.one;
    }

    void OnMouseDown ()
    {
        Destroy(GameObject.Find("MenuButtons(Clone)"));
        Instantiate(GameObject.Find("SceneManager").GetComponent<MenuScript>().options);
        GameObject.Find("SceneManager").GetComponent<MenuScript>().currentMenu = MenuScript.MenuState.options;
	}
}
