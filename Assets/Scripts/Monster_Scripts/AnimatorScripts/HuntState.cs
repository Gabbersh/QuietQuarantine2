using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.AI;

public class HuntState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Collider hearingCollider;
    private GameObject player;
    private MonsterSpeed monsterSpeed;
    private float chaseTimer, reachDistance;
    private bool hunt;

    // Dela upp i tv� states. Hunt och chase! M�ste kunna h�ra spelaren i hunt.

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject.GetComponent<Hearing>().player;
        player = GameObject.Find("Jeff(Clone)").GetComponent<Hearing>().player; // GHETTO FIX

        hearingCollider = animator.transform.Find("HearingRadius").GetComponent<Collider>();

        GameObject monster = animator.gameObject;
        monsterSpeed = monster.GetComponent<MonsterSpeed>();

        hunt = false;

        agent.speed = monsterSpeed.ChaseSpeed;

        chaseTimer = 10;
        reachDistance = 5f;

        agent.ResetPath();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            //Random plats runt spelaren n�r ej i sikte.
            Vector3 randomPos = Random.insideUnitSphere * 15f;
            NavMeshHit navHit;
            NavMesh.SamplePosition(player.transform.position + randomPos, out navHit, 10f, NavMesh.AllAreas);
            agent.SetDestination(navHit.position);


            if (hearingCollider != null)
            {
                Vector3 centerOfHearing = hearingCollider.bounds.center;
                Vector3 centerOfPlayer = player.transform.position + Vector3.up * (player.GetComponent<Collider>().bounds.size.y / 2);

                Vector3 directionToPlayer = centerOfPlayer - centerOfHearing;

                RaycastHit rayHit;
                if (Physics.Raycast(centerOfHearing, directionToPlayer, out rayHit))
                {
                    Debug.Log("Raycast hit: " + rayHit.collider.gameObject.name);

                    if (rayHit.collider.gameObject.name == "Player")
                    {
                        hunt = false;
                    }
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}