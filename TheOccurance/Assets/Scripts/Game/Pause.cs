using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pause;

	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape) && !pause.activeSelf)
        {
            pause.SetActive(true);
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pause.activeSelf)
        {
            Return();
        }
	}

    public void Return()
    {
        pause.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
