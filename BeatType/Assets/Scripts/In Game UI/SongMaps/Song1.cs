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
        addNote(3, 46.51f);
        addNote(2, 46.68f);
        addNote(1, 46.85f);
        addNote(4, 47.04f);
        addNote(3, 47.40f);
        addNote(2, 47.62f);
        addNote(3, 47.98f);
        addNote(2, 48.15f);
        addNote(1, 48.30f);
        addNote(4, 48.47f);
        addNote(3, 48.81f);
        addNote(2, 49.00f);
        addNote(3, 49.39f);
        addNote(2, 49.54f);
        addNote(1, 49.68f);
        addNote(2, 49.88f);
        addNote(3, 50.22f);
        addNote(2, 50.41f);
        addNote(3, 50.79f);
        addNote(4, 50.96f);
        addNote(2, 51.14f);
        addNote(3, 51.31f);
        addNote(2, 51.50f);
        addNote(4, 51.67f);
        addNote(2, 51.86f);
        addNote(3, 52.20f);       
        addNote(3, 52.59f);
        addNote(4, 52.76f);      
        addNote(3, 53.12f);
        addNote(2, 53.29f);
        addNote(1, 53.65f);
        addNote(2, 53.82f);
        addNote(3, 53.99f);
        addNote(2, 54.19f);
        addNote(3, 54.36f);
        addNote(2, 54.55f);
        addNote(3, 54.74f);
        addNote(1, 55.08f);
        addNote(3, 55.40f);
        addNote(2, 55.57f);
        addNote(3, 55.91f);
        addNote(4, 56.13f);

        // slow bridge 
        addNote(3, 57.75f);
        addNote(4, 58.26f);
        addNote(5, 58.77f);
        addNote(7, 59.16f);
        addNote(3, 60.61f);  
        addNote(4, 61.10f);
        addNote(5, 61.65f);
        addNote(7, 61.99f);
        addNote(2, 62.74f);

        addNote(6, 63.47f);
        addNote(6, 63.64f);
        addNote(5, 64.00f);
        addNote(5, 64.17f);
        addNote(4, 64.51f);
        addNote(4, 64.68f);
        addNote(3, 65.04f);
        addNote(3, 65.19f);
        addNote(2, 65.49f);
        addNote(2, 65.68f);
        addNote(1, 66.07f);
        addNote(1, 66.24f);

        addNote(1, 66.62f);
        addNote(1, 66.84f);

        addNote(2, 67.14f);
        addNote(3, 67.48f);
        addNote(4, 67.65f);

        
        addNote(3, 69.18f);
        addNote(4, 69.76f);
        addNote(5, 70.27f);
        addNote(7, 70.61f);
        addNote(3, 72.06f);
        addNote(4, 72.55f);
        addNote(5, 73.11f);
        addNote(7, 73.43f);
        addNote(3, 74.24f);

        //solo 
        addNote(0, 74.90501f);
        addNote(9, 75.012f);
        addNote(8, 75.14001f);
        addNote(0, 75.31001f);
        addNote(9, 75.417f);
        addNote(8, 75.524f);

        addNote(9, 75.694f);
        addNote(8, 75.801f);
        addNote(7, 75.886f);
        addNote(9, 76.078f);
        addNote(8, 76.18501f);
        addNote(7, 76.249f);

        addNote(8, 76.42001f);
        addNote(7, 76.526f);
        addNote(6, 76.59f);
        addNote(8, 76.804f);
        addNote(7, 76.889f);
        addNote(6, 76.996f);

        addNote(9, 77.188f);
        addNote(8, 77.316f);
        addNote(7, 77.422f);

        addNote(5, 77.764f);
        addNote(5, 78.148f);

        addNote(4, 78.297f);
        addNote(3, 79.534f);
        addNote(8, 79.854f);
        addNote(7, 80.217f);
        addNote(9, 80.55801f);
        addNote(8, 81.284f);
        addNote(0, 81.98801f);

        //end hehe
        addNote(0, 83.566f);
        addNote(9, 83.63f);
        addNote(8, 83.73701f);
        addNote(7, 83.886f);
        addNote(6, 83.97201f);
        addNote(5, 84.036f);

        addNote(2, 84.91f);
        addNote(1, 85.358f);
        addNote(3, 85.742f);
        addNote(2, 86.105f);
        addNote(1, 86.48901f);
        addNote(3, 86.788f);
        addNote(4, 87.08601f);
        addNote(5, 87.44901f);
        addNote(3, 87.897f);
        addNote(2, 88.23801f);
        addNote(4, 88.537f);
        addNote(5, 88.87801f);

        addNote(7, 89.19801f);
        addNote(8, 89.262f);
        addNote(9, 89.39001f);

        
        addNote(4, 90.329f);
        addNote(6, 90.77701f);
        addNote(4, 91.118f);
        addNote(7, 91.332f);
        addNote(3, 91.50201f);
        addNote(7, 91.69401f);

        addNote(5, 91.95f);
        addNote(6, 92.206f);
        addNote(7, 92.761f);

        addNote(8, 93.465f);
        addNote(9, 93.508f);
        addNote(0, 93.636f);
        
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
