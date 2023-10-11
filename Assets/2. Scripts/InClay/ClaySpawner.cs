using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    public GameObject SpawnClay(int i_spawnIndex)
    {
        GameObject clay = GameManagerInClay.gm.clayPool.GetClay();
        clay.transform.position = spawnPoints[i_spawnIndex].transform.position;

        return clay;
    }
}
