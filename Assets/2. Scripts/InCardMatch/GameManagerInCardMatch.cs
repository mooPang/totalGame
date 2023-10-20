using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class GameManagerInCardMatch : MonoBehaviour
{
    [SerializeField]
    public List<int> cardIdList;  //game over 위한 ID체크용

    public static GameManagerInCardMatch instance;
    private CardControllerInCardMatch flippedCard;
    public TMP_Text timeText;
    private AudioSource audioSource;
    private float newTime;
    private bool isFinished;

    private void Awake()
    {
        instance = this;
        cardIdList = new List<int>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        DataManager.Instance.LoadGameData(GameKind.CARDMATCH);

        DataManager.Instance.LoadGameData(GameKind.SOUND);
        if (DataManager.instance.data.recordDataList.Count != 0)
            audioSource.volume = float.Parse(DataManager.instance.data.recordDataList[0]) / 100;    //volume : 0 ~ 1
        else
            audioSource.volume = 1;

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
        newTime += Time.deltaTime;
        timeText.text = "Time : " + newTime.ToString("N2");
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

            DataManager.Instance.SaveGameData(GameKind.CARDMATCH, Time.time.ToString(), false);
        }
    }

    public void ChangeSound()
    {
        DataManager.Instance.LoadGameData(GameKind.SOUND);
        if (DataManager.instance.data.recordDataList.Count != 0)
            audioSource.volume = float.Parse(DataManager.instance.data.recordDataList[0]) / 100;    //volume : 0 ~ 1
        else
            audioSource.volume = 1;
    }
}
