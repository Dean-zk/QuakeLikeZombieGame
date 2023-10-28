using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwitchWeapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform[] weapons;

    [Header("Keys")]
    [SerializeField] KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] float switchTime;

    private int selectedWeapon;
    private float timeSinceSwitch;

    private void Start()
    {
        SetWeapon();
        Select(selectedWeapon);

        timeSinceSwitch = 0f;
    }

    private void Update()
    {
        int previousWeapon = selectedWeapon;
        
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && timeSinceSwitch >= switchTime)
            {
                selectedWeapon = i;
            }
        }

        if (previousWeapon != selectedWeapon) 
        { Select(selectedWeapon); }

        timeSinceSwitch += Time.deltaTime;
    }

    void SetWeapon()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }

        if (keys == null) keys = new KeyCode[weapons.Length]; 
    }

    private void Select(int gunIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == gunIndex);
        }

        timeSinceSwitch = 0f;

        onSwitch();
    }

    void onSwitch()
    {
        print("You picked a new weapon.");
    }
} 