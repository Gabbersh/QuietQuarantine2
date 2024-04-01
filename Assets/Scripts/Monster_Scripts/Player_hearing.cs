using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_hearing : MonoBehaviour
{
    private bool playerInTrigger = false;
    private Hearing hearing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HearingRadius")
        {
            playerInTrigger = true;
            hearing = other.transform.parent.GetComponent<Hearing>();

            hearing.CheckSight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HearingRadius")
        {
            playerInTrigger = false;
        }
    }

    void Update()
    {
        if (playerInTrigger && hearing != null)
        {
            hearing.CheckSight();
        }
    }
}
