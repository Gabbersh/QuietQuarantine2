using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    AudioSource audioSource;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = 0f;
        agent.isStopped = true;

        //audioSource = animator.GetComponent<AudioSource>();

        //if (audioSource != null && !audioSource.isPlaying)
        //{
        //    audioSource.Play();
        //}
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //float distance = Vector3.Distance(player.transform.position, animator.transform.position);

        //if (distance > 4.5)
        //{
        //    animator.SetBool("isAttacking", false);
        //}
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = true;

        //audioSource.Stop();

        animator.SetBool("isAttacking", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isHunting", false);
        animator.SetBool("isPatrolling", true);
        animator.SetBool("isCrawling", true);
        animator.SetBool("toCrawl", true);
    }
}
