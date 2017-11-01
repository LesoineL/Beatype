using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapManager : MonoBehaviour
{
    Dictionary<int, int[]> beatmaps;
    Dictionary<int, float[]> beatTimes;

    //Getter for a beatmap
    public int[] Beatmaps(int index)
    {
        return beatmaps[index];
    }

    //Getter for the times in a beatmap
    public float[] BeatTimes(int index)
    {
        return beatTimes[index];
    }

	// Use this for initialization
	void Start ()
    {
        beatmaps = new Dictionary<int, int[]>();
        beatTimes = new Dictionary<int, float[]>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Creates a new beatmap and times to go with it then returns the index
    public int addNewBeatmap(int size)
    {
        int[] bMap = new int[size];
        float[] fMap = new float[size];

        int index = beatmaps.Count;

        beatmaps.Add(index, bMap);
        beatTimes.Add(index, fMap);

        return index;
    }
}
