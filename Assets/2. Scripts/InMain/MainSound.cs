using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSound : MonoBehaviour
{
    AudioSource m_asMain;
    float iVolume;

    private void Awake()
    {
        m_asMain = GetComponent<AudioSource>();

        DataManager.Instance.LoadGameData(GameKind.SOUND);

        if(DataManager.instance.data != null)
        {
            iVolume = float.Parse(DataManager.instance.data.recordDataList[0]) / 100f;
            m_asMain.volume = iVolume;
        }
        else
        {
            m_asMain.volume = 1;
        }

    }

    public void OnChangeVlaue()
    {
        DataManager.Instance.LoadGameData(GameKind.SOUND);

        if (DataManager.instance.data != null)
        {
            iVolume = float.Parse(DataManager.instance.data.recordDataList[0]) / 100f;

            if (m_asMain.volume != iVolume)
            {
                m_asMain.volume = iVolume;
            }
        }
    }
}
