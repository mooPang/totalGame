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
    private float circularMotionSpeed;  //��� �ӵ�

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
        if (!UIManager.Instance.IsActivePause()) //�Ͻ����� �ƴ� ���� �� �巡�� �ǵ���
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
        // ���콺 Ŀ���� ����Ű�� ī�޶� �������ϴ� ���� �հ��� ��ġ ScreenPoint Vector3 position
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);

        // ���콺 Ŀ���� ����Ű�� ī�޶� �������ϴ� ���� �������� ��ġ ScreenPoint Vector3 position
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        // �� ��ġ�� WorldPosition���� ����
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        // RaycastHit ������ ���� ���� ����
        RaycastHit hit;

        // ���� worldMousePosNear���� �����ϰ� worldMousePosFar�� ���ϴ� Raycast�� �����Ѵ�
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        // ������ ���� hit ��ȯ
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
        gameObject.transform.position = initBallSpot;                   //�� ��ġ �ʱ�ȭ
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;   //�� �� �ʱ�ȭ
        GameManagerInBowling.instance.shootingBtnImage.color = new Color32(255, 163, 0, 255);   //��ư�� Ȱ��ȭ ������ ����
        isCrash = false;
    }

    //�浹�� ������ ���� Ž
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
