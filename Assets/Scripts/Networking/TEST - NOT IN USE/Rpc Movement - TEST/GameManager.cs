using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject jeff;
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10,10,300,300));
        if(!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }
        GUILayout.EndArea();
    }


    // Ahhhh cursed goofy ass cursed spawning
    private void StartButtons()
    {
        if(GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.StartHost();
            if(NetworkManager.Singleton.IsServer)
            {
                var instance = Instantiate(jeff);
                var instanceNetworkObject = instance.GetComponent<NetworkObject>();
                instanceNetworkObject.Spawn();
            }
        }
        else if(GUILayout.Button("Client"))
        {
            NetworkManager.Singleton.StartClient();
        }
        else if(GUILayout.Button("Server"))
        {
            NetworkManager.Singleton.StartServer();
        }

    }
    
    private void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
        GUILayout.Label("Mode: " + mode);
    }


}
