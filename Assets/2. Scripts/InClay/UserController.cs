using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserController : MonoBehaviour
{
    [SerializeField] private FixedJoystick m_fixJoy;  // 조이스틱 호출
    [SerializeField] private GameObject m_aimer;      // 조준기
    [SerializeField] private GameObject m_aimPoint;        // 조준점
    [SerializeField] private LayerMask m_lmClay;      // 클레이 LayerMask;

    RaycastHit m_clayHitInfo;         //클레이 히트정보
    public float m_fhitDistance;

    private float m_fRotX;        // X축 회전
    private float m_fRotY;        // Y축 회전
    public float m_fRotSpeed;     // 회전 속도

    private float m_fRotMinY = -60; //Y축 회전 최소값
    private float m_fRotMaxY = 60;  //Y축 회전 최대값
    private float m_fRotMinX = -45; //X축 회전 최소값
    private float m_fRotMaxX = 0;  //X축 회전 최대값

    private void Awake()
    {
        m_fRotX = m_fRotY = 0;
    }

    private void Update()
    {
        //우선 스페이스 나중에 버튼눌렀을때 연동해줘야됨
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shooting());
        }

        //Debug용
        //OnDrawRayLine();
    }

    private void FixedUpdate()
    {
        JoyControl();
    }

    private void JoyControl()
    {
        //회전값에 제한을 줘야된다 //아직 안줌
        m_fRotX -= m_fixJoy.Vertical * m_fRotSpeed;
        m_fRotY += m_fixJoy.Horizontal * m_fRotSpeed;

        //x축 y축 회전 제한 범위 설정
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
            //여기도 맞았으면 뭔가를 해줘야됨
            Debug.Log("맞았다");

            //맞춘 클레이 비활성화하기
            m_clayHitInfo.transform.gameObject.SetActive(false);

            //사운드 추가
            //이펙트 추가
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
