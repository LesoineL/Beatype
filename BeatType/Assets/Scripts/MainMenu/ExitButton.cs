using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    void OnMouseDown()
    {
        Instantiate(GameObject.Find("SceneManager").GetComponent<MenuScript>().quitPrompt);
        GameObject.Find("SceneManager").GetComponent<MenuScript>().currentMenu = MenuScript.MenuState.quit;
    }

}
