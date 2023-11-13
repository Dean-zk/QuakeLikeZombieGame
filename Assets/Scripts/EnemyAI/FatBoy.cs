using UnityEngine;
using UnityEngine.AI;

public class FatBoy : MonoBehaviour, IDamageable
{
    //Added FatBoy logic in different script since i don't have time to rewrite the system.
    public FatBoyFactory factory;
    private NavMeshAgent agent;
    private float health = 150;
    public int deadFatBoy;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        print(factory.deadF);
        agent.destination = player.transform.position;
    }
    public void Damage(float pDamage)
    {
        health -= pDamage;
        if (health <= 0)
        { Destroy(gameObject); factory.deadF += 1; } //for eventual wave system.
    }
}
