using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawns : MonoBehaviour
{
    public List<Transform> EnemySpawns;

    void Awake()
    {
        EnemySpawns = gameObject.GetComponentsInChildren<Transform>().Skip(1).ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
