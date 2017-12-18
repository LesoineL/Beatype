using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public GameObject menu, startPrompt;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            startPrompt.SetActive(true);
            menu.SetActive(false);
        }
    }
    public void Play()
    { SceneManager.LoadScene("GameScene");  }
}
