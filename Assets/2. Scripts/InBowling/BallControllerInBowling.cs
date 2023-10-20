using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallControllerInBowling : MonoBehaviour
{
    [HideInInspector]
    public bool canDrag;

    private bool isCrash;

    private Vector3 initBallSpot;

    public GameObject DirectionNavi;
    private AudioSource audioSource;

    private float timeForAngle;
    private float circularMotionSpeed;  //원운동 속도

    private int minus = -1;
    private float result = 0f;


    private void Awake()
    {
        initBallSpot = transform.position;
        audioSource = GetComponent<AudioSource>();
        circularMotionSpeed = 100;
    }

    void Start()
    {

    }

    void Update()
    {
        if (!UIManager.Instance.IsActivePause()) //일시정지 아닐 때만 공 드래그 되도록
            DragBall(gameObject);

        FixShootDirection();
    }

    void DragBall(GameObject go)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        Vector3 goForward = go.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (hit.transform == transform)
                canDrag = true;

        }
        else if (Input.GetMouseButtonUp(0))
            canDrag = false;

        if (canDrag)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            transform.position = new Vector3(worldPosition.x, 1f, worldPosition.z);
        }
    }
    private RaycastHit CastRay()
    {
        // 마우스 커서가 가리키는 카메라가 랜더링하는 가장 먼곳의 위치 ScreenPoint Vector3 position
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);

        // 마우스 커서가 가리키는 카메라가 랜더링하는 가장 가까운곳의 위치 ScreenPoint Vector3 position
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        // 각 위치를 WorldPosition으로 변경
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        // RaycastHit 정보를 담을 변수 생성
        RaycastHit hit;

        // 현재 worldMousePosNear에서 시작하고 worldMousePosFar로 향하는 Raycast를 생성한다
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        // 정보를 가진 hit 반환
        return hit;
    }

    public void Shoot()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 goForward = gameObject.transform.forward;
        rb.AddForce(goForward * GameManagerInBowling.instance.shootingForce, ForceMode.Impulse);
    }

    public void ResetAllThing()
    {
        //circularMotionSpeed = 0;
        gameObject.transform.position = initBallSpot;                   //공 위치 초기화
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;   //공 힘 초기화
        GameManagerInBowling.instance.shootingBtnImage.color = new Color32(255, 163, 0, 255);   //버튼색 활성화 색으로 원복
        isCrash = false;
    }

    //충돌할 때마다 여기 탐
    void OnCollisionEnter(Collision other)
    {
        if (isCrash)
            return;

        if ((other.gameObject.layer == LayerMask.NameToLayer("PinBodyCol_Bowling")))
        {
            audioSource.clip = SoundManagerInBowling.instance.GetAudioClip(BowlingSoundState.CRASH);
            audioSource.Play();

            isCrash = true;
        }
    }

    private void FixShootDirection()
    {
        timeForAngle += Time.deltaTime * minus * 10;
        result += Time.deltaTime * circularMotionSpeed * minus;

        if (timeForAngle > 5)
            minus = -1;

        if (timeForAngle < -5)
            minus = 1;

        transform.rotation = Quaternion.Euler(0, result, 0);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BallActiveZone_Bowling") && !GameManagerInBowling.instance.isShoot)
        {
            canDrag = false;
            transform.position = initBallSpot;
        }
    }
}
