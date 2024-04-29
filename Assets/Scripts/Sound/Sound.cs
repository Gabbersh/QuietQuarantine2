using UnityEngine;

public class Sound : ScriptableObject
{
    public Vector3 pos;
    public float range;

    public enum SoundType { Default = -1, Interesting, Danger}

    public SoundType soundType;

    public void Initialize(Vector3 _pos, float _range)
    {
        pos = new Vector3(_pos.x, 0f, _pos.z);
        range = _range;
    }
}