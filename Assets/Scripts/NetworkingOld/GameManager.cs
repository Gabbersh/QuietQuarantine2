using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using QFSW.QC;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> currentCheatState = new NetworkVariable<int>(0);

    public int CurrentCheatState { get {  return currentCheatState.Value; } }

    public static GameManager instance;

    public delegate void CheatEventHandler();
    public event CheatEventHandler OnCheatStateChange;

    [Header("Monsters")]
    [SerializeField] private GameObject myNemmaJeff;
    [SerializeField] private List<GameObject> jeffs;

    [Header("Throwables")]
    [SerializeField] private GameObject throwableBottle;
    [SerializeField] private GameObject throwableBrick;
    [SerializeField] private List<GameObject> spawnedThrowables;

    [Header("Collectables")]
    [SerializeField] private GameObject Water;
    [SerializeField] private GameObject Coin;
    [SerializeField] private GameObject Medicine;
    [SerializeField] private List<GameObject> spawnedCollectables;
    private GameObject[] collectables; 

    [Header("Spawns")]
    [SerializeField] private Spawns spawns;

    [Header("Monster settings")]
    [SerializeField] private int monstersToSpawn;

    [Header("Throwables settings")]
    [SerializeField] private int throwablesToSpawn;

    [Header("Collectables settings")]
    [SerializeField] private int collectablesToSpawn;

    private bool spawnComplete;

    // Start is called before the first frame update
    void Start()
    {
        jeffs = new();
        collectables = new GameObject[3];
        collectables[0] = Water;
        collectables[1] = Coin;
        collectables[2] = Medicine;
        instance = this;
    }

    // constantly check for player count and spawn monster
    void FixedUpdate()
    {
        if (HasPlayerJoined() && !spawnComplete)
        {
            SpawnMonsters();
            SpawnThrowables();
            SpawnCollectables();
            spawnComplete = true;
        }
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < monstersToSpawn; i++)
        {
            if(spawns.GetMonsterSpawnPoints().Count <= 0) break;

            jeffs.Add(Instantiate(myNemmaJeff, GetRandomSpawn(spawns.GetMonsterSpawnPoints(), true), Quaternion.identity));
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
            if (spawns.GetThrowableSpawnPoints().Count <= 0) break;

            spawnedThrowables.Add(Instantiate(GetRandomItem(), GetRandomSpawn(spawns.GetThrowableSpawnPoints(), true), Quaternion.identity));
        }

        foreach(var networkInstance in spawnedThrowables)
        {
            networkInstance.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void SpawnCollectables()
    {
        for (int i = 0; i < collectablesToSpawn; i++)
        {
            if (spawns.GetCollectableSpawnPoints().Count <= 0) break;

            spawnedCollectables.Add(Instantiate(collectables[Random.Range(0, collectables.Length)], GetRandomSpawn(spawns.GetCollectableSpawnPoints(), true), Quaternion.identity));
        }

        foreach (var networkInstance in spawnedCollectables)
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

    private GameObject GetRandomItem()
    {
        return Random.Range(0, 2) == 1 ? throwableBottle : throwableBrick;
    }

    private bool HasPlayerJoined()
    {
        return NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClientsList.Count > 0;
    }

    [Command("qq-sv-cheats", "Set cheatstate. 0 = Off, 1 = On")] // Vet att man kan använda bool men vill ha sv_cheats 1
    public void SetCheatState(int cheatState)
    {
        if(IsServer)
        {
            currentCheatState.Value = cheatState;
            SendCheatStateClientRpc();
        }
        else
        {
            SetCheatStateServerRpc(cheatState);
        }
        Debug.Log(currentCheatState.Value);
    }

    // send command from client, force server to set it
    [ServerRpc(RequireOwnership = false)]
    private void SetCheatStateServerRpc(int cheatState)
    {
        currentCheatState.Value = cheatState;
        SendCheatStateClientRpc();
    }

    [ClientRpc]
    private void SendCheatStateClientRpc()
    {
        OnCheatStateChange();
    }
}
