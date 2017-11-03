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
    //Sprite object for the credits
    public GameObject credits;
    //Sprite object for the options
    public GameObject options;

    //A simple timer for the means of oscillating the opacity of the start prompt
    float timer = 0.0f;

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
        //Causes the start prompt to oscillate its opacity
        if (currentMenu == MenuState.startPrompt)
        {
            timer += 3.5f * Time.deltaTime;
            GameObject.Find("StartPrompt(Clone)").GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, .5f * (1.0f + Mathf.Cos(timer)));
        }

        //Replaces the start prompt with the menu whenever any input is received
        if (currentMenu == MenuState.startPrompt && Input.anyKeyDown)
        {
            timer = 0.0f;
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
            Destroy(GameObject.Find("Credits(Clone)"));
            Destroy(GameObject.Find("Options(Clone)"));
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
