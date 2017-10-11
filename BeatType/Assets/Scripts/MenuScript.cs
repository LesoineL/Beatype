using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    //Toggleable states between a "Press Start" prompt, and the actual menu
    public enum MenuState { startPrompt, menu   };
    public MenuState currentMenu;

    //Sprite object for the start prompt
    public GameObject startPrompt;
    //Sprite object for the menu buttons
    public GameObject menuButtons;

	// Use this for initialization
	void Start ()
    {
        //Scene initializes with the start prompt
        currentMenu = MenuState.startPrompt;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(currentMenu == MenuState.startPrompt)
        {
            Instantiate(startPrompt);
            Destroy(GameObject.Find("Menu Buttons"));
        }

        if(currentMenu == MenuState.menu)
        {
            Instantiate(menuButtons);
            Destroy(GameObject.Find("Start Prompt"));
        }
	}
}
