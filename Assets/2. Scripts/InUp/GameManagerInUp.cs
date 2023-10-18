using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class GameManagerInUp : MonoBehaviour
{
    public static GameManagerInUp gm;

    [Tooltip("오브젝트 저장한 오브젝트풀")]
    public ObjPoolManager objPool;

    [SerializeField]
    private List<Transform> m_trSpawns;

    private int m_iSpawnListLength;
    private int m_iObjPoolLength;

    [Tooltip("시간 변수")]
    private float m_fTimer = 0.0f;

    [Tooltip("몇 초마다 생성시킬건지")]
    [SerializeField]
    private float m_fCreateTime;

    [Tooltip("몇 개씩 생성시킬 건지")]
    public int m_iCreateNum;

    [Tooltip("중력값 설정")]
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

        //총 길이 받기
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

        //시작하자마다 오브젝트 생성
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

    public void CreateObj(int i_iCreateNum)  //스폰 생성
    {
        int idx = 0;
        int iRandPosNum = 0;
        int iRandObjNum = 0;

        while (true)
        {
            //먼저 위치 변수를 랜덤함수로 뽑아내고
            iRandPosNum = UnityEngine.Random.Range(0, m_iSpawnListLength);

            //만약 한번 선택된 위치면 다시 돈다.
            if (m_listSelectNum[iRandPosNum].bSelected) continue;

            //그 다음 오브젝트 변수를 랜덤함수로 뽑아낸다
            iRandObjNum = UnityEngine.Random.Range(0, m_iObjPoolLength);

            GameObject intantObj = objPool.GetObj(iRandObjNum);

            //오브젝트 초기 위치값, 이동속도 결정
            intantObj.transform.position = m_trSpawns[iRandPosNum].position;

            //오브젝트가 위치에 생성됐기때문에 선택됐다고 true로 바꿔준다.
            SELECTPOSNUM tempSelNum = m_listSelectNum[iRandPosNum];
            tempSelNum.bSelected = true;
            m_listSelectNum[iRandPosNum] = tempSelNum;

            //만드려는 숫자가 초과하면 break;
            idx++;
            if (idx >= i_iCreateNum) break;
        }

        //true 바꿔줬던 사항 초기화
        idx = 0;
        for(; idx < m_iSpawnListLength; idx++)
        {
            SELECTPOSNUM tempSelNum = m_listSelectNum[idx];
            tempSelNum.bSelected = false;
            m_listSelectNum[idx] = tempSelNum;
        }
    }
}
