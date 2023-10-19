using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerInUp : MonoBehaviour
{
    public static SoundManagerInUp sm;

    public enum AUDIO
    {
        JUMP,
        CLEAR,
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
