using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [HideInInspector]
    public bool canDrag;

    public int force = 200; //�⺻ �Ŀ� 200     //�������� �����Ҽ���

    void Start()
    {

    }

    void Update()
    {
        Shoot(gameObject);
    }

    //�������� ��
    void Shoot(GameObject go)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        Vector3 goForward = go.transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (hit.transform == transform)
                canDrag = true;

            Debug.LogError("HOLD ON");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            canDrag = false;
            rb.AddForce(goForward * force, ForceMode.Impulse);
            //rb.AddTorque  //ȸ��    //�ó׷�

            if (force == 0)
                Debug.LogError("Ball power is 0, please input Force power on Ball Inspector");
            else
                Debug.LogError("SHOOT!! ==== POWER : " + force);
        }

        if (canDrag)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            transform.position = new Vector3(worldPosition.x, 1f, worldPosition.z);
        }
        //if (Input.GetMouseButton(0))
        //{
        //    Debug.LogError("HOLD ON");
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Debug.LogError("SHOOT!!");

        //    rb.AddForce(goForward * 100, ForceMode.Impulse);
        //}
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
}
