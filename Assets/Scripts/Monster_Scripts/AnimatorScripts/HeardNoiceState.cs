using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeardNoiceState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Sound heardSound;
    private MonsterSpeed monsterSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameObject monster = animator.gameObject;
        monsterSpeed = monster.GetComponent<MonsterSpeed>();

        agent.speed = monsterSpeed.ChaseSpeed;

        agent.ResetPath();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //agent.speed = 10.56f;

        float distance = Vector3.Distance(agent.transform.position, heardSound.pos);

        if (animator.GetBool("isHearing") && heardSound != null)
        {
            GoToSoundPosition();

            if (distance < 5)
            {
                animator.SetBool("isHearing", false);
            }
        }
    }

    private void IfPlayerNear(Animator animator)
    {
        float distance = Vector3.Distance(agent.transform.position, player.position);

        if (distance < 15)
        {
            animator.SetBool("isChasing", true);
        }
    }

    private void GoToSoundPosition()
    {
        agent.SetDestination(heardSound.pos);
    }

    public void SetHeardSound(Sound sound)
    {
        heardSound = sound;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ////heardSound = null;
        agent.ResetPath();
    }
}
