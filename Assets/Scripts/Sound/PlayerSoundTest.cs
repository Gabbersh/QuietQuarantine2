using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.Member;

public class PlayerSoundTest : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private float soundRange;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
