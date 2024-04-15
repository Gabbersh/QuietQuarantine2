using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    GameObject player;
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().transform;
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().gameObject.GetComponent<Hearing>().player;
        player = GameObject.Find("Jeff(Clone)").GetComponent<Hearing>().player; // GHETTO FIX

        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = 0f;
        agent.isStopped = true;

        //player.transform.LookAt(agent.transform.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.transform.LookAt(player);

        float distance = Vector3.Distance(player.transform.position, animator.transform.position);

        if (distance > 4.5)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = false;
    }
}
