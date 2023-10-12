using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [HideInInspector]
    public bool canDrag;

    public int force = 200; //기본 파워 200     //게이지로 변경할수도

    void Start()
    {

    }

    void Update()
    {
        Shoot(gameObject);
    }

    //슈팅했을 때
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
            //rb.AddTorque  //회전    //시네루

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
}
