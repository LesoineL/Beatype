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

        //intro 
        addNote(4, 9.88f);
        addNote(5, 10.58f);
        addNote(4, 11.3f);
        addNote(7, 11.95f);
        addNote(5, 13.14f);
        addNote(5, 13.38f);
        addNote(6, 14.85f);
        addNote(7, 15.9f);
        addNote(7, 16.2f);
        addNote(4, 17.79f);
        addNote(5, 18.28f);
        addNote(7, 18.77f);
        addNote(4, 19.16f);
        addNote(5, 19.67f);
        addNote(4, 20.24f);
        addNote(5, 20.59f);
        addNote(6, 21.16f);
        addNote(4, 21.65f);
        addNote(6, 21.97f);
        addNote(7, 22.66f);
        addNote(6, 23.4f);
        addNote(9, 24f);
        addNote(6, 25.86f);
        addNote(7, 26.26f);
        addNote(9, 26.79f);
        addNote(7, 28.76f);
        addNote(8, 29.1f);
        addNote(0, 29.67f);

        //bridge 1 
        addNote(4, 34.18f);
        addNote(3, 34.35f);
        addNote(2, 34.54f);
        addNote(1, 34.73f);
        addNote(3, 34.90f);
        addNote(2, 35.07f);
        addNote(1, 35.24f);
        addNote(4, 35.43f);
        addNote(3, 35.84f);
        addNote(2, 36.01f);
        addNote(4, 36.20f);
        addNote(3, 36.56f);
        addNote(2, 36.71f);
        addNote(4, 36.91f);
        addNote(2, 37.25f);
        addNote(1, 37.44f);
        addNote(3, 37.63f);
        addNote(3, 37.80f);
        addNote(2, 37.95f);
        addNote(1, 38.12f);
        addNote(4, 38.31f);
        addNote(3, 38.66f);
        addNote(2, 38.83f);
        addNote(4, 39.04f);
        addNote(3, 39.40f);
        addNote(2, 39.55f);  
        addNote(4, 39.74f);
        addNote(3, 40.06f);      
        addNote(1, 40.26f);      
        addNote(4, 40.43f);
        addNote(3, 40.64f);     
        addNote(4, 40.98f);
        addNote(4, 41.28f);
        addNote(4, 41.58f);
        addNote(3, 41.81f);
        addNote(4, 41.96f);
        addNote(3, 42.28f);
        addNote(4, 42.45f);
        addNote(3, 42.64f);
        addNote(4, 42.94f);
        addNote(7, 43.43f);
        addNote(3, 43.80f);
        addNote(4, 43.97f);
        addNote(5, 44.24f);

        // break to next bridge   
        addNote(4, 45.01f);
        addNote(3, 45.18f);
        addNote(2, 45.38f);
        addNote(1, 45.55f);
        addNote(2, 45.72f);
        addNote(3, 45.91f);
        addNote(4, 46.08f);

        // bridge 2 

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
