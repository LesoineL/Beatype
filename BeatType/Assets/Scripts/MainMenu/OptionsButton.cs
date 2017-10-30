using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    
	void OnMouseDown ()
    {
        Destroy(GameObject.Find("MenuButtons(Clone)"));
        GameObject.Find("SceneManager").GetComponent<MenuScript>().currentMenu = MenuScript.MenuState.options;
	}
}
