using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson; 

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
    interestPoint[] sPoints;
    //Array of collectables
    GameObject[] collectables;
    //Player object
    GameObject playerObj;
    //Enemy object
    GameObject enemyObj;
    Enemy eScript;
    AudioSource playerSource;  //Player audio source
    int collectedItems;
    Text[] canvasTexts;
    bool playerIsSafe;

    //-----PUBLIC VARIABLES-----
    //Public arrays to get the information for the interestPoints
    public Vector3[] locationPoints;
    public float[] dRads;
    public float[] sRads;

    public Vector3[] safePoints;
    public float safeDistance;

    public Terrain terrain;
    public TerrainData tData;
    public Canvas playerCanvas;
    public bool isPaused; 

    //Marker prefab
    public GameObject markerPrefab;

    //-----CONSTANTS-----
    //Default radius to detect the player
    const float DEFAULT_DRAD = 20.0f;

    //Default radius to spawn the enemy
    const float DEFAULT_SRAD = 30.0f;

    //Default radius of safe distance
    const float DEFAULT_SAFE_DISTANCE = 15.0f;

    //-----GETTERS-----
    public TerrainData GetTerrainData
    {
        get { return tData; }
    }

    public AudioSource PlayerSource
    {
        get { return playerSource; }
    }

    public int CollectedItems
    {
        get { return collectedItems; }
        set { collectedItems = value; }
    }

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

                //Set the y values to the height of the terrain at that point + enemy max height
                interPoints[i].point.y = tData.GetHeight((int)locationPoints[i].x, (int)locationPoints[i].z);

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

        for(int i = 0; i < interPoints.Length; i++)
        {
            GameObject.Instantiate(markerPrefab, interPoints[i].point, Quaternion.identity);
        }

        //Safe point setup
        if(safeDistance <= 0)
        {
            safeDistance = DEFAULT_SAFE_DISTANCE;
        }

        sPoints = new interestPoint[safePoints.Length];

        for (int i = 0; i < safePoints.Length; i++)
        {
            sPoints[i].detectRadius = safeDistance;
            sPoints[i].inRange = false;
            sPoints[i].point = safePoints[i];
        }

        playerIsSafe = false;

        //Get the player object
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerSource = playerObj.GetComponent<AudioSource>();

        //Get the enemy object
        enemyObj = GameObject.FindGameObjectWithTag("Enemy");
        //Get the enemy's script
        eScript = enemyObj.GetComponent<Enemy>();

        collectables = GameObject.FindGameObjectsWithTag("Collectable");
        canvasTexts = playerCanvas.GetComponentsInChildren<Text>();
        canvasTexts[0].text = "Collected Items:  " + collectedItems + " / " + collectables.Length;
        isPaused = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused) Time.timeScale = 0; // Game paused  add code in this if statement for a pause menu 
        else Time.timeScale = 1; // game not paused 
        if (Input.GetKeyDown(KeyCode.Escape)) // toggle pause 
        {
            isPaused = !isPaused;
            playerObj.GetComponent<FirstPersonController>().enabled = !playerObj.GetComponent<FirstPersonController>().enabled;
        }

        //Check win condition
        if (collectedItems == collectables.Length)
        {
            endGame();
        }

        //Detect how far away the player is from a point
        for (int i = 0; i < interPoints.Length; i++)
        {
            Vector3 distanceVec = playerObj.transform.position - interPoints[i].point;
            float distF = distanceVec.magnitude;

            if (distF < interPoints[i].detectRadius)
            {
                if(interPoints[i].inRange == false)
                {
                    interPoints[i].inRange = true;
                }

                //Check if the enemy is already chasing the player
                if (eScript.EState != Enemy.EnemyStates.Chasing)
                {
                    distanceVec = enemyObj.transform.position - interPoints[i].point;
                    distF = distanceVec.magnitude;

                    //Check if the enemy needs to be teleported near the player
                    if(distF > interPoints[i].spawnRadius)
                    {
                        eScript.CircularTeleportTo(interPoints[i].point, interPoints[i].spawnRadius);
                    }

                    eScript.EState = Enemy.EnemyStates.Chasing;
                }

            }
            else if (distF > interPoints[i].detectRadius && interPoints[i].inRange == true)
            {
                interPoints[i].inRange = false;
            }

        }

        
        for(int i = 0; i < sPoints.Length; i++)
        {
            //Detect player distance from a safe point
            Vector3 distanceVec = playerObj.transform.position - sPoints[i].point;
            float distF = distanceVec.magnitude;

            if (distF < safeDistance)
            {
                if(sPoints[i].inRange == false)
                {
                    sPoints[i].inRange = true;
                }

                if (eScript.EState == Enemy.EnemyStates.Chasing)
                {
                    eScript.EState = Enemy.EnemyStates.Retreating;
                }

            }
            else if (distF > safeDistance)
            {
                if(sPoints[i].inRange)
                {
                    sPoints[i].inRange = false;
                }
            }
            //Detect enemy distance from a safe point
            distanceVec = enemyObj.transform.position - sPoints[i].point;
            distF = distanceVec.magnitude;

            if (distF < safeDistance)
            {
                eScript.EState = Enemy.EnemyStates.Retreating;
            }
        }

        //Text fade
        if (Time.timeSinceLevelLoad > 3.0f)
        {
            canvasTexts[1].CrossFadeAlpha(0.0f, 1.0f, false);
        }
	}

    //Increases the points for collected items
    public void IncreaseCollectedItems()
    {
        collectedItems++;
        canvasTexts[0].text = "Collected Items:  " + collectedItems + " / " + collectables.Length;
    }

    //Return the game to the title screen or something
    public void endGame()
    {
        EditorSceneManager.LoadScene("GameScene");
    }
}
