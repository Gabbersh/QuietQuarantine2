using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class CrawlState : StateMachineBehaviour
{
    NavMeshAgent agent;
    private MonsterSpeed monsterSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();

        GameObject monster = animator.gameObject;
        monsterSpeed = monster.GetComponent<MonsterSpeed>();

        agent.speed = monsterSpeed.PatrollSpeed;

        agent.ResetPath();

        Vector3 randomPos = Random.insideUnitSphere * 40f;
        NavMeshHit navHit;
        NavMesh.SamplePosition(agent.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
        agent.SetDestination(navHit.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Vector3 randomPos = Random.insideUnitSphere * 40f;
            NavMeshHit navHit;
            NavMesh.SamplePosition(agent.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
            agent.SetDestination(navHit.position);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isPatrolling", false);
    }
}
