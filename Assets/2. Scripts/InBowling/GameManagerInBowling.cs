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
    private int maxForce = 500;      //슈팅 최대 파워

    [HideInInspector]
    public bool isShoot;
    private int shootBtnClickNum;   //슛버튼 클릭 횟수
    
    [HideInInspector]
    public int downNumber = 0;       //쓰러진 개수
    private int prevScore = 0;

    [HideInInspector]
    public int totalRound = 10;        //총 라운드
    [HideInInspector]
    public int currentRound;       //현재 라운드
    private int currentRoundTrial = 0;  //각 라운드 당 시도1, 시도2

    [HideInInspector]
    public int firstTrialScore = 0;          //첫 시도 점수
    [HideInInspector]
    public int secondTrialScore = 0;         //두번째 시도 점수
    [HideInInspector]
    public int stackTotalScore = 0;         //누적 토탈 점수

    private int finalTrialOnLastRound = 1;      //마지막 10라운드의 마지막 시도
    private bool noMoreTrial = false;

    private float clickTime = 0;
    private bool isClick;

    private AudioSource audioSource;

    [Tooltip("연속 스트라이크 회수 체크용")]
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
        CheckIsDown();  //다 쓰러졌는지 확인

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

    public void ButtonDown()    //외부 버튼에 eventTrigger로 연결됨
    {
        isClick = true;
    }

    public void ButtonUp()
    {
        isClick = false;
        shootBtnClickNum++;

        if (shootBtnClickNum == 1)          //슈팅 게이지일 때 첫 클릭 뗌
        {
            powerGuageBar.gameObject.SetActive(false);
            directionBar.gameObject.SetActive(true);
        }
        else if (shootBtnClickNum == 2)     //방향 설정
        {
            if (shootingChance < 1) //2번 눌러서 공 계속 날아가는 것 막음
            {
                isShoot = true;
                shootingBtnImage.color = Color.gray;    //슛하고 나면 gray처리로 비활성화 느낌주기
                directionBar.SetActive(false);
                ballObject.GetComponent<BallControllerInBowling>().Shoot(); //게이지 다음으로 빼자
                shootingChance++;
                currentTime = 0f;
            }
        }

        //따로 동작할 버튼들
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
        //1차 시도
        if (currentRoundTrial == 1) //10라운드 1시도에서 스트라이크 치면 여기 또 들어오고
        {
            firstTrialScore = downNumber;

            if (downNumber == 10) //스트라이크일 때
            {
                if (finalTrialOnLastRound == 3)
                    noMoreTrial = true;

                if (finalTrialOnLastRound == 2)
                {
                    if (feverTime >= 2) //더블이나 터키면
                    {
                        eState = BowlingState.STRIKE;
                    }
                }

                RoundChange();  //해당 라운드가 끝나


                if (finalTrialOnLastRound != 3)
                {
                    eState = BowlingState.STRIKE;  // 상태 바꿔준다
                    feverTime++;

                    if (feverTime == 2)
                        eState = BowlingState.DOUBLE;
                    else if (feverTime >= 3)
                        eState = BowlingState.TURKEY;
                }
            }
            else    //스트라이크 못 치면
            {
                if (feverTime >= 3) //터키 치고 다음 라운드 1시도 때 스트라이크 못친 경우
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
        else if (currentRoundTrial == 2) //2차 시도
        {
            secondTrialScore = downNumber - firstTrialScore;

            if (downNumber == 10) //스페어 처리 했을 때
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
        //[누적 점수] + [내 라운드 점수] 계산
        stackTotalScore += (firstTrialScore + secondTrialScore);    //현재 라운드에서 기본적으로 추가된 점수

        //[다음 라운드 점수]
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

        //라운드 체인지
        if (currentRound < totalRound)
        {
            currentRound++;
        }
        else if (currentRound == totalRound)
        {
            if (noMoreTrial) //게임 종료
            {
                DataManager.Instance.SaveGameData(GameKind.BOWLING, stackTotalScore.ToString(), true);      //게임 기록 저장
                audioSource.clip = SoundManagerInBowling.instance.GetAudioClip(BowlingSoundState.FINISH);   //FINISH 사운드 플레이
                audioSource.Play();

                Time.timeScale = 0; //동작 멈춤

                //전면 광고 추가
                GoogleMobileVideoAdsScript.instance.LoadInterstitialAd();
                GoogleMobileVideoAdsScript.instance.ShowAd();
                return;
            }

            finalTrialOnLastRound++;

            if (finalTrialOnLastRound == 3)
                eState = BowlingState.NORMAL;
        }


        //핀 위치 초기화
        for (int i = 0; i < eachPinObject.Length; i++)
        {
            eachPinObject[i].InitPinDeckSpot();
        }

        downNumber = 0;       //점수 초기화
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
