using StarterAssets;
using Unity.Netcode;
using UnityEngine;

public class ServerPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform playerTransform;

    public CharacterController cc;

    private void Update()
    {
        var moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(IsServer && IsLocalPlayer)
        {
            Move(moveInput);
        }
        else if(IsClient && IsLocalPlayer)
        {
            MoveServerRpc(moveInput);
        }
    }

    private void Move(Vector2 inputVector)
    {
        var calcMove = inputVector.x * playerTransform.right + inputVector.y * playerTransform.forward;
        cc.Move(calcMove * moveSpeed * Time.deltaTime);
    }

    [ServerRpc]
    private void MoveServerRpc(Vector3 inputVector)
    {
        Move(inputVector);
    }
}