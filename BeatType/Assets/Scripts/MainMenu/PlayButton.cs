using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
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
        SceneManager.LoadScene("LevelSelect");
    }
}
