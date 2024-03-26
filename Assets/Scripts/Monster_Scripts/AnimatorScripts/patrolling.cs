using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrolling : StateMachineBehaviour
{
    NavMeshAgent agent;
    private Transform player, objectToFollow;

    float timer;
    float chaseRange = 15;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chaseRange = 15;
        timer = 0;

        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objectToFollow = FindObjectOfType<SplineObject>().transform;

        agent.speed = 1.58f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(objectToFollow.position);
        agent.speed = 1.58f;

        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (distance < chaseRange)
        {
            animator.SetBool("isChasing", true);
        }

        timer += Time.deltaTime;
        if (timer > 14)
        {
            animator.SetBool("isCrawling", true);
            animator.SetBool("toCrawl", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
