using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Collider hearingCollider;
    private MonsterSpeed monsterSpeed;
    private Transform player;
    private float chaseTimer, reachDistance;
    private bool hunt;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hearingCollider = animator.transform.Find("HearingRadius").GetComponent<Collider>();

        GameObject monster = animator.gameObject;
        monsterSpeed = monster.GetComponent<MonsterSpeed>();

        hunt = false;

        agent.speed = monsterSpeed.ChaseSpeed;

        chaseTimer = 10;
        reachDistance = 10f;

        agent.ResetPath();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //agent.speed = 10.56f;

        float distance = Vector3.Distance(agent.transform.position, player.position);

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
            agent.SetDestination(player.position);

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
                NavMesh.SamplePosition(player.position + randomPos, out navHit, 10f, NavMesh.AllAreas);
                agent.SetDestination(navHit.position);


                if (hearingCollider != null)
                {
                    Vector3 centerOfHearing = hearingCollider.bounds.center;
                    Vector3 centerOfPlayer = player.position + Vector3.up * (player.GetComponent<Collider>().bounds.size.y / 2);

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
