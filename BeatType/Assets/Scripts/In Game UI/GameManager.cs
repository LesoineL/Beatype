using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    AudioClip audio1;

    public AudioSource playAudio;

    public Canvas canvas;

    public Camera mainCamera;

    public GameObject CirclePrefab;
    public RectTransform UICircle;

    ComboManager cManager;
    
    //beats per minute for the current map
    public float BPM;
    
    //the beat of the music we're on
    int beat;
    //timer for each individual beat
    float beatTimer;

    //Time range allowed to count a beat hit
    public float timeAcc;

    //alphabet for looping through input
    char[] keys = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    //live beatmap
    char[] beatmap = { 'a', 'b', 'c', '-', 'o', 'n', 'e', '-', 't', 'w', 'o', '-', 't', 'h', 'r', 'e', 'e' };

    //screen dimensions
    float screenWidth;
    float screenHeight;

    //circle speed
    float speed;
    Vector2 spawnLocation;
    //defines how many beats it takes circles to move across the entire screen, use this to adjust circle speed
    public int beatsAcrossScreen;
    List<RectTransform> hitCircles = new List<RectTransform>();

    //is the current beat hit yet?
    bool hitCurBeat = false;
    //timer for green on beat hit
    float hitTimer;

    //current score
    int score = 0;
    // Use this for initialization
    void Start () {
        //temp
        //BPM = 120;
        beat = 0;
        beatTimer = 0;

        //If a nonsensical BPM is given, auto set to 120
        if(BPM <= 0)
        {
            BPM = 120;
        }

        //If a garbage time range was given just set it to a quarter of the beat speed
        if(timeAcc <= 0.0f)
        {
            timeAcc = 60.0f / (BPM * 4.0f);
        }

        audio1 = (AudioClip)Resources.Load("Sounds/Test/1");
        playAudio.GetComponent<AudioSource>().clip = audio1;

        cManager = GetComponent<ComboManager>();

        screenWidth = mainCamera.pixelWidth;
        screenHeight = mainCamera.pixelHeight;
        spawnLocation = new Vector2(screenWidth / 2 * 1.2f, 0f);
        UICircle.anchoredPosition = new Vector2(-screenWidth / 2 * .8f, 0f);

        //beatsAcrossScreen = 4;
        speed = screenWidth / (beatsAcrossScreen / (BPM / 60f));
    }
	
	// Update is called once per frame
	void Update () {
        beatTimer += Time.deltaTime;
        cManager.BeatHit = false;

        //move circles
        foreach (RectTransform t in hitCircles) {
            t.anchoredPosition -= new Vector2(Time.deltaTime * speed, 0f);
        }

        //beat timing
        if (beatTimer > 60f / BPM) {
            beatTimer -= 60f / BPM;
            //check if map ended
            if (beat < beatmap.Length)
            {
                //display the current beat on screen
                char currentBeat = beatmap[beat];
                //check if empty beat
                if (currentBeat != '-') {
                    GameObject newCircle = GameObject.Instantiate(CirclePrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                    newCircle.transform.SetParent(canvas.transform, false);
                    newCircle.GetComponent<RectTransform>().anchoredPosition = spawnLocation;
                    newCircle.transform.GetChild(0).GetComponent<Text>().text = currentBeat.ToString().ToUpper();
                    //add to list for movement
                    hitCircles.Add(newCircle.GetComponent<RectTransform>());
                }
            }
            hitCurBeat = false;
            
            beat++;

            //Debug.Log(beatmap[beat]);
        }

        //UICircle.GetComponent<Image>().color = Color.white;

        bool anyKeysPressed = false;
        for (int i = 0; i < 26; i++) {
            //check if any keys are pressed for red color
            if (Input.GetKey("" + keys[i])) {
                anyKeysPressed = true;
            }
            //check for accurate beat input
            if (Input.GetKeyDown("" + keys[i])) {
                //Debug.Log(keys[i]);
                if (beat >= beatsAcrossScreen && hitCurBeat == false) {
                    if ((keys[i] == beatmap[beat - beatsAcrossScreen - 1] || keys[i] == beatmap[beat - beatsAcrossScreen])) {
                        hitCurBeat = true;
                        score++;
                        //turn UI circle green on hit
                        UICircle.GetComponent<Image>().color = Color.green;
                        //play hit sound
                        if (hitCurBeat) {
                            playAudio.Play();
                        }
                        //display score
                        Debug.LogWarning("Hit! Score: " + score);
                    }
                }
            }
        }
        //change colors based on current hit state
        if (hitCurBeat) {
            UICircle.GetComponent<Image>().color = Color.green;
        } else if (anyKeysPressed) {
            UICircle.GetComponent<Image>().color = Color.red;
        } else {
            UICircle.GetComponent<Image>().color = Color.white;
        }

        //Check the accuracy
        if(CheckAccuracy())
        {
            cManager.BeatHit = true;

            //Check for combo
            cManager.checkCombo();
        }
    }

    //Method for checking the time accuracy for hitting the notes
    bool CheckAccuracy()
    {
        //If beat is hit within the time range, return true
        if(hitCurBeat && (beatTimer <= (60f / BPM) - (timeAcc / 2.0f) || beatTimer >= 0 + (timeAcc / 2.0f)))
        {
            return true;
        }

        //If out of time period, return false
        return false;
    }
}
