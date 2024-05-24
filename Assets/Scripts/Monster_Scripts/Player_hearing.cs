using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_hearing : MonoBehaviour
{
    private AudioSource audioSource;

    private AudioClip heartbeat;

    private bool playerInTrigger;

    private void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[2];

        heartbeat = Resources.Load<AudioClip>("Audio/heartbeat");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "HearingRadius")
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HearingRadius")
        {
            playerInTrigger = false;
            audioSource.Stop();
        }
    }

    private void Update()
    {
        if (playerInTrigger)
        {
            if (!audioSource.isPlaying)
            {
                PlayClip(audioSource, heartbeat);
            }
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
