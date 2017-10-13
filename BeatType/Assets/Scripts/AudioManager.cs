using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour {

    AudioClip[] audioFiles;
    AudioSource playAudio;

    // Use this for initialization
    void Start () {
        playAudio = GetComponent<AudioSource>();
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

    public void playAudioFile(int index) // pass in the int for the array index of the audio file
    {
        playAudio.PlayOneShot(audioFiles[index]);
    }
}
