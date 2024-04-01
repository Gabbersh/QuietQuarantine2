using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : InteractableObject
{
    private Rigidbody objectRigidBody;
    private Collider objectCollider;

    private Transform playerPickUpPoint;

    private void Start()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }

    public override void OnFocus()
    {
        Debug.Log("Looking at brick");
    }

    public override void OnInteract()
    {
        if (playerPickUpPoint == null)
        {
            this.playerPickUpPoint = FirstPersonController.instance.PickUpPoint;
            objectRigidBody.useGravity = false;
            objectRigidBody.freezeRotation = true;
        }
        else
        {
            this.playerPickUpPoint = null;
            objectRigidBody.useGravity = true;
            objectRigidBody.freezeRotation = false;
        }
    }

    public override void OnLoseFocus()
    {
        Debug.Log("Stopped Looking at brick");
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
