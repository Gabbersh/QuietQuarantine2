using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : InteractableObject
{
    private Rigidbody objectRigidBody;
    private Collider objectCollider;

    private Transform playerPickUpPoint;
    private float throwingForce = 15;
    private float maxRandomRotationAngle = 30f;

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
        if (playerPickUpPoint == null)
        {
            this.playerPickUpPoint = FirstPersonController.instance.PickUpPoint;
            objectRigidBody.useGravity = false;
            objectCollider.enabled = false;
        }
    }

    public void Throw()
    {
        objectRigidBody.useGravity = true;
        objectCollider.enabled = true;

        objectRigidBody.AddForce(playerPickUpPoint.transform.forward * throwingForce, ForceMode.Impulse);
        objectRigidBody.rotation = Quaternion.Euler(0f, Random.Range(-maxRandomRotationAngle, maxRandomRotationAngle), 0f);

        objectRigidBody.angularVelocity = Random.insideUnitSphere * throwingForce;

        this.playerPickUpPoint = null;
    }

    public void Drop()
    {
        objectRigidBody.useGravity = true;
        objectCollider.enabled = true;
        this.playerPickUpPoint = null;
    }

    public override void OnLoseFocus()
    {
        Debug.Log("Stopped Looking at throwbject");
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
