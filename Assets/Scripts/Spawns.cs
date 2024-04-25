using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawns : MonoBehaviour
{
    private List<Transform> EnemySpawns;

    void Awake()
    {
        EnemySpawns = gameObject.GetComponentsInChildren<Transform>().Skip(1).ToList();
    }
    public List<Transform> GetSpawnPoints() => EnemySpawns;
        
}
