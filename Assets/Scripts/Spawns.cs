using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Spawns : MonoBehaviour
{
    [Header("Spawn objects")]
    [SerializeField] GameObject monsterSpawnParent;
    [SerializeField] GameObject throwablesSpawnParent;

    [Header("Spawn lists")]
    private List<Transform> monsterSpawns;
    private List<Transform> throwableSpawns;
    private List<Transform> collectableSpawns;


    void Awake()
    {
        monsterSpawns = monsterSpawnParent.GetComponentsInChildren<Transform>().Skip(1).ToList();

        throwableSpawns = throwablesSpawnParent.GetComponentsInChildren<Transform>().Skip(1).ToList();

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        collectableSpawns = new List<Transform>();

        foreach (GameObject spawnPoint in spawnPoints)
        {
            collectableSpawns.Add(spawnPoint.transform);
        }
    }

    public List<Transform> GetMonsterSpawnPoints() => monsterSpawns;
    public List<Transform> GetThrowableSpawnPoints() => throwableSpawns;
    public List<Transform> GetCollectableSpawnPoints() => collectableSpawns;
}
