using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hearing : MonoBehaviour, IHear
{
    [SerializeField] GameObject player;
    private Collider hearingCollider;
    private Animator animator;
    private bool playerInTrigger, hearingSound;

    private float attackDistance = 4.5f;

    [SerializeField] GameObject deathCam;
    [SerializeField] Transform camPos;
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            CheckSight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

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
        Vector3 centerOfPlayer = player.transform.position;

        Vector3 directionToPlayer = centerOfPlayer - centerOfHearing;

        RaycastHit rayHit;
        if (Physics.Raycast(centerOfHearing, directionToPlayer, out rayHit))
        {
            Debug.Log("Raycast hit: " + rayHit.collider.gameObject.name);

            if (rayHit.collider.gameObject.name == "FirstPersonController")
            {
                //Måste kontrolleras om fungerar.
                if(Vector3.Distance(centerOfPlayer, centerOfHearing) < attackDistance)
                {
                    animator.SetBool("isAttacking", true);
                }
                else
                {
                    Debug.Log("Player spotted!");
                    animator.SetBool("isCrawling", false);
                    animator.SetBool("toCrawl", false);
                    animator.SetBool("isHearing", false);
                    animator.SetBool("isChasing", true);
                }
            }
        }
    }

    public void KillPlayer()
    {
        if (animator.GetBool("isAttacking"))
        {
            player.GetComponent<FirstPersonController>().enabled = false;
            deathCam.SetActive(true);
            Camera.main.gameObject.SetActive(false);
            Invoke("ResetScene", 3f);

        }
    }

    void Update()
    {
        if (hearingCollider == null)
        {
            Debug.LogError("Hearing collider not found!");
            return;
        }

        if (playerInTrigger)
        {
            CheckSight();
        }

        if (hearingSound)
        {
            animator.SetBool("isHearing", true);
        }

        KillPlayer();


        //Vector3 centerOfHearing = hearingCollider.bounds.center;
        //Vector3 centerOfPlayer = player.transform.position + Vector3.up * (player.GetComponent<Collider>().bounds.size.y / 2);

        //Vector3 directionToPlayer = centerOfPlayer - centerOfHearing;

        //Debug.DrawRay(centerOfHearing, directionToPlayer, Color.green);
    }

    public void RespondToSound(Sound sound)
    {
        print(name + " responding to sound at " + sound.pos);

        animator.SetBool("isHearing", true);

        var heardNoiseState = animator.GetBehaviour<HeardNoiceState>();
        if (heardNoiseState != null)
        {
            heardNoiseState.SetHeardSound(sound);
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }

}
