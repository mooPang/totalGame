using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//ÀÌ³Ñ -> Å°
public enum BowlingSoundState
{
    GUAGE,
    CRASH,
    START,
    FINISH
}

public class SoundManagerInBowling : MonoBehaviour
{
    public static SoundManagerInBowling instance;

    [SerializeField]
    private List<AudioClip> soundList = new List<AudioClip>();

    private Dictionary<BowlingSoundState, AudioClip> soundDic = new Dictionary<BowlingSoundState, AudioClip>();

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
            soundDic.Add((BowlingSoundState)i, soundList[i]);
        }
    }

    public AudioClip GetAudioClip(BowlingSoundState bowlingState)
    {
        return soundDic[bowlingState];
    }
}
