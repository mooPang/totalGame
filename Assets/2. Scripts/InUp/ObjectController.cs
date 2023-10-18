using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float m_fMoveSpeed = 0.0f;

    private Rigidbody m_rigidObj;

    private void Awake()
    {
        m_rigidObj = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //MoveDown();
    }

    void MoveDown()
    {
        if (m_fMoveSpeed <= 0.0f) return;

        m_rigidObj.MovePosition(transform.position + Vector3.down * m_fMoveSpeed * Time.deltaTime);
    }
}
