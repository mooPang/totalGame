using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalObject : MonoBehaviour
{
    private float m_fTimer;

    private AudioSource m_asClearSnd;

    private void Awake()
    {
        m_asClearSnd = GetComponent<AudioSource>();
    }

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
            StartCoroutine(EndisableObject());

            //게임 클리어 외치기
            m_asClearSnd.clip = SoundManagerInUp.sm.GetAudioClip(SoundManagerInUp.AUDIO.CLEAR);
            m_asClearSnd.Play();
        }
    }

    IEnumerator EndisableObject()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
