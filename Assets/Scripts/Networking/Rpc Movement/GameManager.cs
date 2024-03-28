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
            SubmitNewPosition();
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

    private static void SubmitNewPosition()
    {
        if(GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Move"))
        {
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            {
                foreach (var uId in NetworkManager.Singleton.ConnectedClientsIds)
                {
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uId).GetComponent<RpcPlayer>().Move();
                }
            }
            else
            {
                var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                var player = playerObject.GetComponent<RpcPlayer>();
                player.Move();
            }
        }
    }
}
