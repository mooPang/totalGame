using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class GameManagerInUp : MonoBehaviour
{
    public static GameManagerInUp gm;

    [Tooltip("������Ʈ ������ ������ƮǮ")]
    public ObjPoolManager objPool;

    [SerializeField]
    private List<Transform> m_trSpawns;

    private int m_iSpawnListLength;
    private int m_iObjPoolLength;

    [Tooltip("�ð� ����")]
    private float m_fTimer = 0.0f;

    [Tooltip("�� �ʸ��� ������ų����")]
    [SerializeField]
    private float m_fCreateTime;

    [Tooltip("�� ���� ������ų ����")]
    public int m_iCreateNum;

    [Tooltip("�߷°� ����")]
    [SerializeField]
    private float m_fGravity;

    [SerializeField]
    private Transform m_trUser;

    public struct SELECTPOSNUM
    {
        public int iSelectNum;
        public bool bSelected { get; set; }
    }

    SELECTPOSNUM sSelectPos;
    public List<SELECTPOSNUM> m_listSelectNum;


    private void Awake()
    {
        gm = this;

        m_listSelectNum = new List<SELECTPOSNUM>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -m_fGravity, 0);

        //�� ���� �ޱ�
        m_iObjPoolLength = objPool.m_iObjPoolLength;
        m_iSpawnListLength = m_trSpawns.Count;

        int idx = 0;
        for (; idx < m_iSpawnListLength; idx++)
        {
            SELECTPOSNUM sSelectPos = new SELECTPOSNUM();
            sSelectPos.iSelectNum = idx;
            sSelectPos.bSelected = false;
            m_listSelectNum.Add(sSelectPos);
        }

        //�������ڸ��� ������Ʈ ����
        CreateObj(m_iCreateNum);
    }

    private void Update()
    {
        m_fTimer += Time.deltaTime;

        if(m_fTimer >= m_fCreateTime)
        {
            //CreateObj(m_iCreateNum, m_iObjMoveSpd);
            m_fTimer = 0.0f;
        }
    }

    public void CreateObj(int i_iCreateNum)  //���� ����
    {
        int idx = 0;
        int iRandPosNum = 0;
        int iRandObjNum = 0;

        while (true)
        {
            //���� ��ġ ������ �����Լ��� �̾Ƴ���
            iRandPosNum = UnityEngine.Random.Range(0, m_iSpawnListLength);

            //���� �ѹ� ���õ� ��ġ�� �ٽ� ����.
            if (m_listSelectNum[iRandPosNum].bSelected) continue;

            //�� ���� ������Ʈ ������ �����Լ��� �̾Ƴ���
            iRandObjNum = UnityEngine.Random.Range(0, m_iObjPoolLength);

            GameObject intantObj = objPool.GetObj(iRandObjNum);

            //������Ʈ �ʱ� ��ġ��, �̵��ӵ� ����
            intantObj.transform.position = m_trSpawns[iRandPosNum].position;

            //������Ʈ�� ��ġ�� �����Ʊ⶧���� ���õƴٰ� true�� �ٲ��ش�.
            SELECTPOSNUM tempSelNum = m_listSelectNum[iRandPosNum];
            tempSelNum.bSelected = true;
            m_listSelectNum[iRandPosNum] = tempSelNum;

            //������� ���ڰ� �ʰ��ϸ� break;
            idx++;
            if (idx >= i_iCreateNum) break;
        }

        //true �ٲ���� ���� �ʱ�ȭ
        idx = 0;
        for(; idx < m_iSpawnListLength; idx++)
        {
            SELECTPOSNUM tempSelNum = m_listSelectNum[idx];
            tempSelNum.bSelected = false;
            m_listSelectNum[idx] = tempSelNum;
        }
    }
}
