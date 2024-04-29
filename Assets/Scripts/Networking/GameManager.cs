using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("Monsters")]
    [SerializeField] private GameObject myNemmaJeff;
    [SerializeField] private List<GameObject> jeffs;

    [Header("Throwables")]
    [SerializeField] private GameObject throwable;
    [SerializeField] private List<GameObject> spawnedThrowables;

    [Header("Spawns")]
    [SerializeField] private Spawns spawns;

    [Header("Monster settings")]
    [SerializeField] private int monstersToSpawn;

    [Header("Throwables settings")]
    [SerializeField] private int throwablesToSpawn;

    private bool spawnComplete;

    // Start is called before the first frame update
    void Start()
    {
        jeffs = new();
    }

    // constantly check for player count and spawn monster
    void FixedUpdate()
    {
        if (HasPlayerJoined() && !spawnComplete)
        {
            SpawnMonsters();
            SpawnThrowables();
            spawnComplete = true;
        }
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < monstersToSpawn; i++)
        {
            if(spawns.GetMonsterSpawnPoints().Count <= 0) return;

            jeffs.Add(Instantiate(myNemmaJeff, GetRandomSpawn(spawns.GetMonsterSpawnPoints(), false), Quaternion.identity));
        }

        foreach (var networkInstance in jeffs)
        {
            networkInstance.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void SpawnThrowables()
    {
        for (int i = 0; i < throwablesToSpawn; i++)
        {
            if (spawns.GetThrowableSpawnPoints().Count <= 0) return;

            spawnedThrowables.Add(Instantiate(throwable, GetRandomSpawn(spawns.GetThrowableSpawnPoints(), false), Quaternion.identity));
        }

        foreach(var networkInstance in spawnedThrowables)
        {
            networkInstance.GetComponent<NetworkObject>().Spawn();
        }
    }

    private Vector3 GetRandomSpawn(List<Transform> spawns, bool removeOnSpawn)
    {
        var index = Random.Range(0, spawns.Count);
        var chosenSpawn = spawns[index].position;

        if (removeOnSpawn)
        {
            spawns.RemoveAt(index);
        }

        return chosenSpawn;
    }

    private bool HasPlayerJoined()
    {
        return NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClientsList.Count > 0;
    }
}
