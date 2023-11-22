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
        OnChangeVolume();
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

            //게임 끝
            GameManagerInUp.gm.GameClear();

            //전면 광고 추가
            GoogleMobileVideoAdsScript.instance.LoadInterstitialAd();
            GoogleMobileVideoAdsScript.instance.ShowAd();
        }
    }

    IEnumerator EndisableObject()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }

    public void OnChangeVolume()
    {
        DataManager.Instance.LoadGameData(GameKind.SOUND);

        if (DataManager.instance.data != null)
        {
            float iVolume = float.Parse(DataManager.instance.data.recordDataList[0]) / 100f;

            if (m_asClearSnd.volume != iVolume)
            {
                m_asClearSnd.volume = iVolume;
            }
        }
        else
        {
            m_asClearSnd.volume = 1;
        }
    }
}
