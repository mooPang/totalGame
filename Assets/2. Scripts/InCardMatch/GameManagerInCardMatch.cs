using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class GameManagerInCardMatch : MonoBehaviour
{
    [SerializeField]
    public List<int> cardIdList;  //game over ���� IDüũ��

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
        newTime += Time.deltaTime;
        timeText.text = "Time : " + newTime.ToString("N2");
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
