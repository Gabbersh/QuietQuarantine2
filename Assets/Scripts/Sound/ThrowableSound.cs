using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableSound : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private float soundRange;
    [SerializeField] private float initialCooldown = 2f;

    private bool canPlaySound = false;

    private void Start()
    {
        Invoke(nameof(EnableSound), initialCooldown);
    }

    private void EnableSound()
    {
        canPlaySound = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (canPlaySound)
        {
            if (source.isPlaying)
                return;

            source.Play();
            Sound sound = ScriptableObject.CreateInstance<Sound>();
            sound.Initialize(transform.position, soundRange);
            Sounds.MakeSound(sound);

            print($"Sound: with pos {sound.pos} and range {sound.range} created!");
        }
    }
}
