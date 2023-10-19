using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//ÀÌ³Ñ -> Å°
public enum bowlingSoundState
{
    GUAGE,
    CRASH
}

public class SoundManagerInBowling : MonoBehaviour
{
    public static SoundManagerInBowling instance;

    [SerializeField]
    private List<AudioClip> soundList = new List<AudioClip>();

    private Dictionary<bowlingSoundState, AudioClip> soundDic = new Dictionary<bowlingSoundState, AudioClip>();

    private void Awake()
    {
        instance = this;
        AddSoundDic();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    void AddSoundDic()
    {
        for (int i = 0; i < soundList.Count; i++) 
        {
            soundDic.Add((bowlingSoundState)i, soundList[i]);
        }
    }

    public AudioClip GetAudioClip(bowlingSoundState bowlingState)
    {
        return soundDic[bowlingState];
    }
}
