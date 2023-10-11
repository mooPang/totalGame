using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    [SerializeField] private FixedJoystick fixJoy;  // 조이스틱 호출

    private float fRotX;        // X축 회전
    private float fRotY;        // Y축 회전
    public float fRotSpeed;     // 회전 속도

    private void Awake()
    {
        fRotX = fRotY = 0;
    }

    private void FixedUpdate()
    {
        JoyControl();
    }

    private void JoyControl()
    {
        //회전값에 제한을 줘야된다 //아직 안줌
        fRotX -= fixJoy.Vertical * fRotSpeed;
        fRotY += fixJoy.Horizontal * fRotSpeed;

        transform.rotation = Quaternion.Euler(fRotX, fRotY, 0);
    }
}
