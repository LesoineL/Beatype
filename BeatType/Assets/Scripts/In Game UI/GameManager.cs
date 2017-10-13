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
    
    //beats per minute for the current map
    float BPM;
    
    //the beat of the music we're on
    int beat;
    //timer for each individual beat
    float beatTimer;

    char[] keys = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

    char[] beatmap = { '-', '-', 'c', 'a', 't', '-', '-', 'd', 'o', 'g', '-', '-', 'f', 'r', 'o', 'g', '-', '-' };

    float screenWidth;
    float screenHeight;

    float speed;
    Vector2 spawnLocation;
    //defines how many beats it takes circles to move across the entire screen
    float beatsAcrossScreen;
    List<RectTransform> hitCircles = new List<RectTransform>();

    bool hitCurBeat = false;
    float hitTimer;

    int score = 0;
    // Use this for initialization
    void Start () {
        //temp
        BPM = 120;
        beat = 0;
        beatTimer = 0;

        audio1 = (AudioClip)Resources.Load("Sounds/Test/1");
        playAudio.GetComponent<AudioSource>().clip = audio1;

        screenWidth = mainCamera.pixelWidth;
        screenHeight = mainCamera.pixelHeight;
        spawnLocation = new Vector2(screenWidth / 2 * 1.2f, 0f);
        UICircle.anchoredPosition = new Vector2(-screenWidth / 2 * .8f, 0f);

        beatsAcrossScreen = 4;
        speed = screenWidth / (beatsAcrossScreen / (BPM / 60f));
    }
	
	// Update is called once per frame
	void Update () {
        beatTimer += Time.deltaTime;

        //move circles
        foreach (RectTransform t in hitCircles) {
            t.anchoredPosition -= new Vector2(Time.deltaTime * speed, 0f);
        }

        //beat timing
        if (beatTimer > 60f / BPM) {
            beatTimer -= 60f / BPM;
            beat++;
            hitCurBeat = false;
            
            char currentBeat = beatmap[beat];
            if(currentBeat != '-') {
                GameObject newCircle = GameObject.Instantiate(CirclePrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
                newCircle.transform.SetParent(canvas.transform, false);
                newCircle.GetComponent<RectTransform>().anchoredPosition = spawnLocation;
                newCircle.transform.GetChild(0).GetComponent<Text>().text = currentBeat.ToString().ToUpper();
                hitCircles.Add(newCircle.GetComponent<RectTransform>());
            }
            Debug.Log(beatmap[beat]);
        }

        UICircle.GetComponent<Image>().color = Color.white;
        bool anyKeysPressed = false;
        for (int i = 0; i < 26; i++) {
            if (Input.GetKey("" + keys[i])) {
                anyKeysPressed = true;
            }
            if (Input.GetKeyDown("" + keys[i])) {
                
                Debug.Log(keys[i]);
                if (beat > 3 && keys[i] == beatmap[beat - 3] && hitCurBeat == false) {
                    hitCurBeat = true;
                    score++;
                    Debug.LogWarning("Hit! Score: " + score);
                    UICircle.GetComponent<Image>().color = Color.green;
                    if (hitCurBeat) {
                        playAudio.Play();
                    }
                }
            }
        }
        if (hitCurBeat) {
            UICircle.GetComponent<Image>().color = Color.green;
        } else if (anyKeysPressed) {
            UICircle.GetComponent<Image>().color = Color.red;
        } else {
            UICircle.GetComponent<Image>().color = Color.white;
        }
    }
}
