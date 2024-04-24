using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Collider hearingCollider;
    private GameObject player;
    private MonsterSpeed monsterSpeed;
    private float chaseTimer, reachDistance;
    private bool hunt;

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
        reachDistance = 14f;

        agent.ResetPath();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("Jeff(Clone)").GetComponent<Hearing>().player; // GHETTO FIX

        agent.speed = 10.56f;

        float distance = Vector3.Distance(agent.transform.position, player.transform.position);

        if (chaseTimer > 0 && distance > reachDistance)
        {
            chaseTimer -= Time.deltaTime;
        }

        if (distance > 15 && chaseTimer <= 0)
        {
            animator.SetBool("isChasing", false);
        }
        else if (distance < reachDistance)
        {
            chaseTimer = 10;
            hunt = false;
        }

        if (!hunt)
        {
            agent.SetDestination(player.transform.position);

            if (distance > reachDistance)
            {
                hunt = true;
            }
        }
        else
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                //Random plats runt spelaren när ej i sikte.
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

        //if (distance < 4.5f)
        //{
        //    if (Vector3.Distance(agent.transform.position, agent.destination) < agent.radius)
        //    {
        //        agent.ResetPath();
        //    }
        //    animator.SetBool("isAttacking", true);
        //}
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
