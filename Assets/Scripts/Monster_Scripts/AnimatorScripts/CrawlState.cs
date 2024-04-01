using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrawlState : StateMachineBehaviour
{
    NavMeshAgent agent;
    private Transform player, objectToFollow;

    //float timer;
    //float chaseRange = 8;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //chaseRange = 8;
        //timer = 25;

        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = 1.7f;

        agent.ResetPath();

        Vector3 randomPos = Random.insideUnitSphere * 40f;
        NavMeshHit navHit;
        NavMesh.SamplePosition(agent.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
        agent.SetDestination(navHit.position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Vector3 randomPos = Random.insideUnitSphere * 40f;
            NavMeshHit navHit;
            NavMesh.SamplePosition(agent.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
            agent.SetDestination(navHit.position);
        }

        float distance = Vector3.Distance(player.transform.position, animator.transform.position);

        //if (distance < chaseRange)
        //{
        //    animator.SetBool("isCrawling", false);
        //    animator.SetBool("toCrawl", false);
        //    animator.SetBool("isChasing", true);
        //}

        //timer -= Time.deltaTime;
        //if (timer <= 0)
        //{
        //    animator.SetBool("isCrawling", false);
        //    animator.SetBool("toCrawl", false);
        //}
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
