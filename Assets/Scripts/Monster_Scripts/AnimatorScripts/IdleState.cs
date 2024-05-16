using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateMachineBehaviour
{
    private NavMeshAgent agent;

    float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        animator.SetBool("isPatrolling", false);

        timer = 10;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isCrawling", true);
            //animator.SetBool("toCrawl", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = false;
        agent.ResetPath();
        animator.SetBool("isIdle", false);
    }
}
