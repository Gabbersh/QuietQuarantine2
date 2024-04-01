using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupClass : MonoBehaviour
{
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float ThrowingForce;
    [SerializeField] private float PickupRange;
    [SerializeField] private Transform Hand;

    private Rigidbody CurrentObjectRigidbody;
    private Collider CurrentObjectCollider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray Pickupray = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward);

            if (Physics.Raycast(Pickupray, out RaycastHit hitInfo, PickupRange, PickupLayer))
            {
                if (CurrentObjectRigidbody)
                {
                    CurrentObjectRigidbody.isKinematic = false;
                    CurrentObjectCollider.enabled = true;

                    CurrentObjectRigidbody = hitInfo.rigidbody;
                    CurrentObjectCollider = hitInfo.collider;

                    CurrentObjectRigidbody.isKinematic = true;
                    CurrentObjectCollider.enabled = false;
                }
                else
                {
                    CurrentObjectRigidbody = hitInfo.rigidbody;
                    CurrentObjectCollider = hitInfo.collider;

                    CurrentObjectRigidbody.isKinematic = true;
                    CurrentObjectCollider.enabled = false;

                }

                return;
            }

            if (CurrentObjectRigidbody)
            {
                CurrentObjectRigidbody.isKinematic = false;
                CurrentObjectCollider.enabled = true;

                CurrentObjectRigidbody = null;
                CurrentObjectCollider = null;

            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (CurrentObjectRigidbody)
            {
                CurrentObjectRigidbody.isKinematic = false;
                CurrentObjectCollider.enabled = true;

                CurrentObjectRigidbody.AddForce(PlayerCamera.transform.forward * ThrowingForce, ForceMode.Impulse);

                CurrentObjectRigidbody = null;
                CurrentObjectCollider = null;

            }
        }

        if (CurrentObjectRigidbody)
        {
            CurrentObjectRigidbody.position = Hand.position;
            CurrentObjectRigidbody.rotation = Hand.rotation;

        }

    }
}

