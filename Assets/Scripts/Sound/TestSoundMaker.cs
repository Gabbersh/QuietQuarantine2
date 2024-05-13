using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundMaker : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private float soundRange;

    private void OnMouseDown()
    {
        if (source.isPlaying)
            return;
       
        source.Play();
        Sound sound = ScriptableObject.CreateInstance<Sound>();
        sound.Initialize(transform.position, soundRange);

        Sounds.MakeSound(sound);
    }
}
