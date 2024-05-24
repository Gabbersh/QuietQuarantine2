using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudioManager : MonoBehaviour
{
    private AudioSource audioSource1;
    private AudioSource audioSource2; 
    private Animator animator;

    private AudioClip biteClip;
    private AudioClip chasingRunningClip;
    private AudioClip fastPantClip;
    private AudioClip slowPantClip;
    private AudioClip roarClip;
    private AudioClip crawlingClip;
    private AudioClip eatingClip;

    [Header("Audio Source Volume")]
    [SerializeField] private float as1Volume;
    [SerializeField] private float as2Volume;

    void Start()
    {
        animator = GetComponent<Animator>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSource1 = audioSources[0];
        audioSource2 = audioSources[1];

        biteClip = Resources.Load<AudioClip>("Audio/bite");
        chasingRunningClip = Resources.Load<AudioClip>("Audio/chasing");
        fastPantClip = Resources.Load<AudioClip>("Audio/fast_pant");
        slowPantClip = Resources.Load<AudioClip>("Audio/slow_pant");
        roarClip = Resources.Load<AudioClip>("Audio/roar");
        crawlingClip = Resources.Load<AudioClip>("Audio/walking");
        eatingClip = Resources.Load<AudioClip>("Audio/eating");

        audioSource1.volume = as1Volume;
        audioSource2.volume = as2Volume;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        AudioLevel(stateInfo);

        if (stateInfo.IsName("IdleState"))
        {
            audioSource1.loop = true;
            audioSource2.Stop();

            PlayClip(audioSource1, slowPantClip);
        }
        else if (stateInfo.IsName("AttackState"))
        {
            audioSource2.loop = false;
            audioSource1.Stop();

            PlayClip(audioSource2, biteClip);
        }
        else if (stateInfo.IsName("ChaseState") || stateInfo.IsName("HeardNoiseState"))
        {
            audioSource1.loop = true;
            audioSource2.loop = true;

            PlayClip(audioSource1, fastPantClip);
            PlayClip(audioSource2, chasingRunningClip);
        }
        else if (stateInfo.IsName("CrawlState"))
        {
            audioSource1.loop = true;
            audioSource2.loop = true;

            PlayClip(audioSource1, slowPantClip);
            PlayClip(audioSource2, crawlingClip);
        }
        else if (stateInfo.IsName("Cooldown"))
        {
            audioSource1.loop = true;
            audioSource2.Stop();

            PlayClip(audioSource1, eatingClip);
        }
        else if (stateInfo.IsName("RoarState"))
        {
            audioSource2.loop = false;
            audioSource1.Stop();

            PlayClip(audioSource2, roarClip);
        }
    }

    private void AudioLevel(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.IsName("ChaseState") || stateInfo.IsName("HeardNoiseState"))
        {
            audioSource2.volume = 3f;
        }
        else
        {
            audioSource2.volume = as2Volume;
        }
    }

    private void PlayClip(AudioSource audioSource, AudioClip clip)
    {
        if (audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
