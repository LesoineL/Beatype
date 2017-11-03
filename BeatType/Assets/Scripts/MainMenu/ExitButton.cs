using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
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

    void OnMouseDown()
    {
        gameObject.transform.localScale = Vector3.one;
        Instantiate(GameObject.Find("SceneManager").GetComponent<MenuScript>().quitPrompt);
        GameObject.Find("SceneManager").GetComponent<MenuScript>().currentMenu = MenuScript.MenuState.quit;
    }

}
