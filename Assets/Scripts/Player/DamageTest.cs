using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{

    private float damage;

    private void OnTriggerEnter(Collider other)
    {
        damage = 50;
        if (other.CompareTag("Player"))
            FirstPersonController.OnTakeDamage(damage);
    }
}
