using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    private Transform player, objectToFollow;
    float chaseTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        objectToFollow = FindObjectOfType<SplineObject>().transform;

        agent.speed = 7.56f;
        chaseTimer = 10;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        agent.speed = 10.56f;

        var dir = (agent.steeringTarget - agent.transform.position).normalized;
        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (chaseTimer > 0 && distance > 15)
        {
            chaseTimer -= Time.deltaTime;
        }

        if (distance < 15)
        {
            chaseTimer = 10;
        }

        if (distance > 15 && chaseTimer <= 0)
        {
            chaseTimer = 10;
            animator.SetBool("isChasing", false);
        }

        if (distance < 4.5f)
        {
            if (Vector3.Distance(agent.transform.position, agent.destination) < agent.radius)
            {
                agent.ResetPath();
            }
            animator.SetBool("isAttacking", true);
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
