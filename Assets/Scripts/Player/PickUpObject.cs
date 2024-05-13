using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickUpObject : InteractableObject
{
    private Rigidbody objectRigidBody;
    private Collider objectCollider;

    private Transform playerPickUpPoint;
    private float throwingForce = 15;
    private float maxRandomRotationAngle = 30f;

    private NetworkVariable<bool> isObjectAvailable = new NetworkVariable<bool>(true); // syncs between all players

    public bool IsObjectAvailable
    {
        get { return isObjectAvailable.Value; }
        set
        {
                if (IsServer)
                {
                    isObjectAvailable.Value = value; // change value directly if isServer (host)
                }
                else if (!IsServer)
                {
                    ChangeObjectAvailabilityServerRpc(); // request server to change value
                }
        }
    }

    private void Start()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        gameObject.layer = LayerMask.NameToLayer("PickUp");
    }

    public override void OnFocus()
    {
        Debug.Log("Looking at throwbject");
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(true);
    }

    public override void OnInteract()
    {
        if (playerPickUpPoint == null && IsObjectAvailable)
        {
            playerPickUpPoint = NetworkManager.LocalClient.PlayerObject.gameObject.GetComponent<FirstPersonController>().PickUpPoint; // get local players PickUpPoint
            objectRigidBody.useGravity = false;
            objectCollider.enabled = false;

            IsObjectAvailable = false; // note property not directly setting network variable
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeObjectAvailabilityServerRpc(ServerRpcParams serverRpcParams = default)
    {
        IsObjectAvailable = !IsObjectAvailable;
    }


    public void Throw()
    {
        objectRigidBody.useGravity = true;
        objectCollider.enabled = true;

        objectRigidBody.AddForce(playerPickUpPoint.transform.forward * throwingForce, ForceMode.Impulse);
        objectRigidBody.rotation = Quaternion.Euler(0f, Random.Range(-maxRandomRotationAngle, maxRandomRotationAngle), 0f);

        objectRigidBody.angularVelocity = Random.insideUnitSphere * throwingForce;

        this.playerPickUpPoint = null;

        IsObjectAvailable = true; // note property not directly setting network variable
    }

    public void Drop()
    {
        objectRigidBody.useGravity = true;
        objectCollider.enabled = true;
        this.playerPickUpPoint = null;

        IsObjectAvailable = true; // note property not directly setting network variable
    }

    public override void OnLoseFocus()
    {
        gameObject.GetComponent<OnFocusHighlight>().ToggleHighlight(false);
    }

    private void FixedUpdate()
    {
        if (playerPickUpPoint != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, playerPickUpPoint.position, Time.deltaTime * lerpSpeed);
            objectRigidBody.MovePosition(newPosition);
            objectRigidBody.rotation = playerPickUpPoint.rotation;
        }
    }

}
