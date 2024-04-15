using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpeed : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 7.56f;
    [SerializeField] private float patrollSpeed = 1.7f;

    public float ChaseSpeed
    {
        get { return chaseSpeed; }
    }

    public float PatrollSpeed
    {
        get { return patrollSpeed; }
    }
}
