using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    
	void OnMouseDown ()
    {
        GameObject.Find("SceneManager").GetComponent<MenuScript>().currentMenu = MenuScript.MenuState.options;
	}
}
