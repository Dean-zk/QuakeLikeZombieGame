using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour, IDamageable
{
    private float health = 100;

    public void Damage(float pDamage)
    {
        health -= pDamage;
        if (health <= 0 )
        { Destroy(gameObject); }
    }
}
