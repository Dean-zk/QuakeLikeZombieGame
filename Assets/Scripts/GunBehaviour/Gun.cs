using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gunData;
    float timeBetweenShot;
    [SerializeField] Transform firePoint;
    public ParticleSystem muzzleFlash;

    private void Start()
    {
        PlayerShoot.shotTrigger += Shoot; //Here I will assign the actions from the shoot script to the local function here.
        PlayerShoot.reloadInput += StartReload;
    }

    private void Update()
    {
        timeBetweenShot += Time.deltaTime; //Need to keep track of timeBetweenShots so I can check if the gun can shoot.
        Debug.DrawRay(firePoint.position, firePoint.forward);
    }

    private bool CanShoot() => !gunData.reloading && timeBetweenShot > 1f / (gunData.fireRate / 60f); //Returns true or false based on these checks here.

    private void Shoot()
    {
        if (gunData.currentAmmo <= 0)
        { return; }

        if (!CanShoot())
        { return; }

        muzzleFlash.Play(); //? This causes a visual bug.
        Debug.DrawRay(firePoint.position, firePoint.forward, Color.green); //# DEBUG Purpose only.

        if (Physics.Raycast(firePoint.position, transform.forward, out RaycastHit hitInfo, gunData.range)) //Checks if the ray has hit anything.
        {
            Debug.Log(hitInfo.transform.name);
            /*Set the interface to the Component of what I hit. This is an expensive call so I will fix this if preformance becomes a thing.*/
            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
            /*Checks if what I hit is damageable. If it is I will call the function and give damage as the param. If it doesn't it avoids a null ref*/
            damageable?.Damage(gunData.damage);
        }

        gunData.currentAmmo--;
        timeBetweenShot = 0;
    }

    /*Simply returns reloading to be false in case of a check.*/
    private void OnDisable() => gunData.reloading = false; 

    /// <summary>
    /// A function to start reloading the current gun.
    /// </summary>
    public void StartReload() 
    {
        if (!gunData.reloading && this.gameObject.activeSelf) //If i am not reloading and the current gameObject is active then start the reload coroutine.
        {
            StartCoroutine(Reload());
        }
    }

    /// <summary>
    /// Reloads the gun 
    /// </summary>
    /// <returns>A reloaded magazine.</returns>
    private IEnumerator Reload()
    {
        gunData.reloading = true;
        print("Reloading");

        yield return new WaitForSeconds(gunData.reloadTime); //Because I use ScriptObj there is no set reload time.

        gunData.currentAmmo = gunData.magSize; //gun is reloaded.
        gunData.reloading = false; //this speaks for it self
    }
}
