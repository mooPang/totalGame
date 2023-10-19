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
    public PinControllerInBowling[] eachPinObject = new PinControllerInBowling[10];
    private float currentTime = 0f;
    private int shootingChance = 0;
    private int maxForce = 500;      //슈팅 최대 파워
    private bool isShoot;
    
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

    [SerializeField]
    private Slider forceUI;

    private void Awake()
    {
        instance = this;
        eState = BowlingState.NORMAL;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentRound = 1;
        currentRoundTrial = 1;
        DataManager.Instance.LoadGameData(GameKind.BOWLING);
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        CheckIsDown();  //다 쓰러졌는지 확인

        if (isClick)
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
        ClickButtonUp();
    }

    public void ClickButtonDown()
    {
        if (shootingForce < maxForce)    //maxForce 500
            shootingForce++;

        if (shootingChance < 1)
            ActiveSlider();

        if (!audioSource.isPlaying && !isShoot)
        {
            audioSource.clip = SoundManagerInBowling.instance.GetAudioClip(bowlingSoundState.GUAGE);
            audioSource.Play();
        }
    }

    public void ClickButtonUp()
    {
        if (shootingChance < 1) //2번 눌러서 공 계속 날아가는 것 막음
        {
            isShoot = true;
            ballObject.GetComponent<BallControllerInBowling>().Shoot();
            shootingChance++;
            currentTime = 0f;
        }

        audioSource.Stop();
    }

    void ActiveSlider()
    {
        forceUI.value = shootingForce;
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
            forceUI.value = 0;
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
            else
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
                        eState = BowlingState.NORMAL;

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
                DataManager.Instance.SaveGameData(GameKind.BOWLING, stackTotalScore.ToString(), true);
                //Time.timeScale = 0;
                //Debug.LogError("stackTotalScore : " + stackTotalScore);
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
}
