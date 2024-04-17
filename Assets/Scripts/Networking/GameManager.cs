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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClientsList.Count > 0)
        {
            // Spawn logic
            var playerInstance = Instantiate(myNemmaJeff); // Use a more descriptive name
            var networkObject = playerInstance.GetComponent<NetworkObject>();
            networkObject.Spawn();
            gameObject.SetActive(false);
        }
    }
}
