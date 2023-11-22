using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public enum BowlingState
{
    NORMAL,
    SPARE,
    STRIKE,
    DOUBLE,
    TURKEY
}

public class GameManagerInBowling : MonoBehaviour
{
    public static GameManagerInBowling instance;
    public GameObject ballObject;
    public Button shootingBtn;
    [HideInInspector]
    public Image shootingBtnImage;
    public PinControllerInBowling[] eachPinObject = new PinControllerInBowling[10];
    private float currentTime = 0f;
    private int shootingChance = 0;
    private int maxForce = 500;      //���� �ִ� �Ŀ�

    [HideInInspector]
    public bool isShoot;
    private int shootBtnClickNum;   //����ư Ŭ�� Ƚ��
    
    [HideInInspector]
    public int downNumber = 0;       //������ ����
    private int prevScore = 0;

    [HideInInspector]
    public int totalRound = 10;        //�� ����
    [HideInInspector]
    public int currentRound;       //���� ����
    private int currentRoundTrial = 0;  //�� ���� �� �õ�1, �õ�2

    [HideInInspector]
    public int firstTrialScore = 0;          //ù �õ� ����
    [HideInInspector]
    public int secondTrialScore = 0;         //�ι�° �õ� ����
    [HideInInspector]
    public int stackTotalScore = 0;         //���� ��Ż ����

    private int finalTrialOnLastRound = 1;      //������ 10������ ������ �õ�
    private bool noMoreTrial = false;

    private float clickTime = 0;
    private bool isClick;

    private AudioSource audioSource;

    [Tooltip("���� ��Ʈ����ũ ȸ�� üũ��")]
    private int feverTime = 0;

    private BowlingState eState;

    [HideInInspector]
    public int shootingForce = 0;

    public Slider powerGuageBar;   //init : active(True)
    public GameObject directionBar;    //init : active(False)

    private void Awake()
    {
        instance = this;
        eState = BowlingState.NORMAL;
        audioSource = GetComponent<AudioSource>();
        shootingBtnImage = shootingBtn.GetComponent<Image>();
    }

    private void Start()
    {
        currentRound = 1;
        currentRoundTrial = 1;
        DataManager.Instance.LoadGameData(GameKind.BOWLING);
        shootBtnClickNum = 0;

        DataManager.Instance.LoadGameData(GameKind.SOUND);
        if (DataManager.instance.data.recordDataList.Count != 0)
            audioSource.volume = float.Parse(DataManager.instance.data.recordDataList[0]) / 100;    //volume : 0 ~ 1
        else
            audioSource.volume = 1;

        audioSource.clip = SoundManagerInBowling.instance.GetAudioClip(BowlingSoundState.START);
        audioSource.Play();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        CheckIsDown();  //�� ���������� Ȯ��

        if (isClick && Time.timeScale != 0)
        {
            clickTime += Time.deltaTime;
            ClickButtonDown();
        }
        else
            clickTime = 0;
    }

    public void InactivePin()
    {
        for (int i = 0; i < eachPinObject.Length; i++)
        {
            eachPinObject[i].CheckBoolIsDown();
        }
    }

    public void ButtonDown()    //�ܺ� ��ư�� eventTrigger�� �����
    {
        isClick = true;
    }

    public void ButtonUp()
    {
        isClick = false;
        shootBtnClickNum++;

        if (shootBtnClickNum == 1)          //���� �������� �� ù Ŭ�� ��
        {
            powerGuageBar.gameObject.SetActive(false);
            directionBar.gameObject.SetActive(true);
        }
        else if (shootBtnClickNum == 2)     //���� ����
        {
            if (shootingChance < 1) //2�� ������ �� ��� ���ư��� �� ����
            {
                isShoot = true;
                shootingBtnImage.color = Color.gray;    //���ϰ� ���� grayó���� ��Ȱ��ȭ �����ֱ�
                directionBar.SetActive(false);
                ballObject.GetComponent<BallControllerInBowling>().Shoot(); //������ �������� ����
                shootingChance++;
                currentTime = 0f;
            }
        }

        //���� ������ ��ư��
        audioSource.Stop();
    }

    public void ClickButtonDown()
    {
        if (shootingForce < maxForce && shootBtnClickNum == 0)    //maxForce 500
            shootingForce++;

        if (shootingChance < 1)
            ActivePowerSlider();

        if (!audioSource.isPlaying && !isShoot)
        {
            audioSource.clip = SoundManagerInBowling.instance.GetAudioClip(BowlingSoundState.GUAGE);
            audioSource.Play();
        }
    }

    void ActivePowerSlider()
    {
        powerGuageBar.value = shootingForce;
    }

