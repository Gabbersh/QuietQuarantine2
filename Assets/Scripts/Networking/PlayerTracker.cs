using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerTracker : NetworkBehaviour
{
    public delegate void PlayerJoinedEventHandler();
    public event PlayerJoinedEventHandler OnPlayerJoined;
    public event PlayerJoinedEventHandler OnPlayerLeft;

    public NetworkVariable<int> playerCount = new();
    public NetworkVariable<NetworkObjectReference> playerObjectReference = new();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;
        }
<<<<<<< HEAD
        balls();
    }

    private void balls()
    {
        return;
=======
>>>>>>> parent of ea8b838 (Merge branch 'UIRestore' into VerticalSlice3)
    }

    private void OnClientDisconnectedCallback(ulong obj)
    {
        playerCount.Value--;
        Debug.Log("Player left, current player count: "+ playerCount.Value);
        if (OnPlayerLeft != null) OnPlayerLeft();
    }

    private void OnClientConnectedCallback(ulong obj)
    {
        playerCount.Value++;
        Debug.Log("Player joined, current player count: " + playerCount.Value);
        if (OnPlayerJoined != null) OnPlayerJoined();
    }
}
