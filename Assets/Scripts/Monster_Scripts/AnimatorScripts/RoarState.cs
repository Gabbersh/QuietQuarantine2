using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class RoarState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AudioSource audioSource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = true;

        //audioSource = animator.GetComponent<AudioSource>();

        //if (audioSource != null && !audioSource.isPlaying)
        //{
        //    audioSource.Play();
        //}
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isCrawling", true);
        animator.SetBool("toCrawl", true);

        agent.isStopped = false;

        //audioSource.Stop();
    }
}
