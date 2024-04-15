using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateMachineBehaviour
{
    private NavMeshAgent agent;

    float timer;

    //float chaseRange = 15;

    private Transform player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        animator.SetBool("isPatrolling", false);

        timer = 2;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isCrawling", true);
            animator.SetBool("toCrawl", true);
        }

        float distance = Vector3.Distance(player.transform.position, animator.transform.position);

        //if (distance < chaseRange)
        //{
        //    animator.SetBool("isChasing", true);
        //}
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = false;
    }
}
