using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

enum State
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
    private int maxForce = 500;      //���� �ִ� �Ŀ�
    private bool isShoot;
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

    [Tooltip("���� ��Ʈ����ũ ȸ�� üũ��")]
    private int feverTime = 0;

    private State eState;

    [HideInInspector]
    public int shootingForce = 0;

    [SerializeField]
    private Slider forceUI;

    private void Awake()
    {
        instance = this;
        eState = State.NORMAL;
    }

    private void Start()
    {
        currentRound = 1;
        currentRoundTrial = 1;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        CheckIsDown();  //�� ���������� Ȯ��

        //ShootingController();
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

    public void ButtonDown()    //�ܺ� ��ư�� eventTrigger�� �����
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
    }

    public void ClickButtonUp()
    {
        if (shootingChance < 1) //2�� ������ �� ��� ���ư��� �� ����
        {
            isShoot = true;
            ballObject.GetComponent<BallControllerInBowling>().Shoot();
            shootingChance++;
            currentTime = 0f;
        }
    }

    //void ShootingController()
    //{
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        if (shootingForce < maxForce)    //maxForce 500
    //            shootingForce++;

    //        if (shootingChance < 1)
    //        {
    //            ActiveSlider();
    //        }
    //    }

    //    if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        if (shootingChance < 1) //2�� ������ �� ��� ���ư��� �� ����
    //        {
    //            isShoot = true;
    //            ballObject.GetComponent<BallControllerInBowling>().Shoot();
    //            shootingChance++;
    //            currentTime = 0f;
    //        }
    //    }
    //}

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
                        eState = State.STRIKE;
                    }
                }

                RoundChange();  //�ش� ���尡 ����


                if (finalTrialOnLastRound != 3)
                {
                    eState = State.STRIKE;  // ���� �ٲ��ش�
                    feverTime++;

                    if (feverTime == 2)
                        eState = State.DOUBLE;
                    else if (feverTime >= 3)
                        eState = State.TURKEY;
                }
            }
            else
            {
                if (feverTime >= 3) //��Ű ġ�� ���� ���� 1�õ� �� ��Ʈ����ũ ��ģ ���
                {
                    feverTime = 2;
                    eState = State.DOUBLE;
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
                        eState = State.NORMAL;

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
                    eState = State.SPARE;
                else
                    eState = State.NORMAL;

                feverTime = 0;
            }
            else
            {
                if (currentRound == totalRound)
                    noMoreTrial = true;

                RoundChange();
                eState = State.NORMAL;
                feverTime = 0;
            }
        }

        //Debug.LogError("���� : " + eState);
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
                case State.NORMAL:
                    break;

                case State.SPARE:
                    stackTotalScore += firstTrialScore;
                    break;

                case State.STRIKE:
                    stackTotalScore += (firstTrialScore + secondTrialScore);
                    break;

                case State.DOUBLE:
                    stackTotalScore += (firstTrialScore + secondTrialScore + firstTrialScore);
                    break;

                case State.TURKEY:
                    stackTotalScore += (firstTrialScore + secondTrialScore + firstTrialScore + secondTrialScore);
                    break;
            }
        }
        //Debug.LogError("1 : " + firstTrialScore);
        //Debug.LogError("2 : " + secondTrialScore);
        //Debug.LogError("���� ��Ż���� : " + stackTotalScore);

        //UITextController(); //UI ����

        //���� ü����
        if (currentRound < totalRound)
        {
            currentRound++;
        }
        else if (currentRound == totalRound)
        {
            if (noMoreTrial) //���� ����
            {
                //Debug.LogError("�������� : " + stackTotalScore);
                return;
            }

            finalTrialOnLastRound++;

            if (finalTrialOnLastRound == 3)
                eState = State.NORMAL;
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
}
