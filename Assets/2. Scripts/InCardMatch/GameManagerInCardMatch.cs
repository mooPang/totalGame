using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class GameManagerInCardMatch : MonoBehaviour
{

    public static GameManagerInCardMatch instance;

    private CardControllerInCardMatch flippedCard;
        
    public TMP_Text timeText;

    private bool isFinished;

    [SerializeField]
    public List<int> cardIdList;  //game over 위한 ID체크용

    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        cardIdList = new List<int>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.clip = SoundManagerInCardMatch.instance.GetAudioClip(CardGameSoundState.START);
        audioSource.Play();
    }

    void Update()
    {
        CheckTime();
        CheckFinishedGame();
    }

    public void CardClicked(CardControllerInCardMatch card)
    {
        card.FlipCard();

        if (flippedCard == null)
            flippedCard = card;
        else
            StartCoroutine(CheckMatchRoutine(flippedCard, card));
    }

    IEnumerator CheckMatchRoutine(CardControllerInCardMatch firstCard, CardControllerInCardMatch secondCard) 
    {
        if (firstCard.cardID == secondCard.cardID)  //카드 비교
        {
            audioSource.clip = SoundManagerInCardMatch.instance.GetAudioClip(CardGameSoundState.MATCH);
            audioSource.Play();

            firstCard.gameObject.SetActive(false);
            secondCard.gameObject.SetActive(false);

            for (int i = 0; i < cardIdList.Count; i++)
            {
                if (cardIdList[i] == firstCard.cardID)
                    cardIdList.Remove(firstCard.cardID);
            }
        }
        else    //다르면 
        {
            yield return new WaitForSeconds(0.1f);     //1초 기다렸다가

            firstCard.FlipCard();
            secondCard.FlipCard();  //다시 카드들 뒤집고

            yield return new WaitForSeconds(0.1f);
        }

        flippedCard = null;
    }

    private void CheckTime()
    {
        timeText.text = "Time : " + Time.time.ToString("N2");
    }

    private void CheckFinishedGame()
    {
        if (isFinished)
            return;

        if (cardIdList.Count == 0)  //카드 다 매칭되면
        {
            isFinished = true;
            audioSource.clip = SoundManagerInCardMatch.instance.GetAudioClip(CardGameSoundState.FINISH);
            audioSource.Play();

            Time.timeScale = 0;     //타이머 및 작동 스톱
        }
    }
}
