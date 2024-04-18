using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject myNemmaJeff;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // constantly check for player count and spawn monster
    void FixedUpdate()
    {
        if (HasPlayerJoined())
        {
            SpawnMonster();
            gameObject.SetActive(false);
        }
    }

    private void SpawnMonster()
    {
        var localMonsterInstance = Instantiate(myNemmaJeff);
        var networkInstance = localMonsterInstance.GetComponent<NetworkObject>();
        networkInstance.Spawn();
    }


    private bool HasPlayerJoined()
    {
        return NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClientsList.Count > 0;
    }
}
