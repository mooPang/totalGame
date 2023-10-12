using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserController : MonoBehaviour
{
    [SerializeField] private FixedJoystick m_fixJoy;  // ���̽�ƽ ȣ��
    [SerializeField] private GameObject m_aimer;      // ���ر�
    [SerializeField] private GameObject m_aimPoint;        // ������
    [SerializeField] private LayerMask m_lmClay;      // Ŭ���� LayerMask;

    RaycastHit m_clayHitInfo;         //Ŭ���� ��Ʈ����
    public float m_fhitDistance;

    private float m_fRotX;        // X�� ȸ��
    private float m_fRotY;        // Y�� ȸ��
    public float m_fRotSpeed;     // ȸ�� �ӵ�

    private float m_fRotMinY = -60; //Y�� ȸ�� �ּҰ�
    private float m_fRotMaxY = 60;  //Y�� ȸ�� �ִ밪
    private float m_fRotMinX = -45; //X�� ȸ�� �ּҰ�
    private float m_fRotMaxX = 0;  //X�� ȸ�� �ִ밪

    private void Awake()
    {
        m_fRotX = m_fRotY = 0;
    }

    private void Update()
    {
        //�켱 �����̽� ���߿� ��ư�������� ��������ߵ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shooting());
        }

        //Debug��
        //OnDrawRayLine();
    }

    private void FixedUpdate()
    {
        JoyControl();
    }

    private void JoyControl()
    {
        //ȸ������ ������ ��ߵȴ� //���� ����
        m_fRotX -= m_fixJoy.Vertical * m_fRotSpeed;
        m_fRotY += m_fixJoy.Horizontal * m_fRotSpeed;

        //x�� y�� ȸ�� ���� ���� ����
        m_fRotX = ClampAngle(m_fRotX, m_fRotMinX, m_fRotMaxX);
        m_fRotY = ClampAngle(m_fRotY, m_fRotMinY, m_fRotMaxY);


        transform.rotation = Quaternion.Euler(m_fRotX, m_fRotY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)   angle += 360;
        if (angle > 360)    angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    IEnumerator Shooting()
    {
        Vector3 vecAimDir = m_aimPoint.transform.position - m_aimer.transform.position;

        if(Physics.Raycast(m_aimer.transform.position, vecAimDir, out m_clayHitInfo, m_fhitDistance, m_lmClay))
        {
            //���⵵ �¾����� ������ ����ߵ�
            Debug.Log("�¾Ҵ�");

            //���� Ŭ���� ��Ȱ��ȭ�ϱ�
            m_clayHitInfo.transform.gameObject.SetActive(false);

            //���� �߰�
            //����Ʈ �߰�
        }

        yield return null;
    }

    public void OnDrawRayLine()
    {
        Transform trAimer = m_aimer.transform;
        Vector3 vecAimDir = m_aimPoint.transform.position - m_aimer.transform.position;

        if (m_clayHitInfo.collider != null)
        {
            Debug.DrawLine(trAimer.position, trAimer.position + vecAimDir * m_clayHitInfo.distance, Color.red);
        }
        else
        {
            Debug.DrawLine(trAimer.position, trAimer.position + vecAimDir * m_fhitDistance, Color.white);
        }
    }
}
