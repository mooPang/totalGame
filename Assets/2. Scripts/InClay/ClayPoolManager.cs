using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject clayPrefab;          //Ŭ���� ������

    Transform m_trInitClay;                 //�ʱ� Ʈ������

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
                //����, ��ġ �ʱ�ȭ
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
