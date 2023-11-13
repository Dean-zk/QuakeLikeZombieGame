using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName ="Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Gun name:")]
    public string wpName;

    [Header("Gun stats:")]
    public float damage;
    public float range;

    [Header("Reloading vars:")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;
}
