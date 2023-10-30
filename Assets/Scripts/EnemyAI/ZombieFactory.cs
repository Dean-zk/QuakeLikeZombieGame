using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFactory : MonoBehaviour
{
    //Spawn zombies using Objects.

    public ZombieAI zombie;
    public ZombieAI zombiePrefab;
    NavMeshAgent agent;
    public Transform player;

    const int Wave_Size = 10;


    //? ERROR: Does not assign the destination to the zombie for some reason.
    // Update is called once per frame
    /*void Start()
    {
        for (int i = 0; i < Wave_Size; i++) //voor elke int ga ik ervoor zorgen dat er een tank gespawned word.
        {
            ZombieAI myZomb = Instantiate(zombiePrefab);
        }
    }*/
}
