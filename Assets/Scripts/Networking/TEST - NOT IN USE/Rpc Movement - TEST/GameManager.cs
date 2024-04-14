using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    private static void StartButtons()
    {
        if(GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.StartHost();
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
    
    private static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
        GUILayout.Label("Mode: " + mode);
    }
}
