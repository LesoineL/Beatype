using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    int comboCount;
    float timeFromLastNote;
    float avgTime;
    bool beatHit;
    public float bpm;
    //Allows for a range in time that can vary based on value specified
    public float extraTimeBuffer;

    //Property for beatHit
    public bool BeatHit
    {
        get { return beatHit; }
        set { beatHit = value; }
    }

    //Getter for comboCount
    public int Combo
    {
        get { return comboCount; }
    }

    public float BPM
    {
        set { bpm = value; }
    }

	// Use this for initialization
	void Start ()
    {
        comboCount = 0;
        timeFromLastNote = 0.0f;
        avgTime = 60.0f / bpm;
        beatHit = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //beatHit = false;
        timeFromLastNote += Time.deltaTime;
	}

    //Checks to see if the combo needs to be updated
    public void checkCombo()
    {
        /*
        //Check if missed note
        if(timeFromLastNote > avgTime + extraTimeBuffer && comboCount > 1)
        {
            //Lose combo and reset timeFromLastNote
            comboCount = 0;
            timeFromLastNote = 0.0f;
            Debug.Log("lost combo");  //temporary log until UI is in place
        }
        //Check if they are within the time range for the note
        else if(beatHit == true && (timeFromLastNote >= avgTime - extraTimeBuffer && timeFromLastNote <= avgTime + extraTimeBuffer))
        {
            //Increase combo and reset timeFromLastNote
            comboCount++;
            timeFromLastNote = 0.0f;

            //Display the combo if greater than 1
            if(comboCount > 1)
            {
                Debug.Log("Combo + " + comboCount);  //temporary log until UI is in place
            }
        }
        */

        if(beatHit)
        {
            //Increase combo and reset timeFromLastNote
            comboCount++;

            Debug.Log("Combo + " + comboCount);  //temporary log until UI is in place
        }
        else
        {
            //Lose combo and reset timeFromLastNote
            comboCount = 0;

            Debug.Log("lost combo");  //temporary log until UI is in place
        }
        
    }
}
