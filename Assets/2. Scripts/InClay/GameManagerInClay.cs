using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInClay : MonoBehaviour
{
    public static GameManagerInClay gm;

    public ClayPoolManager clayPool;        // Ŭ���� Ǯ ������

    public ClaySpawner claySpawner;         // Ŭ���� �������� �������

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
}
