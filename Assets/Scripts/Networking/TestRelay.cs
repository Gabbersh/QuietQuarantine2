using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : MonoBehaviour
{

    public string joinCode;
    public static TestRelay Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private async void Start()
    {
        
    }

    [Command]
    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);


            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }



    [Command]
    public async void JoinRelay(string joincode)
    {
        try
        {
            Debug.Log("Joining relay with " + joincode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joincode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void StartGame()
    {
        await CreateRelay();
    }

    public string ReturnJoinCode()
    {
        return joinCode;
    }

    //public void ClickJoinRelay()
    //{
    //    JoinRelay(inputText.text);
    //}

}
