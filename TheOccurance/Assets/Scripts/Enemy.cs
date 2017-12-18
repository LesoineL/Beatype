using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //-----PUBLIC VARIABLES-----
    //Get the object of the player
    public GameObject playerObject;
    public float speed;
    public float chaseTime;
    public float chaseRange;  //Essentially the view distance
    public float retreatTime;  //Time it takes to retreat after an attack

    public float maxHeight;  //Maximum height above the terrain it can be
    public float maxSpeed;
    public float maxForce;
    public float distanceTillDecent;  //Distance until it goes for an attack
    public AudioClip soundEffect;  //Sound effect played when near the player

    //-----PRIVATE VARIABLES-----
    Vector3 unitMoveVector;  //Normalized vector giving the direction to move on
    Vector3 previousMoveVector;  //The direction it was previously moving
    Vector3 currentPos;  //Current position of the enemy
    Vector3 velocity;
    Vector3 acceleration;
    Vector3 forces;

    //Distance between player and enemy variables
    Vector3 distanceVec;
    float distanceMag;
    float xZDistanceMag;

    float timeLastChase;  //Holds the amount of time since the last chase
    float wDirectionTime;
    float wTime;
    float cChaseTime;
    Rigidbody eBody;
    EnemyStates eState;
    Manager gMan;

    //Sound effect variables
    //The frequency to play the sound effect
    float frequency = 0.0f;
    //Time since it was last played
    float timeSinceLastPlay;

    //Enum for enemy states
    public enum EnemyStates
    {
        Wandering,
        Chasing,
        Retreating,
        Idle
    }

    //Property for eState
    public EnemyStates EState
    {
        get { return eState; }
        set { eState = value; }
    }

	// Use this for initialization
	void Start ()
    {
        //-----CHECK PUBLIC START VALUES-----
        if(speed < 0)
        {
            speed = 0;
        }

        if(chaseTime <= 0)
        {
            chaseTime = Mathf.Infinity;
        }
        
        if(chaseRange <= 0)
        {
            chaseRange = Mathf.Infinity;
        }

        if(retreatTime <= 0)
        {
            retreatTime = 1.0f;
        }

        if(maxHeight <= 0)
        {
            maxHeight = 10.0f;
        }

        if(distanceTillDecent <= 0)
        {
            distanceTillDecent = 15.0f;
        }

        if(playerObject == null)
        {
            playerObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            playerObject.AddComponent<Rigidbody>();
            playerObject.GetComponent<Rigidbody>().useGravity = false;
        }

        if (GetComponent<Rigidbody>() != null)
        {
            eBody = GetComponent<Rigidbody>();
        }
        else
        {
            eBody = new Rigidbody();
        }
        //-----END CHECK PUBLIC START VALUES-----

        eBody.position = new Vector3(500.0f, 20.0f, 500.0f);
        currentPos = transform.position;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        forces = Vector3.zero;

        //Initialize distance info
        distanceVec = transform.position - playerObject.transform.position;
        distanceMag = distanceVec.magnitude;
        xZDistanceMag = distanceVec.x + distanceVec.z;

        previousMoveVector = unitMoveVector = Vector3.zero;
        cChaseTime = 0;
        eState = EnemyStates.Idle;

        gMan = GameObject.Find("GameManager").GetComponent<Manager>();
	}

    private void FixedUpdate()
    {
        //Get the current velocity vector
        velocity = eBody.velocity;
        //Magnitude of the velocity vector
        float vMag = velocity.magnitude;

        //Clear the forces vector
        forces = Vector3.zero;

        //Get the current position
        currentPos = eBody.position;

        //Check states
        //Idle
        if (eState == EnemyStates.Idle)
        {
            //Come to a stop
            if(vMag == 0)
            {
                return;
            }
            //Set the velocity to 0 since it is essentially stopped
            else if(vMag < 0.01f)
            {
                velocity = Vector3.zero;
            }
            //Slow the enemy down to a stop
            else
            {
                eBody.velocity *= 0.5f;
            }
        }
        //Chasing
        else if(eState == EnemyStates.Chasing)
        {
            if (xZDistanceMag < distanceTillDecent)
            {
                MoveToObject(playerObject);
            }
            else
            {
                Vector3 nextPoint = eBody.position;
                nextPoint.x -= eBody.position.x - playerObject.transform.position.x;
                nextPoint.z -= eBody.position.z - playerObject.transform.position.z;

                MoveToPoint(nextPoint);
            }
        }
        //Retreating
        else if(eState == EnemyStates.Retreating)
        {
            Vector3 nextPoint = eBody.position + unitMoveVector;

            nextPoint.y = gMan.tData.GetHeight((int)eBody.position.x, (int)eBody.position.z) + maxHeight;

            MoveToPoint(nextPoint);
        }

        //Keep from touching the ground
        if(eBody.position.y < gMan.tData.GetHeight((int)eBody.position.x, (int)eBody.position.z) + 2.0f && eState != EnemyStates.Chasing)
        {
            eBody.AddForce(Vector3.up * 2.0f * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        //Else keep below max height
        else if(eBody.position.y > gMan.tData.GetHeight((int)eBody.position.x, (int)eBody.position.z) + maxHeight)
        {
            eBody.AddForce(-Vector3.up * 2.0f * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        Vector3.ClampMagnitude(forces, maxForce);

        //Apply the forces
        eBody.AddForce(forces);

        //clamp velocity
        Vector3.ClampMagnitude(eBody.velocity, maxSpeed);
    }

    // Update is called once per frame
    void Update ()
    {
        //Recalculate distance between player and enemy
        distanceVec = transform.position - playerObject.transform.position;
        distanceMag = distanceVec.magnitude;
        xZDistanceMag = distanceVec.x + distanceVec.z;

        eBody.angularVelocity = Vector3.zero;

        //Update to see if it needs to play the sound effect
        UpdateNearSound();

        //Check the state
        //Idle state, not doing anything
        if (eState == EnemyStates.Idle)
        {
            timeLastChase += Time.deltaTime;

            //Target is in range, chase it
            if(distanceMag <= chaseRange)
            {
                eState = EnemyStates.Chasing;
                timeLastChase = 0.0f;
            }
        }
        //Wandering state, moving around the map
        else if(eState == EnemyStates.Wandering)
        {
            timeLastChase += Time.deltaTime;
            wTime += Time.deltaTime;

            //-----TODO: Wandering

            //----------

            //Target is in range, chase it
            if (distanceMag <= chaseRange)
            {
                eState = EnemyStates.Chasing;
                timeLastChase = 0.0f;
            }
        }
        //Chasing state, chasing the player
        else if(eState == EnemyStates.Chasing)
        {
            cChaseTime += Time.deltaTime;

            if(cChaseTime >= chaseTime && xZDistanceMag > distanceTillDecent)
            {
                eState = EnemyStates.Retreating;
                cChaseTime = 0;

                Vector2 randomPoint = Random.insideUnitCircle;

                Vector3 direction = new Vector3(randomPoint.x, 1.0f, randomPoint.y);

                direction = direction.normalized;

                unitMoveVector = direction;
            }
        }
        //Retreating state, moving away from where it just attacked the player
        else if(eState == EnemyStates.Retreating)
        {
            if(cChaseTime != 0)
            {
                cChaseTime = 0;

                Vector2 randomPoint = Random.insideUnitCircle;

                Vector3 direction = new Vector3(randomPoint.x, 1.0f, randomPoint.y);

                direction = direction.normalized;

                unitMoveVector = direction;
            }

            timeLastChase += Time.deltaTime;

            if (timeLastChase >= retreatTime)
            {
                eState = EnemyStates.Idle;
            }
        }
    }

    //Collision detection
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == playerObject)
        {
            eState = EnemyStates.Retreating;

            Vector2 randomPoint = Random.insideUnitCircle;

            Vector3 direction = new Vector3(randomPoint.x, 1.0f, randomPoint.y);

            direction = direction.normalized;

            unitMoveVector = direction;

            gMan.endGame();
        }
    }

    //Move towards the object given
    void MoveToObject(GameObject objToFollow)
    {
        previousMoveVector = unitMoveVector;

        //Get the unit vector of the direction to move
        unitMoveVector = Vector3.Normalize(objToFollow.transform.position - currentPos);

        forces += unitMoveVector * speed;
    }

    //Move towards a point
    void MoveToPoint(Vector3 point)
    {
        previousMoveVector = unitMoveVector;

        //Get the unit vector of the direction to move
        unitMoveVector = Vector3.Normalize(point - currentPos);

        forces += unitMoveVector * speed;
    }

    //Teleports the enemy to a point
    void TeleportToPoint(Vector3 point)
    {
        currentPos = point;

        eBody.MovePosition(point);

        previousMoveVector = unitMoveVector = transform.forward;

        eBody.velocity = Vector3.zero;
    }

    //Plays a sound effect the closer to the target that it is
    void UpdateNearSound()
    {
        //Update time
        timeSinceLastPlay += Time.deltaTime;

        if(distanceMag < 40.0f)
        {
            //Update frequency to play the sound
            Mathf.Clamp(frequency = distanceMag / 20.0f, 0.1f, 10.0f);

            //Check if it needs to play the sound effect
            if (timeSinceLastPlay >= frequency)
            {
                timeSinceLastPlay = 0;

                //Play sound effect
                //Debug.Log("bubump");
                gMan.PlayerSource.PlayOneShot(soundEffect);
            }
        }
    }

    //Teleports the enemy to a random point of radius rad around the center cPoint
    public Vector3 CircularTeleportTo(Vector3 cPoint, float rad)
    {
        //Get a random raidan around a circle
        float radian = Random.Range(0, 2.0f * Mathf.PI);

        //Get a point on the circle
        Vector3 tpPoint = new Vector3(cPoint.x + Mathf.Cos(radian) * rad, cPoint.y + maxHeight, cPoint.z + Mathf.Sin(radian) * rad);

        //Move to that point
        TeleportToPoint(tpPoint);

        return tpPoint;
    }
}
