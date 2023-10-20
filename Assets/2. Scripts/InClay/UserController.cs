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
    private float m_fRotMaxX = 5;  //X�� ȸ�� �ִ밪

    //�Ѿ� �� �������� ����
    private int m_iCurBullet;       //���� ����ź
    private float m_fReloadTime;    //���ε� �ð�
    private bool m_bReloding;       //���ε� �Ұ�

    [SerializeField]
    private GameObject m_goReload;  //���ε� �ؽ�Ʈ������Ʈ
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
        //�켱 �����̽� ���߿� ��ư�������� ��������ߵ�
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
        if(m_bReloding)
        {
            //���ε����̶� ĿƮ
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
            //���� Ŭ���� ��Ȱ��ȭ�ϱ�
            m_clayHitInfo.transform.gameObject.SetActive(false);

            //�����߰�
            GameManagerInClay.gm.UpdateScore();

            //���� �߰�
            //����Ʈ �߰�
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
