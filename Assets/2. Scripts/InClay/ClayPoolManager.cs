using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject clayPrefab;          //클레이 프리팹

    Transform m_trInitClay;                 //초기 트랜스폼

    List<GameObject> clayPool;

    private void Awake()
    {
        clayPool = new List<GameObject>();
        m_trInitClay = clayPrefab.transform;
    }

    public GameObject GetClay()
    {
        GameObject selectClay = null;

        foreach (GameObject item in clayPool)
        {
            if(!item.activeSelf)
            {
                selectClay = item;
                //각도, 위치 초기화
                Rigidbody intantRigid = selectClay.GetComponent<Rigidbody>();
                intantRigid.velocity = Vector3.zero;
                intantRigid.angularVelocity = Vector3.zero;

                selectClay.SetActive(true);
                break;
            }
        }

        if (!selectClay)
        {
            selectClay = Instantiate(clayPrefab, this.transform);
            clayPool.Add(selectClay);
        }

        return selectClay;
    }
}
