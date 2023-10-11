using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    [SerializeField] private FixedJoystick fixJoy;  // ���̽�ƽ ȣ��

    private float fRotX;        // X�� ȸ��
    private float fRotY;        // Y�� ȸ��
    public float fRotSpeed;     // ȸ�� �ӵ�

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
        //ȸ������ ������ ��ߵȴ� //���� ����
        fRotX -= fixJoy.Vertical * fRotSpeed;
        fRotY += fixJoy.Horizontal * fRotSpeed;

        transform.rotation = Quaternion.Euler(fRotX, fRotY, 0);
    }
}
