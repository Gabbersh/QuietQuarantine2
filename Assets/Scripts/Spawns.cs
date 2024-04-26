using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawns : MonoBehaviour
{
    [Header("Spawn objects")]
    [SerializeField] GameObject monsterSpawnParent;
    [SerializeField] GameObject throwablesSpawnParent;

    [Header("Spawn lists")]
    private List<Transform> monsterSpawns;
    private List<Transform> throwableSpawns;

    void Awake()
    {
        monsterSpawns = monsterSpawnParent.GetComponentsInChildren<Transform>().Skip(1).ToList();
        throwableSpawns = throwablesSpawnParent.GetComponentsInChildren<Transform>().Skip(1).ToList();
    }

    public List<Transform> GetMonsterSpawnPoints() => monsterSpawns;
    public List<Transform> GetThrowableSpawnPoints() => throwableSpawns;
}
