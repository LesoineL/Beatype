using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameOver : MonoBehaviour
{
    //Object for pause overlay
    public GameObject pauseScreen;
    //Object for end overlay
    public GameObject endScreen;

	// Update is called once per frame
	void Update ()
    {
        //When the game is paused, all transformations are stopped and the pause overlay is projected
        if (gameObject.GetComponent<Manager>().currState == Manager.gameState.Paused)
        {
            Time.timeScale = 0.0f;
        }

        //Removes any overlay and resumes all transformations, when user returns to the game
        else if (gameObject.GetComponent<Manager>().currState == Manager.gameState.InGame)
        {
            Time.timeScale = 1.0f;
            Destroy(GameObject.Find("PauseOverlay(Clone)"));
        }

        //When the game ends, overlay is updated
        else if (gameObject.GetComponent<Manager>().currState == Manager.gameState.GameEnd)
        {
            Time.timeScale = 0.0f;
            gameObject.GetComponent<Manager>().scoreText.rectTransform.position = new Vector3(0, 1.0f, -9.0f);
            gameObject.GetComponent<Manager>().comboText.gameObject.SetActive(false);
        }
	}
}
