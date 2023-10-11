using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClayPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject clayPrefab;

    List<GameObject> clayPool;

    private void Awake()
    {
        clayPool = new List<GameObject>();
    }

    public GameObject GetClay()
    {
        GameObject selectClay = null;

        foreach (GameObject item in clayPool)
        {
            if(!item.activeSelf)
            {
                selectClay = item;
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