    void CheckIsDown()
    {
        if (!isShoot)
            return;
        
        if (prevScore < downNumber)
        {
            prevScore = downNumber;
            currentTime = 0f;
        }
        else if (currentTime >= 7f)
        {
            RoundLogic();
            InactivePin();
            ballObject.GetComponent<BallControllerInBowling>().ResetAllThing();

            currentTime = 0f;
            shootingChance = 0;
            shootingForce = 0;
            powerGuageBar.value = 0;
            shootBtnClickNum = 0;
            powerGuageBar.gameObject.SetActive(true);
            directionBar.gameObject.SetActive(false);
            isShoot = false;
        }
    }

    void RoundLogic()
    {
        //1�� �õ�
        if (currentRoundTrial == 1) //10���� 1�õ����� ��Ʈ����ũ ġ�� ���� �� ������
        {
            firstTrialScore = downNumber;

            if (downNumber == 10) //��Ʈ����ũ�� ��
            {
                if (finalTrialOnLastRound == 3)
                    noMoreTrial = true;

                if (finalTrialOnLastRound == 2)
                {
                    if (feverTime >= 2) //�����̳� ��Ű��
                    {
                        eState = BowlingState.STRIKE;
                    }
                }

                RoundChange();  //�ش� ���尡 ����


                if (finalTrialOnLastRound != 3)
                {
                    eState = BowlingState.STRIKE;  // ���� �ٲ��ش�
                    feverTime++;

                    if (feverTime == 2)
                        eState = BowlingState.DOUBLE;
                    else if (feverTime >= 3)
                        eState = BowlingState.TURKEY;
                }
            }
            else    //��Ʈ����ũ �� ġ��
            {
                if (feverTime >= 3) //��Ű ġ�� ���� ���� 1�õ� �� ��Ʈ����ũ ��ģ ���
                {
                    feverTime = 2;
                    eState = BowlingState.DOUBLE;
                }

                if (finalTrialOnLastRound == 3)
                {
                    noMoreTrial = true;
                    RoundChange();
                    feverTime = 0;
                }

                if (currentRound == totalRound)
                {
                    finalTrialOnLastRound++;
                    if (finalTrialOnLastRound == 3)
                    {
                        noMoreTrial = true; //
                        eState = BowlingState.NORMAL;
                    }

                    feverTime = 0;
                }

                currentRoundTrial++;
            }

        }
        else if (currentRoundTrial == 2) //2�� �õ�
        {
            secondTrialScore = downNumber - firstTrialScore;

            if (downNumber == 10) //����� ó�� ���� ��
            {
                if (finalTrialOnLastRound == 3)
                    noMoreTrial = true;

                RoundChange();

                if (finalTrialOnLastRound != 3)
                    eState = BowlingState.SPARE;
                else
                    eState = BowlingState.NORMAL;

                feverTime = 0;
            }
            else
            {
                if (currentRound == totalRound)
                    noMoreTrial = true;

                RoundChange();
                eState = BowlingState.NORMAL;
                feverTime = 0;
            }
        }
    }

    void RoundChange()
    {
        //[���� ����] + [�� ���� ����] ���
        stackTotalScore += (firstTrialScore + secondTrialScore);    //���� ���忡�� �⺻������ �߰��� ����

        //[���� ���� ����]
        //if (currentRound != totalRound && finalTrialOnLastRound < 3)
        {
            switch (eState)
            {
                case BowlingState.NORMAL:
                    break;

                case BowlingState.SPARE:
                    stackTotalScore += firstTrialScore;
                    break;

                case BowlingState.STRIKE:
                    stackTotalScore += (firstTrialScore + secondTrialScore);
                    break;

                case BowlingState.DOUBLE:
                    stackTotalScore += (firstTrialScore + secondTrialScore + firstTrialScore);
                    break;

                case BowlingState.TURKEY:
                    stackTotalScore += (firstTrialScore + secondTrialScore + firstTrialScore + secondTrialScore);
                    break;
            }
        }

        //���� ü����
        if (currentRound < totalRound)
        {
            currentRound++;
        }
        else if (currentRound == totalRound)
        {
            if (noMoreTrial) //���� ����
            {
                DataManager.Instance.SaveGameData(GameKind.BOWLING, stackTotalScore.ToString(), true);      //���� ��� ����
                audioSource.clip = SoundManagerInBowling.instance.GetAudioClip(BowlingSoundState.FINISH);   //FINISH ���� �÷���
                audioSource.Play();

                Time.timeScale = 0; //���� ����

                //���� ���� �߰�
                GoogleMobileVideoAdsScript.instance.LoadInterstitialAd();
                GoogleMobileVideoAdsScript.instance.ShowAd();
                return;
            }

            finalTrialOnLastRound++;

            if (finalTrialOnLastRound == 3)
                eState = BowlingState.NORMAL;
        }


        //�� ��ġ �ʱ�ȭ
        for (int i = 0; i < eachPinObject.Length; i++)
        {
            eachPinObject[i].InitPinDeckSpot();
        }

        downNumber = 0;       //���� �ʱ�ȭ
        firstTrialScore = 0;
        secondTrialScore = 0;
        currentRoundTrial = 1;
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
