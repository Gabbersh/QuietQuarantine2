using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundMaker : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private float soundRange = 45f;

    private void OnMouseDown()
    {
        if (source.isPlaying)
            return;
       
        source.Play();
        var sound = new Sound(transform.position, soundRange);

        Sounds.MakeSound(sound);
    }
}
