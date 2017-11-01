using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

    //-----Variables------
    int comboCount;
    const float hitTime = .15f;
    float timeOffset;
    float offSetTimer; 
    bool beatHit;
    float globalTimer; 

    //World coordinates of the range for hitting a beat
    Vector3 topLeftRange;
    Vector3 botRightRange;
    float songTimer; // an easier way of keeping track of the current song time
    float noteFallSpeed; 
    Dictionary<int, Vector3> keySpawns;  //Key and spawn location
    float score;  //Current score (seperate from the combo)
    float percentPerHit; // to calculate a more easy to recognize score, based on % hit

    //Audio manager
    AudioManager aManager;

    public Canvas canvas;
    public Camera mainCamera;

    //UI items
    public GameObject CirclePrefab;
    public RectTransform UICircle;
    public Text comboText;
    public Text scoreText;
    public GameObject restartButton;

    //live beatmap
    List<int> beatmap;
    List<float> beatmapTimes;  


    //Temporary integer for traversing the beatmap array
    //CHANGE when destroying objects is implemented as this will affect hitCircles
    int nextBeat;

    //TEMP - change if better way of doing traversing found
    int currentBeat;

    //defines how many beats it takes circles to move across the entire screen, use this to adjust circle speed
    public int beatsAcrossScreen;
    List<GameObject> hitItems = new List<GameObject>();

    Reflection song;

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
        song = GetComponent<Reflection>(); 
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

        keySpawns = new Dictionary<int, Vector3>(); //Create a new dictionary for each possible key and it's spawn location

        beatmap = song.LoadandSetNoteMap();
        beatmapTimes = song.LoadandSetNoteTimes(); //Initialize the list for containing the times

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
                spawnPoint = mainCamera.ViewportToWorldPoint(new Vector3(0.0925f * (i + 1), 1.1f));      
                keySpawns.Add(0, spawnPoint);
            }
        }


        aManager.loadAudioFiles("Sounds/Songs");
        aManager.setSongToPlay(0);
        aManager.playSong();
        noteFallSpeed = 4.0f;
        timeOffset = noteFallSpeed * 5.7f;
        percentPerHit = 100.0f / beatmap.Count;

        //Set the initial state to InGame
        currState = gameState.InGame;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //-----InGame-----
        if(currState == gameState.InGame)
        {
            songTimer = aManager.getCurrentTime();
            globalTimer += Time.deltaTime;
            offSetTimer = globalTimer + timeOffset; 

         //  Debug.Log("Beat time: " + beatmapTimes[currentBeat] + " Song Timer: " + songTimer);
            if (nextBeat < beatmap.Count)
            {
                float timeToSpawn = beatmapTimes[nextBeat] + timeOffset;
                 if (offSetTimer >= timeToSpawn)
                {
                    spawnBeat(beatmap[nextBeat]);
                    nextBeat++;
                }
            }

            //Move circles
            foreach (GameObject t in hitItems)
            {
                t.transform.position = new Vector3(t.transform.position.x, t.transform.position.y - Time.deltaTime * noteFallSpeed, 0.0f);
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
            if (currentBeat < hitItems.Count)
            {
                if (beatmapTimes[currentBeat] - songTimer <= .5)
                {
                  //  Debug.Log("song timer " + songTimer + " beat time " + beatmapTimes[currentBeat]); 
                    //Check if there is a beat to be hit
                    if (Input.GetKeyDown("" + beatmap[currentBeat]) || Input.GetKeyDown("[" + beatmap[currentBeat] + "]"))
                    {
                        if (beatmapTimes[currentBeat] <= songTimer + hitTime && beatmapTimes[currentBeat] >= songTimer - hitTime)
                        {
                            beatHit = true;
                            currentBeat++;
                            updateCombo();
                        }
                        else
                        {
                            beatHit = false;
                            updateCombo();
                            currentBeat++;
                        }
                    }
                }
               
            }

            //If no more beats
            if(currentBeat >= beatmap.Count)
            {
                currState = gameState.GameEnd;
                restartButton.SetActive(true);
            }

            //pause
            if (Input.GetKeyUp(KeyCode.Space))
            {
                currState = gameState.Paused;
                aManager.isPaused(true);
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
                aManager.isPaused(false); 
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
            score += percentPerHit;
            
            //Check if the combo is at least 2
            if(comboCount > 1)
            {
                //Consider increasing the score with combo
              //  score += comboCount;

            }
            //Update the score text
            scoreText.text = score.ToString("f2") + "%";

            //Debug.Log("Combo + " + comboCount + "!");
        }
        else
        {
            //Reset the combo and give feedback
            comboCount = 0;
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
