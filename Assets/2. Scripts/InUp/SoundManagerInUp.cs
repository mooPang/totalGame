using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerInUp : MonoBehaviour
{
    public static SoundManagerInUp sm;

    [SerializeField]
    private List<AudioClip> AudioClips;

    public Dictionary<int, AudioSource> m_mapAudio;

    public enum AUDIO
    {
        
    }

    private void Awake()
    {
        sm = this;
    }


}
