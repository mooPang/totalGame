using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerInClay : MonoBehaviour
{
    public static SoundManagerInClay sm;

    public enum AUDIO
    {
        SCORE,
        RELOAD,
        FIRE,
        CLAYFIRE,
        _MAX_
    }

    [SerializeField]
    private List<AudioClip> AudioClips;

    private Dictionary<AUDIO, AudioClip> m_mapAudio;


    private void Awake()
    {
        sm = this;

        m_mapAudio = new Dictionary<AUDIO, AudioClip>();

        for (int idx = 0; idx < AudioClips.Count; idx++)
        {
            m_mapAudio.Add((AUDIO)idx, AudioClips[idx]);
        }
    }


    public AudioClip GetAudioClip(AUDIO eAduio)
    {
        return m_mapAudio[eAduio];
    }
}
