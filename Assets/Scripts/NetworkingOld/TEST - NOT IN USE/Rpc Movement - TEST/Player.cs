using Unity.Netcode;
using UnityEngine;

public class RpcPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Move();
        }
    }
    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else
        {
            SubmitPositionServerRpc();
        }
    }
    
    [ServerRpc]
    private void SubmitPositionServerRpc(ServerRpcParams rpcparams = default)
    {
        Position.Value = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }
    
    void Update()
    {
        transform.position = Position.Value;
    }
}
