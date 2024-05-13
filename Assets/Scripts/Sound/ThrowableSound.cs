using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class ThrowableSound : NetworkBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private float soundRange;
    [SerializeField] private float initialCooldown = 2f;

    private bool canPlaySound = false;

    void OnCollisionEnter(Collision collision)
    {
        if (canPlaySound)
        {
            if (source.isPlaying)
                return;

            if (IsServer)
            {
                PlaySoundToAllClientRpc();
            }
            else
            {
                PlaySoundToServerServerRpc();
            }

            Sound sound = ScriptableObject.CreateInstance<Sound>();
            sound.Initialize(transform.position, soundRange);
            Sounds.MakeSound(sound);
        }
        else
        {
            Invoke(nameof(EnableSound), initialCooldown);
        }
    }

    private void EnableSound()
    {
        canPlaySound = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlaySoundToServerServerRpc()
    {
        PlaySoundToAllClientRpc();
    }

    [ClientRpc]
    private void PlaySoundToAllClientRpc()
    {
        source.Play();
    }
}
