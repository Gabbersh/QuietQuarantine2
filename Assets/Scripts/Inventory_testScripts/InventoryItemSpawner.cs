using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Water;
    [SerializeField] private GameObject Coin;
    [SerializeField] private GameObject Medicine;
    [SerializeField, Range(0, 100)] private int maxLoops;
    void Start()
    {
        int loops = Random.Range(0, maxLoops);

        Debug.Log("Number of loops: " + loops);

        for(int i = 0; i < loops; i++)
        {
            int j = Random.Range(1, 4);
            Vector3 position = new Vector3(Random.Range(-10f, 10f), Random.Range(5f, 10f), Random.Range(-10f, 10f));

            switch (j)
            {
                case 1:
                    Instantiate(Water, position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(Coin, position, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(Medicine, position, Quaternion.identity);
                    break;
            }
        }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
