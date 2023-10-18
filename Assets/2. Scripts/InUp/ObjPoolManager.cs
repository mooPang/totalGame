using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objPrefabs;

    List<GameObject>[] objPools;

    public int m_iObjPoolLength;

    private void Awake()
    {
        objPools = new List<GameObject>[objPrefabs.Length];

        m_iObjPoolLength = objPools.Length;

        for (int idx = 0; idx < m_iObjPoolLength; idx++)
        {
            objPools[idx] = new List<GameObject>();
        }
    }

    public GameObject GetObj(int i_idx)
    {
        GameObject SelectObj = null;

        foreach (GameObject item in objPools[i_idx])
        {
            if( !item.activeSelf )
            {
                SelectObj = item;
                SelectObj.SetActive(true);
                break;
            }
        }


        if (!SelectObj)
        {
            SelectObj = Instantiate(objPrefabs[i_idx], transform);
            objPools[i_idx].Add(SelectObj);
        }

        return SelectObj;
    }
}
