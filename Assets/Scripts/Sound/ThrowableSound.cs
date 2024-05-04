using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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

            //source.Play();
            
            PlaySoundToAllClientRpc();

            Sound sound = ScriptableObject.CreateInstance<Sound>();
            sound.Initialize(transform.position, soundRange);
            Sounds.MakeSound(sound);

            print($"Sound: with pos {sound.pos} and range {sound.range} created!");
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

    [ClientRpc]
    private void PlaySoundToAllClientRpc()
    {
        source.Play();
    }
}
