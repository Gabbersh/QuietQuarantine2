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
using System.Linq;
using Unity.Multiplayer.Tools.TestData.Definitions;

public class Hearing : NetworkBehaviour, IHear
{
    public GameObject player;
    public NetworkVariable<NetworkObjectReference> currentTarget = new();
    private Collider hearingCollider;
    private Animator animator;
    private bool playerInTrigger, hearingSound;

    private float attackDistance = 2.5f;

    public List<GameObject> players = new List<GameObject>();

    private int connectedClientsCount;
    private int lastClientsCount;

    private Vector3 centerOfHearing;

    [SerializeField] CinemachineVirtualCamera deathCam;

    //Slutf�r trigger och ta bort ljud inom triggern. L�gg till ljudeffekter p� monster.
    private Collider safezoneCollider;

    private Transform respawnPoint;
    private Transform deathPoint; 

    private void Start()
    {
        deathPoint = GameObject.Find("RespawnPoint").transform;

        if (deathPoint == null)
        {
            Debug.LogError("Death Point object not found in the hierarchy!");
        }

        safezoneCollider = GameObject.FindGameObjectWithTag("Safezone").GetComponent<Collider>();

        if (safezoneCollider == null)
        {
            Debug.LogError("Safezone collider not found!");
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
        if(NetworkManager.Singleton.IsServer)
        {
            Transform hearingRadius = transform.Find("HearingRadius");

            deathCam.Priority = 0;

            hearingCollider = hearingRadius.GetComponent<Collider>();

            animator = transform.GetComponent<Animator>();

            //foreach (var uid in NetworkManager.Singleton.ConnectedClientsIds)
            //{
            //    players.Add(NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GameObject());
            //}

            //connectedClientsCount = NetworkManager.Singleton.ConnectedClientsList.Count;
            //lastClientsCount = connectedClientsCount;
        }
    }



    public void CheckSight()
    {
        if (hearingCollider == null)
        {
            return;
        }

        centerOfHearing = hearingCollider.bounds.center;
        centerOfHearing.y = hearingCollider.bounds.center.y + hearingCollider.bounds.max.y / 4;
        Vector3 centerOfPlayer = player.GetComponent<Collider>().transform.position;
        centerOfPlayer.y = player.GetComponent<Collider>().bounds.max.y;

        Vector3 directionToPlayer = centerOfPlayer - centerOfHearing;

        Debug.DrawRay(centerOfHearing, directionToPlayer, Color.green);

        RaycastHit rayHit;
        if (Physics.Raycast(centerOfHearing, directionToPlayer, out rayHit))
        {
            if (rayHit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log(Vector3.Distance(centerOfPlayer, centerOfHearing));

                if (Vector3.Distance(centerOfPlayer, centerOfHearing) < attackDistance)
                {
                    animator.SetBool("isAttacking", true);

                }
                else
                {
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
                //player.GetComponent< FirstPersonController > ().enabled = false;
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
        Physics.SyncTransforms();
        player.GetComponent<FirstPersonController>().enabled = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetTargetServerRpc(NetworkObjectReference networkObject)
    {
        currentTarget.Value = networkObject;
        SendTargetToClientRpc(networkObject);
    }

    [ClientRpc]
    private void SendTargetToClientRpc(NetworkObjectReference networkObject)
    {
        currentTarget.Value = networkObject;
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player").ToList();

        if (currentTarget.Value.TryGet(out NetworkObject targetNetworkObject))
        {
            player = targetNetworkObject.gameObject;
            Debug.Log($"Set player to {targetNetworkObject.gameObject}");
        }
        else
        {
            Debug.Log($"player was null");
        }

        if (NetworkManager.Singleton.IsServer)
        {
            //connectedClientsCount = NetworkManager.Singleton.ConnectedClientsList.Count;

            //// update player list on new player join, KANSKE FINNS B�TTRE S�TT VEM VET!?!?
            //if (connectedClientsCount != lastClientsCount)
            //{
            //    lastClientsCount = connectedClientsCount;

            //    players.Clear();

            //    foreach (var uid in NetworkManager.Singleton.ConnectedClientsIds)
            //    {
            //        players.Add(NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GameObject());
            //    }
            //}

            double distance = Mathf.Infinity;

            // best most awesomest way to get nearest player to monster (not costly at all)
            foreach (var player in players)
            {
                float currentDistance = (transform.position - player.transform.position).sqrMagnitude;

                if (currentDistance < distance)
                {
                    SetTargetServerRpc(player.GetComponent<NetworkObject>());
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

            if (hearingSound && animator.GetBool("isChasing") == false)
            {
                animator.SetBool("isHearing", true);
            }

            KillPlayer();


            //Vector3 centerOfHearing = hearingCollider.bounds.center;
            //Vector3 centerOfPlayer = player.transform.position + Vector3.up * (player.GetComponent<Collider>().bounds.size.y / 2);
            //Vector3 directionToPlayer = centerOfPlayer - centerOfHearing;

        }
    }

    public void RespondToSound(Sound sound)
    {
        if (animator.GetBool("isChasing") == false)
        {
            // Check if the sound position is inside the safezone collider bounds
            if (safezoneCollider.bounds.Contains(sound.pos))
            {
                // Sound was made inside the safezone, don't respond
                return;
            }

            animator.SetBool("isHearing", true);

            var heardNoiseState = animator.GetBehaviour<HeardNoiceState>();
            if (heardNoiseState != null)
            {
                heardNoiseState.SetHeardSound(sound);
            }
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }
}
