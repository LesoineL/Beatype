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
    float spacing;  // spacing of note spawning 

    //Offsets used in placement of beats
    float xOffset;
    float yOffset; 

    Dictionary<int, Vector3> keySpawns;  //Key and spawn location
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
    int[] keys = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
    //live beatmap
    int[] beatmap = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

    //Temporary integer for traversing the beatmap array
    //CHANGE when destroying objects is implemented as this will affect hitCircles
    int nextBeat;

    int beatsOnScreen;

    //defines how many beats it takes circles to move across the entire screen, use this to adjust circle speed
    public int beatsAcrossScreen;
    //List<RectTransform> hitCircles = new List<RectTransform>();
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
        nextBeat = 0; 

        //initialize other variables
        comboCount = 0;
        beatHit = false;
        score = 0;

        xOffset = 9.425f; // used for aligning spawned beat, the higher the more to the left all the notes will be
        yOffset = 6.0f; // how high the beats spawn
        spacing = 2.1f; // space between each beat horizontally 

        //Get the screen dimensions
        screenWidth = mainCamera.pixelWidth;
        screenHeight = mainCamera.pixelHeight;

        keySpawns = new Dictionary<int, Vector3>(); //Create a new hashtable for each possible key and it's spawn location

        for (int i = 0; i < 9; i++)  // note this will have to be in update when we do actual songs. 
        {
            //1 - 9 keys
            Vector3 spawnX = new Vector3(screenWidth, 0.0f, 0.0f);
            spawnX = mainCamera.ScreenToViewportPoint(spawnX);

            keySpawns.Add(i + 1, new Vector3((spawnX.x * spacing * i)  - xOffset, yOffset, 0.0f));

            //Add the 0 key
            if (i == 8)
            {
                spawnBeat(beatmap[i]); // spawn beat 8 then do 0
                i++;              
                keySpawns.Add(0, new Vector3((spawnX.x * spacing * i) - xOffset, yOffset, 0.0f));              
            }

            spawnBeat(beatmap[i]);
        }

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
            spawnTimer += Time.deltaTime;

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
                nextBeat++;
            }

            //Make sure that there is a next beat
            if(nextBeat < hitItems.Count)
            {
                //Check if there is a beat to be hit
                if (checkRange(hitItems[nextBeat]))
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

    bool checkRange(GameObject beat)  //Checks if a beat can be hit
    {
        //temporary range is the bottom 1/4 of the screen to the bottom 1/6
        if(beat.transform.position.y > (screenHeight / 6.0f) && beat.transform.position.y < (screenHeight / 4.0f))
        {
            return true;
        }

        return false;
    }

    bool missedBeat()  //Checks if any beats were missed
    {
        //Loop through each necessary RectTransform in hitCircles
        for(int i = nextBeat; i < hitItems.Count; i++)
        {
            //Check to see if it past the allotted range for being hit
            if(hitItems[i].transform.position.y < (screenHeight / 6.0f))
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
