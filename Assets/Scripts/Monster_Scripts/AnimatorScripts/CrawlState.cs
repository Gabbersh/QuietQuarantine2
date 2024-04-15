using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrawlState : StateMachineBehaviour
{
    NavMeshAgent agent;
    private Transform player;
    private MonsterSpeed monsterSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;


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

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
