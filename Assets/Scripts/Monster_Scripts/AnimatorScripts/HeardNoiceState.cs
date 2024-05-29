using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeardNoiceState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Sound heardSound;
    private MonsterSpeed monsterSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();

        GameObject monster = animator.gameObject;
        monsterSpeed = monster.GetComponent<MonsterSpeed>();

        agent.speed = monsterSpeed.ChaseSpeed;

        agent.ResetPath();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();

        if (animator.GetBool("isHearing") && heardSound != null)
        {
            float distance = Vector3.Distance(agent.transform.position, heardSound.pos);

            GoToSoundPosition();

            if (distance < 5)
            {
                animator.SetBool("isHearing", false);
            }
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

    public void IsStationary(Animator animator)
    {
        if (agent.velocity.magnitude < 0.1f)
        {
            animator.SetBool("isPatrolling", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.ResetPath();
    }
}
