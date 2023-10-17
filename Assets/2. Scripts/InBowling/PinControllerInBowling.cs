using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class PinControllerInBowling : MonoBehaviour
{
    /// <summary>
    /// 핀 콜라이더에 부착된 스크립트
    /// </summary>

    [HideInInspector]
    private GameObject pinCollider;
    
    [Tooltip("핀 덱 최초 위치")]
    private Vector3 initSpot;

    [Tooltip("핀이 쓰러졌는지")]
    private bool isDown;
    private Rigidbody rb;
    private GameObject parentPin;

    private void Awake()
    {
        rb = transform.parent.gameObject.transform.GetComponent<Rigidbody>();
        initSpot = transform.parent.gameObject.transform.position;
        parentPin = transform.parent.gameObject;
        isDown = false;
    }

    //충돌할 때마다 여기 탐
    void OnTriggerEnter(Collider other)
    {
        pinCollider = gameObject.GetComponent<BoxCollider>().gameObject;    //각각의 핀 헤드에 부착된 콜라이더 (점수판정용)

        if ((other.gameObject.layer == LayerMask.NameToLayer("Lane_Bowling")) || (other.gameObject.layer == LayerMask.NameToLayer("TrashCan_Bowling")))
        {
            //충돌해서 기울어진 핀의 콜라이더 inactive
            pinCollider.SetActive(false);
            isDown = true;

            GameManagerInBowling.instance.downNumber++;
        }
    }

    public void CheckBoolIsDown()
    {
        if (isDown)
        {
            parentPin.SetActive(false);
            return;
        }

        rb.velocity = Vector3.zero;         //핀 물리력 초기화
        rb.angularVelocity = Vector3.zero;  //핀 회전력 초기화
        parentPin.transform.position = initSpot;                    //쓰러지지 않은 애들은 기존 위치로 초기화
        parentPin.transform.rotation = Quaternion.Euler(0, 0, 0);   //핀 회전 수치 초기화
    }

    public void InitPinDeckSpot()
    {
        isDown = false;
        parentPin.SetActive(true);
        pinCollider.SetActive(true);

        rb.velocity = Vector3.zero;         //핀 물리력 초기화
        rb.angularVelocity = Vector3.zero;  //핀 회전력 초기화
        parentPin.transform.position = initSpot;                       //쓰러지지 않은 애들은 기존 위치로 초기화
        parentPin.transform.rotation = Quaternion.Euler(0, 0, 0);      //핀 회전 수치 초기화
    }
}
