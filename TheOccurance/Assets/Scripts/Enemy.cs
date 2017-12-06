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
    public float distanceTillDecent;  //Distance until it goes for an attack

    //-----PRIVATE VARIABLES-----
    Vector3 unitMoveVector;  //Normalized vector giving the direction to move on
    Vector3 previousMoveVector;  //The direction it was previously moving
    Vector3 currentPos;  //Current position of the enemy
    float timeLastChase;  //Holds the amount of time since the last chase
    float wDirectionTime;
    float wTime;
    float cChaseTime;
    float velocity;
    bool chasing;
    Rigidbody eBody;
    EnemyStates eState;

    //Enum for enemy states
    enum EnemyStates
    {
        Wandering,
        Chasing,
        Retreating,
        Idle
    }

    //Getter for the chasing property
    public bool Chasing
    {
        get { return chasing; }
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

        //transform.position = CircularTeleportTo(playerObject.transform.position, warpRadius);
        transform.position = Vector3.zero;
        currentPos = transform.position;
        velocity = 0;

        previousMoveVector = unitMoveVector = Vector3.zero;
        cChaseTime = 0;
        eState = EnemyStates.Idle;
	}
	// Update is called once per frame
	void Update ()
    {
        //Calculate the distances on each axis
        float distX = Mathf.Abs(transform.position.x - playerObject.transform.position.x);
        float distY = Mathf.Abs(transform.position.y - playerObject.transform.position.y);
        float distZ = Mathf.Abs(transform.position.z - playerObject.transform.position.z);

        //Calculate the total distance
        float totalDist = distX + distY + distZ;
        float xzDist = distX + distZ;

        //currentPos = transform.position;

        //Check the state
        //Idle state, not doing anything
        if(eState == EnemyStates.Idle)
        {
            timeLastChase += Time.deltaTime;

            //eBody.AddForce(unitMoveVector * speed * -1);

            //Target is in range, chase it
            if(totalDist <= chaseRange)
            {
                eState = EnemyStates.Chasing;
                chasing = true;
                timeLastChase = 0.0f;
            }
        }
        //Wandering state, moving around the map
        else if(eState == EnemyStates.Wandering)
        {
            timeLastChase += Time.deltaTime;
            wTime += Time.deltaTime;

            //-----TODO: Wandering

            if (unitMoveVector == Vector3.zero)
            {
                Vector2 randomPoint = Random.insideUnitCircle;

                Vector3 direction = new Vector3(randomPoint.x, 1.0f, randomPoint.y);

                direction = direction.normalized;

                unitMoveVector = direction;
            }
            else
            {
                if(wTime >= wDirectionTime)
                {
                    wTime = 0.0f;
                    wDirectionTime = Random.Range(0.4f, 3.0f);
                    unitMoveVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                    unitMoveVector = unitMoveVector.normalized;
                }

                GetComponent<Rigidbody>().AddForce(unitMoveVector * 4.0f);
            }
            currentPos = transform.position;

            if(currentPos.y > maxHeight)
            {
                currentPos.y = maxHeight;
                transform.position = currentPos;
            }
            else if(currentPos.y < 2.0f)
            {
                currentPos.y = 2.0f;
                transform.position = currentPos;
            }

            //----------

            //Target is in range, chase it
            if (totalDist <= chaseRange)
            {
                eState = EnemyStates.Chasing;
                chasing = true;
                timeLastChase = 0.0f;
            }
        }
        //Chasing state, chasing the player
        else if(eState == EnemyStates.Chasing)
        {
            cChaseTime += Time.deltaTime;

            if(xzDist < distanceTillDecent)
            {
                MoveToObject(playerObject);
            }
            else
            {
                Vector3 nextPoint = transform.position;
                nextPoint.x -= transform.position.x - playerObject.transform.position.x;
                nextPoint.z -= transform.position.z - playerObject.transform.position.z;

                MoveToPoint(nextPoint);
            }

            if(cChaseTime >= chaseTime && xzDist > distanceTillDecent)
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
            timeLastChase += Time.deltaTime;

            Vector3 nextPoint = transform.position + unitMoveVector;

            if(nextPoint.y > maxHeight)
            {
                nextPoint.y = maxHeight;
            }

            MoveToPoint(nextPoint);

            if(timeLastChase >= retreatTime)
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
            chasing = false;

            Vector2 randomPoint = Random.insideUnitCircle;

            Vector3 direction = new Vector3(randomPoint.x, 1.0f, randomPoint.y);

            direction = direction.normalized;

            unitMoveVector = direction;
        }
    }

    //Move towards the object given
    void MoveToObject(GameObject objToFollow)
    {
        previousMoveVector = unitMoveVector;

        //Get the unit vector of the direction to move
        unitMoveVector = Vector3.Normalize(objToFollow.transform.position - currentPos);

        //eBody.AddForce(unitMoveVector * speed);

        currentPos += unitMoveVector * Time.deltaTime * speed;

        GetComponent<Rigidbody>().MovePosition(currentPos);

        transform.position = currentPos;
    }

    //Move towards a point
    void MoveToPoint(Vector3 point)
    {
        previousMoveVector = unitMoveVector;

        //Get the unit vector of the direction to move
        unitMoveVector = Vector3.Normalize(point - currentPos);

        //eBody.AddForce(unitMoveVector * speed);

        currentPos += unitMoveVector * Time.deltaTime * speed;

        GetComponent<Rigidbody>().MovePosition(currentPos);

        transform.position = currentPos;
    }

    //Teleports the enemy to a point
    void TeleportToPoint(Vector3 point)
    {
        currentPos = point;

        transform.position = point;

        previousMoveVector = unitMoveVector = transform.forward;
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
