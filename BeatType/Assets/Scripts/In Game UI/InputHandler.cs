using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    AudioClip audio1;
    AudioClip audio2;
    AudioClip audio3;
    AudioClip audio4;
    AudioClip audio5;
    AudioClip audio6;
    AudioClip audio7;
    AudioClip audio8;
    AudioClip audio9;
    AudioClip audio10;

    AudioSource playAudio; 

    // Use this for initialization
    void Start () {

        audio1 = (AudioClip)Resources.Load("Sounds/Test/1");
        audio2 = (AudioClip)Resources.Load("Sounds/Test/2");
        audio3 = (AudioClip)Resources.Load("Sounds/Test/3");
        audio4 = (AudioClip)Resources.Load("Sounds/Test/4");
        audio5 = (AudioClip)Resources.Load("Sounds/Test/5");
        audio6 = (AudioClip)Resources.Load("Sounds/Test/6");
        audio7 = (AudioClip)Resources.Load("Sounds/Test/7");
        audio8 = (AudioClip)Resources.Load("Sounds/Test/8");
        audio9 = (AudioClip)Resources.Load("Sounds/Test/9");
        audio10 = (AudioClip)Resources.Load("Sounds/Test/10");
        playAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1)) { playAudio.PlayOneShot(audio1); }

        if (Input.GetKeyDown(KeyCode.Alpha2)) { playAudio.PlayOneShot(audio2); }

        if (Input.GetKeyDown(KeyCode.Alpha3)) { playAudio.PlayOneShot(audio3); }

        if (Input.GetKeyDown(KeyCode.Alpha4)) { playAudio.PlayOneShot(audio4); }

        if (Input.GetKeyDown(KeyCode.Alpha5)) { playAudio.PlayOneShot(audio5); }

        if (Input.GetKeyDown(KeyCode.Alpha6)) { playAudio.PlayOneShot(audio6); }

        if (Input.GetKeyDown(KeyCode.Alpha7)) { playAudio.PlayOneShot(audio7); }

        if (Input.GetKeyDown(KeyCode.Alpha8)) { playAudio.PlayOneShot(audio8); }

        if (Input.GetKeyDown(KeyCode.Alpha9)) { playAudio.PlayOneShot(audio9); }

        if (Input.GetKeyDown(KeyCode.Alpha0)) { playAudio.PlayOneShot(audio10); }
    }
}
