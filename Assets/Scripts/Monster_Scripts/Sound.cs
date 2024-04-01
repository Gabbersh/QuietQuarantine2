using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    float sound;
    Vector3 soundPosition;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            sound = 10;
            soundPosition = transform.position;
            Debug.Log(sound);
        }
        else
        {
            sound = 0;
        }
    }

    public float GetSound()
    {
        return sound;
    }

    public Vector3 GetSoundPosition()
    {
        return soundPosition;
    }
}
