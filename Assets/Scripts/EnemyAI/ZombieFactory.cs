using UnityEngine;
using UnityEngine.AI;

public class ZombieFactory : MonoBehaviour
{
    //Spawn zombies using Objects.

    public ZombieAI zombie;
    public ZombieAI zombiePrefab;
    NavMeshAgent agent;
    private int indexHolder; //Used for certain for loop based checks.
    public Transform player;
    public int zombieCount = 0;
    int Wave_Size = 5;
    float waveDelay = 5;
    float nextEnemyDelay = 1f;
    private void Update()
    {
        if (zombieCount == 0)
        {
            Wave_Size += 5;
            for (int i = 0; i < Wave_Size; i++)
            {
                Instantiate(zombiePrefab);
                zombieCount++;
            }
        }

        //print(zombList);
        //WaveFactory();
    }
}
