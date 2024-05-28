using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MonsterAudioManager : NetworkBehaviour
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
        if (!IsServer) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        AudioLevel(stateInfo);

        if (stateInfo.IsName("IdleState"))
        {
            audioSource1.loop = true;
            audioSource2.Stop();
            PlaySoundServerRpc(audioSource1.loop, slowPantClip.name, false);
        }
        else if (stateInfo.IsName("AttackState"))
        {
            audioSource2.loop = false;
            audioSource1.Stop();
            PlaySoundServerRpc(audioSource2.loop, biteClip.name, true);
        }
        else if (stateInfo.IsName("ChaseState") || stateInfo.IsName("HeardNoiseState"))
        {
            audioSource1.loop = true;
            audioSource2.loop = true;
            PlaySoundServerRpc(audioSource1.loop, fastPantClip.name, false);
            PlaySoundServerRpc(audioSource2.loop, chasingRunningClip.name, false);
        }
        else if (stateInfo.IsName("CrawlState"))
        {
            audioSource1.loop = true;
            audioSource2.loop = true;
            PlaySoundServerRpc(audioSource1.loop, slowPantClip.name, false);
            PlaySoundServerRpc(audioSource2.loop, crawlingClip.name, false);
        }
        else if (stateInfo.IsName("Cooldown"))
        {
            audioSource1.loop = true;
            audioSource2.Stop();
            PlaySoundServerRpc(audioSource1.loop, eatingClip.name, false);
        }
        else if (stateInfo.IsName("RoarState"))
        {
            audioSource2.loop = false;
            audioSource1.Stop();
            PlaySoundServerRpc(audioSource2.loop, roarClip.name, true);
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

    [ServerRpc(RequireOwnership = false)]
    private void PlaySoundServerRpc(bool loop, string clipName, bool stopOtherSource)
    {
        PlaySoundToAllClientRpc(loop, clipName, stopOtherSource);
    }

    [ClientRpc]
    private void PlaySoundToAllClientRpc(bool loop, string clipName, bool stopOtherSource)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/{clipName}");
        if (stopOtherSource)
        {
            audioSource1.Stop();
        }

        AudioSource audioSource = stopOtherSource ? audioSource2 : audioSource1;
        if (audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }
}