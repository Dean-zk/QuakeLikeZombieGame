using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour, IDamageable
{
    //public Transform destination;
    private NavMeshAgent agent;
    private float health = 100;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = player.transform.position;
    }
    public void Damage(float pDamage)
    {
        health -= pDamage;
        if (health <= 0 )
        { Destroy(gameObject); }
    }
}
