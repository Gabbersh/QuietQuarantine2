using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    public readonly Vector3 pos;
    public readonly float range;
    public Sound(Vector3 _pos, float _range)
    {
        pos = _pos;
        range = _range;
    }
}
