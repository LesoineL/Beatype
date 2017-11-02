using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song1 : MonoBehaviour {

    List<int> beatmap;
    List<float> beatmapTimes;
    // Use this for initialization
    void Start () {
        beatmap = new List<int>();
        beatmapTimes = new List<float>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public List<int> LoadandSetNoteMap()
    {
        beatmap.Add(4);
        beatmap.Add(5);
        beatmap.Add(4);
        beatmap.Add(7);
        beatmap.Add(5);
        beatmap.Add(5);
        beatmap.Add(6);
        beatmap.Add(7);
        beatmap.Add(7);
        return beatmap;
    }
    public List<float> LoadandSetNoteTimes()
    {
        beatmapTimes.Add(9.9f);
        beatmapTimes.Add(10.4f);
        beatmapTimes.Add(11.1f);
        beatmapTimes.Add(11.8f);
        beatmapTimes.Add(12.95f);
        beatmapTimes.Add(13.25f);

        beatmapTimes.Add(14.8f);
        beatmapTimes.Add(15.7f);
        beatmapTimes.Add(16.05f);

        return beatmapTimes;
    }
}
