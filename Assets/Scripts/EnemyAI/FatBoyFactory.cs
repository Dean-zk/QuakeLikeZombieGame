using UnityEngine;
using UnityEngine.AI;

public class FatBoyFactory : MonoBehaviour
{
    //Spawn zombies using Objects.

    public FatBoy fatBoy;
    public FatBoy fatBoyPrefab;
    NavMeshAgent agent;
    private int indexHolder; //Used for certain for loop based checks.
    public Transform player;
    const int Wave_Size = 10;
    public int deadF;


    //? ERROR: Does not assign the destination to the zombie for some reason.
    // Update is called once per frame
    void Start()
    {
        WaveFactory();
    }

    private void Update()
    {
        if (deadF == indexHolder)
        {
            WaveFactory();
            deadF = 0;
        }
    }

    void WaveFactory()
    {
        for (int i = 0; i < Wave_Size; i++) //voor elke int ga ik ervoor zorgen dat er een tank gespawned word.
        {
            FatBoy myFatBoy = Instantiate(fatBoyPrefab);
            indexHolder += i;
        }
    }
}
