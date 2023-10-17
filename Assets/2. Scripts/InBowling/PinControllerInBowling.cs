using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class PinControllerInBowling : MonoBehaviour
{
    /// <summary>
    /// �� �ݶ��̴��� ������ ��ũ��Ʈ
    /// </summary>

    [HideInInspector]
    private GameObject pinCollider;
    
    [Tooltip("�� �� ���� ��ġ")]
    private Vector3 initSpot;

    [Tooltip("���� ����������")]
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

    //�浹�� ������ ���� Ž
    void OnTriggerEnter(Collider other)
    {
        pinCollider = gameObject.GetComponent<BoxCollider>().gameObject;    //������ �� ��忡 ������ �ݶ��̴� (����������)

        if ((other.gameObject.layer == LayerMask.NameToLayer("Lane_Bowling")) || (other.gameObject.layer == LayerMask.NameToLayer("TrashCan_Bowling")))
        {
            //�浹�ؼ� ������ ���� �ݶ��̴� inactive
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

        rb.velocity = Vector3.zero;         //�� ������ �ʱ�ȭ
        rb.angularVelocity = Vector3.zero;  //�� ȸ���� �ʱ�ȭ
        parentPin.transform.position = initSpot;                    //�������� ���� �ֵ��� ���� ��ġ�� �ʱ�ȭ
        parentPin.transform.rotation = Quaternion.Euler(0, 0, 0);   //�� ȸ�� ��ġ �ʱ�ȭ
    }

    public void InitPinDeckSpot()
    {
        isDown = false;
        parentPin.SetActive(true);
        pinCollider.SetActive(true);

        rb.velocity = Vector3.zero;         //�� ������ �ʱ�ȭ
        rb.angularVelocity = Vector3.zero;  //�� ȸ���� �ʱ�ȭ
        parentPin.transform.position = initSpot;                       //�������� ���� �ֵ��� ���� ��ġ�� �ʱ�ȭ
        parentPin.transform.rotation = Quaternion.Euler(0, 0, 0);      //�� ȸ�� ��ġ �ʱ�ȭ
    }
}
