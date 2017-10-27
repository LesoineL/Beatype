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
    float globalTimer;  //float to hold the total time
    bool beatHit;
    float spacing;  // spacing of note spawning 
    //World coordinates of the range for hitting a beat
    Vector3 topLeftRange;
    Vector3 botRightRange;

    //Offsets used in placement of beats
    float xOffset;
    float yOffset; 

    Dictionary<int, Vector3> keySpawns;  //Key and spawn location
    int score;  //Current score (seperate from the combo)

    //AudioClip audio1;
    //public AudioSource playAudio;

    //Audio manager
    AudioManager aManager;

    public Canvas canvas;
    public Camera mainCamera;

    //UI items
    public GameObject CirclePrefab;
    public RectTransform UICircle;
    public Text comboText;
    public Text scoreText;

    //alphabet for looping through input
    int[] keys = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
    //live beatmap
    int[] beatmap = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 7, 4, 2, 8, 1, 0, 0, 0, 6, 4, 1, 9, 3, 4 };
    List<float> beatmapTimes;  //TEMP - time list to create a time distance between notes

    //Temporary integer for traversing the beatmap array
    //CHANGE when destroying objects is implemented as this will affect hitCircles
    int nextBeat;

    //TEMP - change if better way of doing traversing found
    int currentBeat;

    int beatsOnScreen;

    //defines how many beats it takes circles to move across the entire screen, use this to adjust circle speed
    public int beatsAcrossScreen;
    List<GameObject> hitItems = new List<GameObject>();

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
        aManager = GetComponent<AudioManager>(); //Get a reference to the audio manager
        currentBeat = 0;  //The current beat of focus (the one that is the closest to being hittable)
        nextBeat = 0;   //The next beat for spawning

        //initialize other variables
        comboCount = 0;
        beatHit = false;
        score = 0;
        //World coordinates of the range for hitting a beat
        topLeftRange = mainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.3f));
        botRightRange = mainCamera.ViewportToWorldPoint(new Vector3(1.0f, 0.1f));
        //-----IGNORE-----

        xOffset = 9.425f; // used for aligning spawned beat, the higher the more to the left all the notes will be
        yOffset = 6.0f; // how high the beats spawn
        spacing = 2.1f; // space between each beat horizontally 

        //----------------

        keySpawns = new Dictionary<int, Vector3>(); //Create a new dictionary for each possible key and it's spawn location

        beatmapTimes = new List<float>();  //Initialize the list for containing the times

        //For loop to set up key spawn locations
        for (int i = 0; i < 9; i++)
        {
            //1 - 9 keys
            //ViewPointToWorld of the spawning location
            Vector3 spawnPoint = mainCamera.ViewportToWorldPoint(new Vector3(0.091f * (i + 1), 1.1f));
            keySpawns.Add(i + 1, spawnPoint);
            //Add the 0 key
            if (i == 8)
            {
                //increment i for 0
                i++;
                //Change the spawn location to adjust for the new i value
                spawnPoint = mainCamera.ViewportToWorldPoint(new Vector3(0.091f * (i + 1), 1.1f));      
                keySpawns.Add(0, spawnPoint);
            }
        }

        //-----Set up Beatmap times-----
        //TEMP
        for(int i = 0; i < beatmap.Length; i++)
        {
            if(i == 0)
            {
                beatmapTimes.Add(Random.Range(0.5f, 3.0f));
            }
            else
            {
                beatmapTimes.Add(beatmapTimes[i-1] + Random.Range(0.5f, 3.0f));
            }
        }

        //-----End Beatmap time setup

        //Set the initial state to InGame
        currState = gameState.InGame;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //-----InGame-----
        if(currState == gameState.InGame)
        {
            //Increment the spawn timer
            //spawnTimer += Time.deltaTime;

            //Increment the global timer
            globalTimer += Time.deltaTime;

            if(nextBeat < beatmap.Length)
            {
                if (globalTimer >= beatmapTimes[nextBeat])
                {
                    spawnBeat(beatmap[nextBeat]);
                    nextBeat++;
                }
            }

            //Move circles
            foreach (GameObject t in hitItems)
            {
                t.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - Time.deltaTime * 2.0f, 0.0f);
            }

            //Check if a beat was missed
            if (missedBeat())
            {
                beatHit = false;

                //Update the combo
                updateCombo();

                //change the item to check
                currentBeat++;
            }

            //Make sure that there is a next beat
            if(currentBeat < hitItems.Count)
            {
                //Check if there is a beat to be hit
                if (checkRange(hitItems[currentBeat]))
                {
                    //Check if a key is pressed
                    if (Input.anyKeyDown)
                    {
                        //Check if the key pressed matches
                        if (Input.GetKeyDown("" + beatmap[currentBeat]))
                        {
                            beatHit = true;
                            currentBeat++;
                            updateCombo();
                        }
                    }
                }
            }

            //If no more beats
            //else
            //{
            //    currState = gameState.GameEnd;
            //}

            //pause
            if (Input.GetKeyUp(KeyCode.Space))
            {
                currState = gameState.Paused;
                return;
            }

        } // in game end bracket 

        //-----Pause Menu-----
        else if(currState == gameState.Paused)
        {
            //Check if the user is unpausing the game
            if (Input.GetKeyUp(KeyCode.Space)) //Space for now due to easy reachability
            {
                currState = gameState.InGame;
                return;
            }
        }

        //-----Song Ended-----
        else if(currState == gameState.GameEnd)
        {
            //TODO--song end screen & return to main?
        }  

    } // Update end bracket 

    //-----Helper Methods-----
    void updateCombo()  //Updates the current combo
    {
        //Check if the beat was hit
        if(beatHit)
        {
            comboCount++;
            score++;
            //Check if the combo is at least 2
            if(comboCount > 1)
            {
                //Consider increasing the score with combo
                score += comboCount;

                //Give some feedback
                Debug.Log("Combo + " + comboCount + "!");
            }
            //Update the score text
            scoreText.text = "Score: " + score;

            //Debug.Log("Combo + " + comboCount + "!");
        }
        else
        {
            //Reset the combo and give feedback
            comboCount = 0;
            //Debug.Log("Combo lost!");
        }
        //Update the combo text
        comboText.text = "Combo: " + comboCount;
    }

    bool checkRange(GameObject beat)  //Checks if a beat can be hit
    {
        //temporary range is the bottom 1/4 of the screen to the bottom 1/6
        if (beat.transform.position.y > botRightRange.y && beat.transform.position.y < topLeftRange.y)
        {
            return true;
        }

        return false;
    }

    bool missedBeat()  //Checks if any beats were missed
    {
        //Loop through each necessary RectTransform in hitCircles
        for (int i = currentBeat; i < hitItems.Count; i++)
        {
            //Check to see if it past the allotted range for being hit
            if(hitItems[i].transform.position.y < botRightRange.y)
            {
                return true;
            }
        }

        return false;
    }

    void spawnBeat(int number)  //Spawns a beat labeled with the specified number
    {
        //Make sure it is a valid value
        if(number != -1)
        {
            //Instantiate a new object
            //GameObject newTarget = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere, (Vector3)keySpawns[(int)number], Quaternion.identity) as GameObject;
            GameObject newTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newTarget.transform.position = (Vector3)keySpawns[number];
            hitItems.Add(newTarget);
            Debug.Log("Circle made at: " + (Vector3)keySpawns[(int)number]);
        }
    }
}
