using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour, IDamageable
{
    [SerializeField] Transform dest;
    private NavMeshAgent agent;
    private float health = 100;

    private void Start()
    {
       agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = dest.position;
    }
    public void Damage(float pDamage)
    {
        health -= pDamage;
        if (health <= 0 )
        { Destroy(gameObject); }
    }
}
