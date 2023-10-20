using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerInClay : MonoBehaviour
{
    public static GameManagerInClay gm;

    public ClayPoolManager clayPool;        // 클레이 풀 담은곳

    public ClaySpawner claySpawner;         // 클레이 시작지점 스폰장소

    private float m_fTimer;

    [SerializeField]
    private TMP_Text m_txtCurTime;      //남은 시간 텍스트

    private float m_fInitTime;          //초기시간
    private float m_fCurTime;           //현재남은 시간
    private int m_iMinute;              //분
    private int m_iSecond;              //초

    [SerializeField]
    private TMP_Text m_txtScore;        //점수 텍스트
    private int m_iScore;               //점수

    private AudioSource m_asScoreSnd;   //스코어 사운드

    private void Awake()
    {
        gm = this;
        m_fTimer = 0.0f;
        m_fInitTime = 90.0f;
        m_fCurTime = 0.0f;
        m_asScoreSnd = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(StartTimer());
        OnChangeVolume();
    }

    private void Update()
    {
        UpdateClaySpawn();
    }

    private void UpdateClaySpawn()
    {
        m_fTimer += Time.deltaTime;

        if (m_fTimer >= 3.0f)
        {
            int iRanIndex = UnityEngine.Random.Range(1, 3);
            SpawnClay(iRanIndex);
            m_asScoreSnd.clip = SoundManagerInClay.sm.GetAudioClip(SoundManagerInClay.AUDIO.CLAYFIRE);
            m_asScoreSnd.Play();

            m_fTimer = 0.0f;
        }
    }

    public void SpawnClay(int i_SpawnCnt)
    {
        //스폰위치 장소 여기서 지정하고
        GameObject intantClay = clayPool.GetClay();

        int intantRandomNum = UnityEngine.Random.Range(1, 3);
        Transform intantPoint = claySpawner.GetSpawnPoint(intantRandomNum);

        //클레이 위치를 초기화
        intantClay.transform.position = intantPoint.position;

        StartCoroutine(ShotClay(intantClay, intantPoint));
    }

    IEnumerator ShotClay(GameObject i_goClay, Transform i_trSpawnPoint) 
    {
        //날아가는 각도 및 힘 조절함
        GameObject intantClay = i_goClay;
        //스포너의 forward로 하겠슴
        Vector3 clayVec = i_trSpawnPoint.forward;
        clayVec.x += UnityEngine.Random.Range(-0.6f, 0.6f);
        clayVec.y = 0.40f;
        Rigidbody clayRigid = intantClay.GetComponent<Rigidbody>();

        int iRanPow = UnityEngine.Random.Range(60, 80);

        clayRigid.AddForce(clayVec * iRanPow, ForceMode.Impulse);   // 날아가는 힘
        clayRigid.AddTorque(Vector3.up * 70, ForceMode.Impulse);    // 회전력 줌

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
                Debug.Log("시간 끝");
                m_fCurTime = 0;
                Time.timeScale = 0;

                DataManager.Instance.LoadGameData(GameKind.CLAY);
                DataManager.Instance.SaveGameData(GameKind.CLAY, m_txtScore.text, true);

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

    public void OnChangeVolume()
    {
        DataManager.Instance.LoadGameData(GameKind.SOUND);

        if (DataManager.instance.data != null)
        {
            float iVolume = float.Parse(DataManager.instance.data.recordDataList[0]) / 100f;

            if (m_asScoreSnd.volume != iVolume)
            {
                m_asScoreSnd.volume = iVolume;
            }
        }
        else
        {
            m_asScoreSnd.volume = 1;
        }
    }
}
