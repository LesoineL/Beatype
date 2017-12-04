﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //Struct for a point of interest that contains the point of the object and the
    //radius where it will detect the player and spawn the enemy
    struct interestPoint
    {
        public Vector3 point;
        public float detectRadius;
        public float spawnRadius;
        public bool inRange;
    }

    //-----PRIVATE VARIABLES-----
    //Array of interestPoints
    interestPoint[] interPoints;
    //Player object
    GameObject playerObj;
    //Enemy object
    GameObject enemyObj;
    Enemy eScript;

    //-----PUBLIC VARIABLES-----
    //Public arrays to get the information for the interestPoints
    public Vector3[] locationPoints;
    public float[] dRads;
    public float[] sRads;

    //-----CONSTANTS-----
    //Default radius to detect the player
    const float DEFAULT_DRAD = 20.0f;

    //Default radius to spawn the enemy
    const float DEFAULT_SRAD = 30.0f;

	// Use this for initialization
	void Start ()
    {
        //Get the number of points
        int numPoints = locationPoints.Length;

        //If there is at least one, create the interest points
        if(numPoints >= 1)
        {
            interPoints = new interestPoint[numPoints];

            for (int i = 0; i < numPoints; i++)
            {
                interPoints[i].point = locationPoints[i];

                if(i < dRads.Length)
                {
                    interPoints[i].detectRadius = dRads[i];
                }
                else
                {
                    interPoints[i].detectRadius = DEFAULT_DRAD;
                }

                if(i < sRads.Length)
                {
                    interPoints[i].spawnRadius = sRads[i];
                }
                else
                {
                    interPoints[i].spawnRadius = DEFAULT_SRAD;
                }

                interPoints[i].inRange = false;
            }
        }
        //Otherwise create a default interest point
        else
        {
            interPoints = new interestPoint[1];

            interPoints[0].point = new Vector3(0.0f, 0.0f, 0.0f);
            interPoints[0].detectRadius = DEFAULT_DRAD;
            interPoints[0].spawnRadius = DEFAULT_SRAD;
            interPoints[0].inRange = false;
        }

        //Get the player object
        playerObj = GameObject.FindGameObjectWithTag("Player");

        //Get the enemy object
        enemyObj = GameObject.FindGameObjectWithTag("Enemy");
        //Get the enemy's script
        eScript = enemyObj.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		//Detect how far away the player is from a point
        for(int i = 0; i < interPoints.Length; i++)
        {
            Vector3 distanceVec = playerObj.transform.position - interPoints[i].point;
            float distF = distanceVec.magnitude;

            if(distF < interPoints[i].detectRadius && interPoints[i].inRange == false)
            {
                interPoints[i].inRange = true;

                if (eScript.Chasing == false)
                {
                    eScript.CircularTeleportTo(interPoints[i].point, interPoints[i].spawnRadius);
                }
                
            }
            else if(distF > interPoints[i].detectRadius && interPoints[i].inRange == true)
            {
                interPoints[i].inRange = false;
            }

        }
	}
}