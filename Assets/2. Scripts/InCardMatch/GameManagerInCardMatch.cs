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
    public List<int> cardIdList;  //game over ���� IDüũ��

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

        DataManager.Instance.LoadGameData(GameKind.CARDMATCH);
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
        if (firstCard.cardID == secondCard.cardID)  //ī�� ��
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
        else    //�ٸ��� 
        {
            yield return new WaitForSeconds(0.1f);     //1�� ��ٷȴٰ�

            firstCard.FlipCard();
            secondCard.FlipCard();  //�ٽ� ī��� ������

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

        if (cardIdList.Count == 0)  //ī�� �� ��Ī�Ǹ�
        {
            isFinished = true;
            audioSource.clip = SoundManagerInCardMatch.instance.GetAudioClip(CardGameSoundState.FINISH);
            audioSource.Play();

            Time.timeScale = 0;     //Ÿ�̸� �� �۵� ����

            Debug.LogError(Time.time.ToString());

            DataManager.Instance.SaveGameData(GameKind.CARDMATCH, Time.time.ToString(), false);
        }
    }
}