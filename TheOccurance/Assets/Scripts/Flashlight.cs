using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    Light flashLight;

    //Values to bound the timer controlling the time between flickers
    public float lowTimerBound, highTimerBound;
    //Values to bound the light intensity of each flicker
    public float lowIntensityBound, highIntensityBound;
    //Values to bound the duration of each flicker
    public float lowDurationBound, highDurationBound;

    //Values to store the randomly generated timer, intenstity, and duration
    [SerializeField]
    float timer, intensity, duration;

    //Values to set the range and spot angle of the light and FOV of the camera when zoomed, and the rate at which the camera zooms in
    public float zoomRange, zoomAngle, zoomFOV, zoomSpeed;

    bool flickerEngaged;

	// Use this for initialization
	void Start ()
    {
        flashLight = gameObject.GetComponent<Light>();
        
        timer = Random.Range(lowTimerBound, highTimerBound);

        if (lowIntensityBound < 0.1f || lowIntensityBound > 1.0f)
            lowIntensityBound = 0.1f;
        if (highIntensityBound > 1.0f || highIntensityBound < 0.1f)
            highIntensityBound = 1.0f;

        flickerEngaged = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Hitting the 'F' key toggles the flashlight on and off
        if (Input.GetKeyDown(KeyCode.F))
            flashLight.enabled = !flashLight.enabled;

        //Holding the right mouse button zooms in the flashlight and camera
        if (Input.GetMouseButton(1))
        {
            flashLight.spotAngle = Mathf.Lerp(flashLight.spotAngle, zoomAngle, zoomSpeed * Time.deltaTime);
            flashLight.range = Mathf.Lerp(flashLight.range, zoomRange, zoomSpeed * Time.deltaTime);
            Camera.current.fieldOfView = Mathf.Lerp(Camera.current.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
        }
        else
        {
            flashLight.spotAngle = Mathf.Lerp(flashLight.spotAngle, 80.0f, zoomSpeed * Time.deltaTime);
            flashLight.range = Mathf.Lerp(flashLight.range, 30.0f, zoomSpeed * Time.deltaTime);
            Camera.current.fieldOfView = Mathf.Lerp(Camera.current.fieldOfView, 60.0f, zoomSpeed * Time.deltaTime);
        }

        //If the light is not engaged in a flicker, the timer between them counts down
        if (!flickerEngaged)
        {
            //If the timer has not reached zero, the timer decrements
            if (timer > 0.0f)
                timer -= Time.deltaTime * 1000.0f;
            //Once it reaches zero, a flicker is engaged
            else if (timer <= 0.0f)
            {
                flickerEngaged = true;
                //Flicker duration and intensity are randomly determined
                duration = Random.Range(lowDurationBound, highDurationBound);
                intensity = Random.Range(lowIntensityBound, highIntensityBound);
            }
        }
        //If the light is engaged in a flicker, the intensity of the light is altered until its reached its duration
        else if (flickerEngaged)
        {
            //If the duration has not reached zero, the light is set to the determined intensity, and the duration decrements
            if (duration > 0.0f)
            {
                duration -= Time.deltaTime * 1000.0f;
                if (flashLight.intensity != intensity)
                    flashLight.intensity = intensity;
            }
            //Once the duration has reached zero, the flicker is disengaged
            else if (duration <= 0.0f)
            {
                flickerEngaged = false;
                //Timer is randomly determined
                timer = Random.Range(lowTimerBound, highTimerBound);
                flashLight.intensity = 1.0f;
            }
        }
    }
}
