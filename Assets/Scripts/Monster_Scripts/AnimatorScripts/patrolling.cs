using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class patrolling : StateMachineBehaviour
{
    NavMeshAgent agent;
    private Transform player;
    private MonsterSpeed monsterSpeed;

    float timer;
    float chaseRange = 15;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chaseRange = 15;
        timer = 0;

        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject monster = animator.gameObject;
        monsterSpeed = monster.GetComponent<MonsterSpeed>();

        agent.speed = monsterSpeed.PatrollSpeed;

        

        Vector3 randomPos = Random.insideUnitSphere * 20f;
        NavMeshHit navHit;
        NavMesh.SamplePosition(agent.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
        agent.SetDestination(navHit.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Vector3 randomPos = Random.insideUnitSphere * 20f;
            NavMeshHit navHit;
            NavMesh.SamplePosition(agent.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
            agent.SetDestination(navHit.position);
        }
        
        //agent.speed = 1.7f;

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

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
