using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QPlasmaGun : MonoBehaviour
{
    [SerializeField] ZombieAI zomb;
    Camera cam;
    RaycastHit hit;
    const float range = 1000000f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip firing;
    [SerializeField] AudioSource source;

    int damage = 2;
    const int magSize = 200;
    int bulletsInMag = magSize;
    void Start()
    {
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
                    print("You hit a zombie");
                    zomb.zombHealth = zomb.zombHealth - damage;
                break;
            }
        }
    }
}
