using UnityEngine;

public class Sound : ScriptableObject
{
    public Vector3 pos;
    public float range;

    public enum SoundType { Default = -1, Intresting, Danger}

    public SoundType soundType;

    public void Initialize(Vector3 _pos, float _range)
    {
        pos = _pos;
        range = _range;
    }
}