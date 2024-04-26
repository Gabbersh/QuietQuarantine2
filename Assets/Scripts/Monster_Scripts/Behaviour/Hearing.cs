using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using Unity.Services.Lobbies.Models;
using Unity.IO.LowLevel.Unsafe;

public class Hearing : NetworkBehaviour, IHear
{
    public GameObject player;
    private Collider hearingCollider;
    private Animator animator;
    private bool playerInTrigger, hearingSound;

    private float attackDistance = 2.5f;

    public List<GameObject> players = new List<GameObject>();

    private int connectedClientsCount;
    private int lastClientsCount;

    private Vector3 centerOfHearing;

    [SerializeField] CinemachineVirtualCamera deathCam;

    private Transform respawnPoint;
    private Transform deathPoint; // Add this line to store reference to deathPoint

    private void Start()
    {
        // Find the deathPoint object in the hierarchy
        deathPoint = GameObject.Find("DeathPoint").transform;

        // If deathPoint is not found, log an error
        if (deathPoint == null)
        {
            Debug.LogError("Death Point object not found in the hierarchy!");
        }
    }

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

    // called when monster is spawned in by server
    public override void OnNetworkSpawn()
    {
        if(NetworkManager.Singleton.IsServer )
        {
            Transform hearingRadius = transform.Find("HearingRadius");

            deathCam.Priority = 0;

            //gameObject.GetComponent<Rigidbody>().position = new Vector3(145f, 1.5f, 160f);
            //transform.position = gameObject.GetComponent<Rigidbody>().position;
            
            //Debug.Log("Position set to: " + transform.position);
            //Physics.SyncTransforms();

            hearingCollider = hearingRadius.GetComponent<Collider>();

            animator = transform.GetComponent<Animator>();

            foreach (var uid in NetworkManager.Singleton.ConnectedClientsIds)
            {
                players.Add(NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GameObject());
            }

            connectedClientsCount = NetworkManager.Singleton.ConnectedClientsList.Count;
            lastClientsCount = connectedClientsCount;
        }
        //player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GameObject();
    }

    public void CheckSight()
    {
        if (hearingCollider == null)
        {
            Debug.LogError("Hearing collider not found!");
            return;
        }

        centerOfHearing = hearingCollider.bounds.center;
        Vector3 centerOfPlayer = player.transform.position;

        Vector3 directionToPlayer = centerOfPlayer - centerOfHearing;

        RaycastHit rayHit;
        if (Physics.Raycast(centerOfHearing, directionToPlayer, out rayHit))
        {
            Debug.Log("Raycast hit: " + rayHit.collider.gameObject.name);

            if (rayHit.collider.gameObject.CompareTag("Player"))
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
        if (NetworkManager.Singleton.IsServer)
        {
            if (animator.GetBool("isAttacking"))
            {
                player.GetComponent< FirstPersonController > ().enabled = false;
                deathCam.enabled = true;
                deathCam.Priority = 11;
                StartCoroutine(RespawnAfterDelay(1f)); // Activate death cam for 1 second
            }
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        deathCam.enabled = false;
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        deathCam.enabled = false;
        deathCam.Priority = 1;
        player.transform.position = deathPoint.position;
        player.GetComponent<FirstPersonController>().enabled = true;
    }

    void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            connectedClientsCount = NetworkManager.Singleton.ConnectedClientsList.Count;

            // update player list on new player join, KANSKE FINNS BÄTTRE SÄTT VEM VET!?!?
            if (connectedClientsCount != lastClientsCount)
            {
                lastClientsCount = connectedClientsCount;

                players.Clear();

                foreach (var uid in NetworkManager.Singleton.ConnectedClientsIds)
                {
                    players.Add(NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GameObject());
                }
            }


            double distance = Mathf.Infinity;

            // best most awesomest way to get nearest player to monster (not costly at all)
            foreach (var player in players)
            {
                float currentDistance = (transform.position - player.transform.position).sqrMagnitude;

                if (currentDistance < distance)
                {
                    this.player = player;
                    //Debug.Log("CURRENT CHÒSEN PLAYER TRANSFORM" + this.player.transform.position);
                    distance = currentDistance;
                }
            }

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
