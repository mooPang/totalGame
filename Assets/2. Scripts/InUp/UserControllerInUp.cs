using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class UserControllerInUp : MonoBehaviour
{
    [SerializeField]
    private GroundCheck m_groundCheck;

    public FixedJoystick m_fixJoy;

    [Tooltip("�̵� �ӵ�")]
    public float m_fMoveSpeed;

    Rigidbody m_rigidUser;

    [Space(10)]
    [Tooltip("���� �ִ� �Ŀ�")]
    public float m_fMaxJumpPow = 30.0f;

    [Tooltip("���� ���� �Ŀ�")]
    public float m_fCurJumpPow = 5.0f;

    [Tooltip("�̵� ���� ����")]
    Vector3 m_vecMove;
    float m_fMoveX = 0.0f;
    float m_fMoveY = 0.0f;

    [Tooltip("���� ������� bool��")]
    private bool m_bJumpKey;

    [Tooltip("���� �����ϴٴ� bool ��")]
    private bool m_bActiveJump;

    public Vector3 m_vecSpawnPointOffset;
    public Vector3 m_vecCutOffset;

    [SerializeField]
    private Transform m_trSpawnPoint;

    [SerializeField]
    private Transform m_trCutPoint;

    private AudioSource m_asJumpSound;

    private void Awake()
    {
        m_rigidUser = GetComponent<Rigidbody>();
        m_asJumpSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        JumpAndGravity();
        OnGageJumpPower();
    }

    private void LateUpdate()
    {
        m_trSpawnPoint.position = new Vector3(m_trSpawnPoint.position.x, 
                                            transform.position.y + m_vecSpawnPointOffset.y,
                                            m_trSpawnPoint.position.z);

        m_trCutPoint.position = new Vector3(m_trCutPoint.position.x,
                                            transform.position.y + m_vecCutOffset.y,
                                            m_trCutPoint.position.z);
    }

    private void FixedUpdate()
    {
        m_fMoveX = m_fixJoy.Horizontal;
        m_fMoveY = m_fixJoy.Vertical;

        m_vecMove = new Vector3(m_fMoveX, 0, m_fMoveY) * m_fMoveSpeed * Time.deltaTime;
        m_rigidUser.MovePosition(m_rigidUser.position + m_vecMove);

        if (m_vecMove.sqrMagnitude == 0) return;    //�������� ���ٸ� ���������

        Quaternion quatDir = Quaternion.LookRotation(m_vecMove);
        Quaternion quatMove = Quaternion.Slerp(m_rigidUser.rotation, quatDir, 0.3f);
        m_rigidUser.MoveRotation(quatMove);
    }

    private void JumpAndGravity()
    {
        if (m_bActiveJump && m_groundCheck.m_bGrounded)
        {
            m_rigidUser.AddForce(Vector3.up * m_fCurJumpPow, ForceMode.Impulse);
            m_bActiveJump = false;
            m_fCurJumpPow = 5.0f;
        }
    }

    public void OnJumpButtonDown()
    {
        m_bJumpKey = true;
    }

    public void OnJumpButtonUp()
    {
        m_bJumpKey = false;
        m_bActiveJump = true;

        m_asJumpSound.clip = SoundManagerInUp.sm.GetAudioClip(SoundManagerInUp.AUDIO.JUMP);
        m_asJumpSound.Play();
    }

    private void OnGageJumpPower()
    {
        if (!m_bJumpKey) return;
        if (m_bActiveJump) return;

        if (m_fCurJumpPow < m_fMaxJumpPow)
        {
            m_fCurJumpPow += Time.deltaTime * 15;

            if (m_fCurJumpPow >= m_fMaxJumpPow)
                m_fCurJumpPow = m_fMaxJumpPow;
        }
    }
}
