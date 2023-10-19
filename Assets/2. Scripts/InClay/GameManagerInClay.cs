using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerInClay : MonoBehaviour
{
    public static GameManagerInClay gm;

    public ClayPoolManager clayPool;        // Ŭ���� Ǯ ������

    public ClaySpawner claySpawner;         // Ŭ���� �������� �������

    private float m_fTimer;

    [SerializeField]
    private TMP_Text m_txtCurTime;      //���� �ð� �ؽ�Ʈ

    private float m_fInitTime;          //�ʱ�ð�
    private float m_fCurTime;           //���糲�� �ð�
    private int m_iMinute;              //��
    private int m_iSecond;              //��

    [SerializeField]
    private TMP_Text m_txtScore;        //���� �ؽ�Ʈ
    private int m_iScore;               //����

    private AudioSource m_asScoreSnd;   //���ھ� ����

    private void Awake()
    {
        gm = this;
        m_fTimer = 0.0f;
        m_fInitTime = 90.0f;
        m_fCurTime = 0.0f;
        m_asScoreSnd = GetComponent<AudioSource>();
    }

    private void Update()
    {
        UpdateClaySpawn();
    }

    private void Start()
    {
        StartCoroutine(StartTimer());
    }

    private void UpdateClaySpawn()
    {
        m_fTimer += Time.deltaTime;

        if (m_fTimer >= 3.0f)
        {
            int iRanIndex = UnityEngine.Random.Range(1, 3);
            SpawnClay(iRanIndex);

            m_fTimer = 0.0f;
        }
    }

    public void SpawnClay(int i_SpawnCnt)
    {
        //������ġ ��� ���⼭ �����ϰ�
        GameObject intantClay = clayPool.GetClay();

        int intantRandomNum = UnityEngine.Random.Range(1, 3);
        Transform intantPoint = claySpawner.GetSpawnPoint(intantRandomNum);

        //Ŭ���� ��ġ�� �ʱ�ȭ
        intantClay.transform.position = intantPoint.position;

        StartCoroutine(ShotClay(intantClay, intantPoint));
    }

    IEnumerator ShotClay(GameObject i_goClay, Transform i_trSpawnPoint) 
    {
        //���ư��� ���� �� �� ������
        GameObject intantClay = i_goClay;
        //�������� forward�� �ϰڽ�
        Vector3 clayVec = i_trSpawnPoint.forward;
        clayVec.x += UnityEngine.Random.Range(-0.6f, 0.6f);
        clayVec.y = 0.40f;
        Rigidbody clayRigid = intantClay.GetComponent<Rigidbody>();

        int iRanPow = UnityEngine.Random.Range(60, 80);

        clayRigid.AddForce(clayVec * iRanPow, ForceMode.Impulse);   // ���ư��� ��
        clayRigid.AddTorque(Vector3.up * 70, ForceMode.Impulse);    // ȸ���� ��

        yield return null;
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(3.0f);

        m_fCurTime = m_fInitTime;

        while(m_fCurTime > 0)
        {
            m_fCurTime -= Time.deltaTime;
            m_iMinute = (int)m_fCurTime / 60;
            m_iSecond = (int)m_fCurTime % 60;
            m_txtCurTime.text = m_iMinute.ToString("00") + ":" + m_iSecond.ToString("00");
            yield return null;

            if (m_fCurTime <= 0)
            {
                Debug.Log("�ð� ��");
                m_fCurTime = 0;
                yield break;
            }
        }
    }

    public void UpdateScore()
    {
        m_iScore += 100;

        m_txtScore.text = m_iScore.ToString();

        m_asScoreSnd.clip = SoundManagerInClay.sm.GetAudioClip(SoundManagerInClay.AUDIO.SCORE);
        m_asScoreSnd.Play();
    }
}
