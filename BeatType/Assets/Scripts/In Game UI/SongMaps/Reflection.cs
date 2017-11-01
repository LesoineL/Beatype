using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour {

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
        beatmap.Add(1);
        beatmap.Add(2);
        beatmap.Add(3);
        beatmap.Add(4);
        return beatmap;
    }

    public List<float> LoadandSetNoteTimes()
    {
        beatmapTimes.Add(1.0f);
        beatmapTimes.Add(1.5f);
        beatmapTimes.Add(2.0f);
        beatmapTimes.Add(2.5f);
        return beatmapTimes; 
    }
}
