using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour {

    AudioClip[] audioFiles;
    AudioSource playAudio;

    // Use this for initialization
    void Start () {
	// make sure when you attach this script the obj has a AudioSource Component. 
	// Dont need to do anything to the AudioSource Component. Just have one.
        playAudio = GetComponent<AudioSource>();
        playAudio.clip = null; // Makes sure you set a song to play. 
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadAudioFiles(string path) // Path starts by default at Assets/Resources/path
    {
        audioFiles = Resources.LoadAll(path, typeof(AudioClip)).Cast<AudioClip>().ToArray();
    } 

    public void getAudioFileIndex() // Use this to find what index for each audio file, for playing audio
    {
        for (int i = 0; i < audioFiles.Length; i++)
        {
            Debug.Log("File name: " + audioFiles[i].name + " Index: " + i);
        }
    }

    public void setAudioToPlay(int index) // Set the SONG that will be played.
    {
        playAudio.clip = audioFiles[index]; 
    }

    public void playAudioFile() // Play the selected song. 
    {
        if (playAudio.clip == null)
        {
            Debug.Log("Set an audio clip to play in audio manager. ");
        }
        else
        {
            playAudio.Play();
        }
       
    }

    public int getCurrentTime() // returns the current playback time of the song in seconds.
    {
        return playAudio.timeSamples; 
    }
}
