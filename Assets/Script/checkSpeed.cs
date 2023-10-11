using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkSpeed : MonoBehaviour
{
    private Vector3 oldPosition;
    private Vector3 currentPosition;
    private double velocity;

    void Start()
    {
        oldPosition = transform.position;
    }

    void Update()
    {
        currentPosition = transform.position;
        var dis = (currentPosition - oldPosition);
        var distance = Math.Sqrt(Math.Pow(dis.x, 2) + Math.Pow(dis.y, 2) + Math.Pow(dis.z, 2));
        velocity = distance / Time.deltaTime;
        oldPosition = currentPosition;

        Debug.LogError("currentPosition : " + currentPosition);
    }
}
