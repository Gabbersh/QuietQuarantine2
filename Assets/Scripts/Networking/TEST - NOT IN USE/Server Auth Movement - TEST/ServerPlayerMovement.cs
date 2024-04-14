using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
public class ServerPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CinemachineVirtualCamera cv;
    [SerializeField] private AudioListener listener;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float accumulatedRotation;
    public CharacterController cc;

    private MyPlayerInput playerInput;

    private void Start()
    {
        playerInput = new();
        playerInput.Enable();
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            listener.enabled = true;
            cv.Priority = 1;
        }
        else
        {
            cv.Priority = 0;
        }
    }

    private void Update()
    {
        var moveInput = playerInput.Player.Movement.ReadValue<Vector2>();

        if(IsServer && IsLocalPlayer)
        {
            Move(moveInput);
        }
        else if(IsClient && IsLocalPlayer)
        {
            MoveServerRpc(moveInput);
        }

        if(playerInput.Player.RightClick.ReadValue<float>() > 0)
        {
            var mouseDelta = playerInput.Player.Look.ReadValue<Vector2>();
            if(IsServer && IsLocalPlayer)
            {
                LookAround(mouseDelta);
            }
            else if(IsClient && IsLocalPlayer)
            {
                LookAroundServerRpc(mouseDelta);
            }
        }
    }

    private void Move(Vector2 inputVector)
    {
        var calcMove = inputVector.x * playerTransform.right + inputVector.y * playerTransform.forward;
        cc.Move(calcMove * moveSpeed * Time.deltaTime);
    }

    private void LookAround(Vector2 _lookVector)
    {
        float rotationAmount = _lookVector.x * rotationSpeed;

        accumulatedRotation += rotationAmount;

        transform.rotation = quaternion.Euler(0,accumulatedRotation, 0);
    }

    [ServerRpc]
    private void MoveServerRpc(Vector3 _moveVector)
    {
        Move(_moveVector);
    }

    [ServerRpc]
    private void LookAroundServerRpc(Vector2 _lookVector)
    {
        LookAround(_lookVector);
    }
}