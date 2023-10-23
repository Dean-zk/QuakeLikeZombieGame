using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public int zombHealth = 100;
    float zombSpeed;
    AudioClip zombSound;
    GameObject zombie;
    void Start()
    {
        zombie = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        print(zombHealth);
        if (zombHealth >= 0)
        {
            Destroy(zombie);
        }
    }
}
