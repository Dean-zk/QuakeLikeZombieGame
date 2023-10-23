using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QPlasmaGun : MonoBehaviour
{
    ZombieAI zomb;
    Camera cam;
    RaycastHit hit;
    const float range = 1000000f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip firing;
    [SerializeField] AudioSource source;
    bool isPlaying = false;

    int damage = 3;
    const int magSize = 200;
    int bulletsInMag = magSize;
    void Start()
    {
        zomb = GetComponent<ZombieAI>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton("Fire1")) //als je op fire1 drukt dan word de shooting class geroepen.
        {
            muzzleFlash.Play();
            Shooting();
        }
    }

    void Shooting()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            print(hit);
            switch (hit.collider.tag)
            {
                case "ground":
                    print("you hit the ground");
                break;

                case "Player":
                    print("You hit a player!?");
                break;

                case "Enemy":
                    zomb.zombHealth -= damage;
                break;
            }
        }
    }
}
