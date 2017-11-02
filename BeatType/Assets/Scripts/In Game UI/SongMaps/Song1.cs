using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song1 : MonoBehaviour {

    List<int> beatmap;
    List<float> beatmapTimes;
    // Use this for initialization
    void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void createSongMap()
    {
        beatmap = new List<int>();
        beatmapTimes = new List<float>();

        addNote(4, 9.9f);
        addNote(5, 10.4f);
        addNote(4, 11.1f);
        addNote(7, 11.8f);
        addNote(5, 12.95f);
        addNote(5, 13.25f);
        addNote(6, 14.8f);
        addNote(7, 15.7f);
        addNote(7, 16.05f);
        
    }              
    void addNote(int key, float time)
    {
        beatmap.Add(key);
        beatmapTimes.Add(time); 
    }

    public List<int> LoadandSetNotes()
    {
        return beatmap;
    }
    public List<float> LoadandSetTimes()
    {
        return beatmapTimes;
    }
}
