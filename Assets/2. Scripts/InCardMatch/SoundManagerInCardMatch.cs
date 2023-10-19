using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardGameSoundState
{
    MATCH,
    START,
    FINISH
}
public class SoundManagerInCardMatch : MonoBehaviour
{
    public static SoundManagerInCardMatch instance;

    [SerializeField]
    private List<AudioClip> soundList = new List<AudioClip>();

    private Dictionary<CardGameSoundState, AudioClip> soundDic = new Dictionary<CardGameSoundState, AudioClip>();

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
            soundDic.Add((CardGameSoundState)i, soundList[i]);
        }
    }

    public AudioClip GetAudioClip(CardGameSoundState gameState)
    {
        return soundDic[gameState];
    }
}
