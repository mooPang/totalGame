using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_trSpawnPoints;

    void Awake()
    {
        m_trSpawnPoints = GetComponentsInChildren<Transform>();
    }

    public Transform GetSpawnPoint(int i_spawnIndex)
    {
        Transform spawnPoint = m_trSpawnPoints[i_spawnIndex];
        return spawnPoint;
    }
}
