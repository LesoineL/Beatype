﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float rotationDamping = 5.0f;

    // Update is called once per frame
    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, target.transform.position.z);

        // Set the forward to rotate with time
        transform.forward = Vector3.Lerp(transform.forward, target.GetChild(0).transform.forward, Time.deltaTime * rotationDamping);
    }
}
