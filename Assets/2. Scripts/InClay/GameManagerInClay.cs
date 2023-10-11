using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInClay : MonoBehaviour
{
    public static GameManagerInClay gm;

    public ClayPoolManager clayPool;        // 클레이 풀 담은곳

    public ClaySpawner claySpawner;         // 클레이 시작지점 스폰장소

    private float m_fTimer;

    private void Awake()
    {
        gm = this;
        m_fTimer = 0.0f;
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

            m_fTimer = 0.0f;
        }
    }

    public void SpawnClay(int i_SpawnCnt)
    {
        GameObject intantClay = claySpawner.SpawnClay(i_SpawnCnt);

        StartCoroutine(ShotClay(intantClay));
    }

    IEnumerator ShotClay(GameObject i_goClay) 
    {
        //날아가는 각도 및 힘 조절함
        GameObject intantClay = i_goClay;
        Vector3 clayVec = intantClay.transform.forward;
        clayVec.x += UnityEngine.Random.Range(-0.6f, 0.6f);
        clayVec.y = 0.40f;
        Rigidbody clayRigid = intantClay.GetComponent<Rigidbody>();

        int iRanPow = UnityEngine.Random.Range(60, 80);

        clayRigid.AddForce(clayVec * iRanPow, ForceMode.Impulse);   // 날아가는 힘
        clayRigid.AddTorque(Vector3.back * 70, ForceMode.Impulse);    // 회전력 줌

        yield return null;
    }
}
