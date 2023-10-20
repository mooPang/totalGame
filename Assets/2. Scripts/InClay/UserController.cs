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
    private float m_fRotMaxX = 5;  //X축 회전 최대값

    //총알 및 장전관련 변수
    private int m_iCurBullet;       //현재 남은탄
    private float m_fReloadTime;    //리로드 시간
    private bool m_bReloding;       //리로딩 불값

    [SerializeField]
    private GameObject m_goReload;  //리로드 텍스트오브젝트
    [SerializeField]
    private GameObject m_goGun;
    

    private Animator m_animGun;
    [SerializeField]
    private GameObject m_goAimPoint;

    private AudioSource m_asGunSnd;

    private void Awake()
    {
        m_fRotX = m_fRotY = 0;
        m_animGun = m_goGun.GetComponent<Animator>();
        m_asGunSnd = GetComponent<AudioSource>();
    }

    private void Start()
    {
        m_iCurBullet = 2;
        m_fReloadTime = 1.5f;
    }

    private void Update()
    {
        //우선 스페이스 나중에 버튼눌렀을때 연동해줘야됨
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!UIManager.Instance.IsActivePause())
                StartCoroutine(Shooting());
        }
    }

    private void FixedUpdate()
    {
        if (!UIManager.Instance.IsActivePause())
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
        if(m_bReloding)
        {
            //리로딩중이라 커트
            yield break;
        }

        m_asGunSnd.clip = SoundManagerInClay.sm.GetAudioClip(SoundManagerInClay.AUDIO.FIRE);
        m_asGunSnd.Play();

        m_animGun.SetTrigger("trShoot");

        m_iCurBullet--;

        if(m_iCurBullet <= 0)
        {
            StartCoroutine(ReloadAction());
        }

        Vector3 vecAimDir = m_aimPoint.transform.position - m_aimer.transform.position;

        if(Physics.Raycast(m_aimer.transform.position, vecAimDir, out m_clayHitInfo, m_fhitDistance, m_lmClay))
        {
            //맞춘 클레이 비활성화하기
            m_clayHitInfo.transform.gameObject.SetActive(false);

            //점수추가
            GameManagerInClay.gm.UpdateScore();

            //사운드 추가
            //이펙트 추가
        }

        yield return null;
    }

    public void OnClickShoot()
    {
        if (!UIManager.Instance.IsActivePause())
            StartCoroutine(Shooting());
    }

    IEnumerator ReloadAction()
    {
        m_bReloding = true;
        m_animGun.SetTrigger("trReload");
        m_goAimPoint.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        m_goReload.SetActive(true);
        m_asGunSnd.clip = SoundManagerInClay.sm.GetAudioClip(SoundManagerInClay.AUDIO.RELOAD);

        yield return new WaitForSeconds(0.5f);
        m_asGunSnd.Play();
        m_asGunSnd.loop = true;

        yield return new WaitForSeconds(0.8f);
        m_asGunSnd.Stop();
        m_asGunSnd.loop = false;

        yield return new WaitForSeconds(m_fReloadTime);

        m_animGun.SetTrigger("trReady");
        m_bReloding = false;
        m_goReload.SetActive(false);
        m_goAimPoint.SetActive(true);
        m_iCurBullet = 2;
    }
}
