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

    // Dela upp i två states. Hunt och chase! Måste kunna höra spelaren i hunt.

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();

        player = animator.gameObject.GetComponent<Hearing>().player;

        hearingCollider = animator.transform.Find("HearingRadius").GetComponent<Collider>();

        GameObject monster = animator.gameObject;
        monsterSpeed = monster.GetComponent<MonsterSpeed>();

        agent.speed = monsterSpeed.ChaseSpeed;

        reachDistance = 12f;

        agent.ResetPath();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.gameObject.GetComponent<Hearing>().player;

        float distance = Vector3.Distance(agent.transform.position, player.transform.position);

        agent.SetDestination(player.transform.position);

        if (distance > reachDistance)
        {
            animator.SetBool("isHunting", true);
            animator.SetBool("isChasing", false);
        }

        IsStationary(animator);
    }

    public void IsStationary(Animator animator)
    {
        if (agent.velocity.magnitude < 0.1f)
        {
            animator.SetBool("isPatrolling", true);

            animator.SetBool("isChasing", false);
            animator.SetBool("isHunting", false);
            animator.SetBool("isHearing", false);
            agent.ResetPath();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
