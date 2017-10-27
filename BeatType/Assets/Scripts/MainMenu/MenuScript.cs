using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    //Toggleable states between a "Press Start" prompt, the main menu, the options menu, and the credits
    public enum MenuState { startPrompt, menu, options, credits, quit   };
    public MenuState currentMenu;

    //Sprite object for the start prompt
    public GameObject startPrompt;
    //Sprite object for the menu buttons
    public GameObject menuButtons;
    //Sprite object for the quit prompt
    public GameObject quitPrompt;

	// Use this for initialization
	void Start ()
    {
        //Scene initializes with the start prompt
        currentMenu = MenuState.startPrompt;
        Instantiate(startPrompt);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Replaces the start prompt with the menu whenever any input is received
        if (currentMenu == MenuState.startPrompt && Input.anyKeyDown)
        {
            currentMenu = MenuState.menu;
            Instantiate(menuButtons);
            Destroy(GameObject.Find("StartPrompt(Clone)"));
        }

        //Returns the menu to the start prompt whenever the escape key is hit
        if (currentMenu == MenuState.menu && Input.GetKeyDown(KeyCode.Escape))
        {
            currentMenu = MenuState.startPrompt;
            Instantiate(startPrompt);
            Destroy(GameObject.Find("MenuButtons(Clone)"));
        }

        //Returns back to the main menu when the escape key is hit in any other menu
        if ((currentMenu == MenuState.options && Input.GetKeyDown(KeyCode.Escape)) ||
            (currentMenu == MenuState.credits && Input.GetKeyDown(KeyCode.Escape)))
        {
            currentMenu = MenuState.menu;
            Instantiate(menuButtons);
        }

        //Prompts the user with quit options when they attempt to quit
        if (currentMenu == MenuState.quit)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                Application.Quit();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(GameObject.Find("QuitPrompt(Clone)"));
                currentMenu = MenuState.menu;
            }
        }
	}
}
