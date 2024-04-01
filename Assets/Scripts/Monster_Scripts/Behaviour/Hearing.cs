using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Collider hearingCollider;
    private Animator animator;

    void Start()
    {
        Transform hearingRadius = transform.Find("HearingRadius");

        hearingCollider = hearingRadius.GetComponent<Collider>();
        
        animator = transform.GetComponent<Animator>();
    }

    public void CheckSight()
    {
        if (hearingCollider == null)
        {
            Debug.LogError("Hearing collider not found!");
            return;
        }

        Vector3 centerOfHearing = hearingCollider.bounds.center;
        Vector3 centerOfPlayer = player.transform.position + Vector3.up * (player.GetComponent<Collider>().bounds.size.y / 2);

        Vector3 directionToPlayer = centerOfPlayer - centerOfHearing;

        RaycastHit rayHit;
        if (Physics.Raycast(centerOfHearing, directionToPlayer, out rayHit))
        {
            Debug.Log("Raycast hit: " + rayHit.collider.gameObject.name);

            if (rayHit.collider.gameObject.name == "Player")
            {
                Debug.Log("Player spotted!");
                animator.SetBool("isCrawling", false);
                animator.SetBool("toCrawl", false);
                animator.SetBool("isChasing", true);
            }
        }
    }

    void Update()
    {
        if (hearingCollider == null)
        {
            Debug.LogError("Hearing collider not found!");
            return;
        }

        Vector3 centerOfHearing = hearingCollider.bounds.center;
        Vector3 centerOfPlayer = player.transform.position + Vector3.up * (player.GetComponent<Collider>().bounds.size.y / 2);

        Vector3 directionToPlayer = centerOfPlayer - centerOfHearing;

        // Draw the line from the center of both objects
        Debug.DrawRay(centerOfHearing, directionToPlayer, Color.green);
    }
}
