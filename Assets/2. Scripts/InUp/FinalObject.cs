using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalObject : MonoBehaviour
{
    private float m_fTimer;

    private void Start()
    {
        m_fTimer = Time.deltaTime;
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * m_fTimer * -15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameObject.SetActive(false);

            //게임 클리어 외치기
        }
    }
}
