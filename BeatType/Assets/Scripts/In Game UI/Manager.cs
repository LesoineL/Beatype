using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

    //-----Variables------
    int comboCount;
    float timeFromLastNote;
    float spawnTimer;
    bool beatHit;
    public float bpm;  //Beats per minute
    float beatSpeed;  //Speed the notes travel along the screen
    //Offsets used in placement of beats
    float xOffset;
    float yOffset;
    Vector2 spawnLocation;  //Spawn location of the notes
    int score;  //Current score (seperate from the combo)
    //screen dimensions
    float screenWidth;
    float screenHeight;

    //AudioClip audio1;
    //public AudioSource playAudio;

    //Audio manager
    AudioManager aManager;

    public Canvas canvas;
    public Camera mainCamera;

    //UI items
    public GameObject CirclePrefab;
    public RectTransform UICircle;

    //alphabet for looping through input
    char[] keys = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
    //live beatmap
    char[] beatmap = { '1', '1', '4', '9', '-', '-', '7', '-', '8', '6' };

    //Temporary integer for traversing the beatmap array
    //CHANGE when destroying objects is implemented as this will affect hitCircles
    int nextBeat;

    //defines how many beats it takes circles to move across the entire screen, use this to adjust circle speed
    public int beatsAcrossScreen;
    List<RectTransform> hitCircles = new List<RectTransform>();

    //timer for green on beat hit
    float hitTimer;

    //Enum for the game state
    enum gameState
    {
        Paused,
        InGame,
        GameEnd
    }

    gameState currState;

    // Use this for initialization
    void Start ()
    {
		//Check public values
        if(bpm <= 0)  //If bpm is given a garbage value, set to 120
        {
            bpm = 120;
        }

        //Get a reference to the audio manager
        aManager = GetComponent<AudioManager>();

        //initialize other variables
        comboCount = 0;
        beatHit = false;
        score = 0;
        //Offsets used in placement of beats
        yOffset = 5.0f;  //Dummy number for now
        xOffset = 5.0f;  //Dummy number for now

        if (beatmap.Length > 0)  //Make sure there are notes
        {
            nextBeat = beatmap[0];
        }

        //Get the screen dimensions
        screenWidth = mainCamera.pixelWidth;
        screenHeight = mainCamera.pixelHeight;

        //Set the beatSpeed
        beatSpeed = (screenHeight + yOffset) / (60.0f / bpm);

        //Set the initial state to InGame
        currState = gameState.InGame;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //-----InGame-----
        if(currState == gameState.InGame)
        {
            //Check if the user is pausing the game
            if (Input.GetKeyUp(KeyCode.Space)) //Space for now due to easy reachability
            {
                currState = gameState.Paused;
                Debug.Log("PAUSED");
                return;
            }

            //Increment the spawn timer
            spawnTimer += Time.deltaTime;

            //Check to see if another beat needs to be spawned
            if(spawnTimer >= 60.0f / bpm)
            {
                spawnTimer -= 60.0f / bpm;

                //Make sure the next beat isn't empty
                if(nextBeat != '-')
                {
                    spawnBeat(beatmap[nextBeat]);
                }
            }

            //Move circles
            foreach (RectTransform t in hitCircles)
            {
                //t.anchoredPosition -= new Vector2(Time.deltaTime * speed, 0f);
                t.anchoredPosition -= new Vector2(0.0f, Time.deltaTime);
            }

            //Check if a beat was missed
            if (missedBeat())
            {
                beatHit = false;

                //Update the combo
                updateCombo();

                //change the item to check
                nextBeat++;
            }

            if(nextBeat < hitCircles.Count)
            {
                //Check if there is a beat to be hit
                if (checkRange(hitCircles[nextBeat]))
                {
                    //Check if a key is pressed
                    if (Input.anyKeyDown)
                    {
                        //Check if the key pressed matches
                        if (Input.GetKeyDown("Alpha" + beatmap[nextBeat]))
                        {
                            beatHit = true;
                            updateCombo();
                        }
                    }
                }
            }
        }
        //-----Pause Menu-----
        else if(currState == gameState.Paused)
        {
            //Check if the user is unpausing the game
            if (Input.GetKeyUp(KeyCode.Space)) //Space for now due to easy reachability
            {
                currState = gameState.InGame;
                Debug.Log("UNPAUSED");
                return;
            }
        }
        //-----Song Ended-----
        else if(currState == gameState.GameEnd)
        {
            //TODO--song end screen & return to main?
        }
	}

    //-----Helper Methods-----
    void updateCombo()  //Updates the current combo
    {
        //Check if the beat was hit
        if(beatHit)
        {
            comboCount++;
            //Check if the combo is at least 2
            if(comboCount > 1)
            {
                //Consider increasing the score with combo
                //score += 2;

                //Give some feedback
                Debug.Log("Combo + " + comboCount + "!");
            }
        }
        else
        {
            //Reset the combo and give feedback
            comboCount = 0;
            Debug.Log("Combo lost!");
        }
    }

    bool checkRange(RectTransform beat)  //Checks if a beat can be hit
    {
        //Check if anything is in range
        //temporary range is the bottom 1/4 of the screen to the bottom 1/6
        if(beat.position.y > (screenHeight / 6.0f) && beat.position.y < (screenHeight / 4.0f))
        {
            return true;
        }

        return false;
    }

    bool missedBeat()  //Checks if any beats were missed
    {
        //Loop through each necessary RectTransform in hitCircles
        for(int i = nextBeat; i < hitCircles.Count; i++)
        {
            //Check to see if it past the allotted range for being hit
            if(hitCircles[i].position.y < (screenHeight / 6.0f))
            {
                return true;
            }
        }

        return false;
    }

    void spawnBeat(char number)  //Spawns a beat labeled with the specified number
    {
        //Make sure it is a valid value
        if(number != '-')
        {
            //Distance from x=0 to spawn the object
            int distance = 0;

            if(number != '0')
            {
                distance = number;
            }
            else
            {
                distance = 10;
            } 

            //Instantiate a new object
            GameObject newTarget = GameObject.Instantiate(CirclePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            newTarget.transform.SetParent(canvas.transform, false);
            //Move the object to a starting position based upon it's number
            newTarget.transform.position = new Vector3((screenWidth / 10.0f) * distance - xOffset, screenHeight + yOffset, 0.0f);  //Final values changable
            newTarget.transform.GetChild(0).GetComponent<Text>().text = number.ToString().ToUpper();
        }
    }
}
