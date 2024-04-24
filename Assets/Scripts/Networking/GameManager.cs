using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject myNemmaJeff;
    [SerializeField] private Spawns enemySpawns;
    [SerializeField] private List<GameObject> jeffs;
    [SerializeField] private int monstersToSpawn;
    private bool monstersSpawned;

    // Start is called before the first frame update
    void Start()
    {
        jeffs = new();
    }

    // constantly check for player count and spawn monster
    void FixedUpdate()
    {
        if (HasPlayerJoined() && !monstersSpawned)
        {
            SpawnMonsters();
            monstersSpawned = true;
        }
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < monstersToSpawn; i++)
        {
            if(!(enemySpawns.EnemySpawns.Count > 0)) return;

            jeffs.Add(Instantiate(myNemmaJeff, GetRandomSpawn(), Quaternion.identity));
        }

        foreach (var networkInstance in jeffs)
        {
            networkInstance.GetComponent<NetworkObject>().Spawn();
        }
    }

    private Vector3 GetRandomSpawn()
    {
        var index = Random.Range(0, enemySpawns.EnemySpawns.Count);
        var chosenSpawn = enemySpawns.EnemySpawns[index].position;
        enemySpawns.EnemySpawns.RemoveAt(index);
        return chosenSpawn;
    }


    private bool HasPlayerJoined()
    {
        return NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClientsList.Count > 0;
    }
}
