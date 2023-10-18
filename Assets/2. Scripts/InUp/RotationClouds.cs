using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationClouds : MonoBehaviour
{
    private float m_fTimer;

    private void Start()
    {
        m_fTimer = Time.deltaTime;
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * m_fTimer * -1.0f);
    }
}
