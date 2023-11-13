using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    /*These vars are static to prevent multiple instances you will get with dynamic vars.
     * Also prevents duplicating the vars in multiple scripts.
     * Static isn't bad in this context because it serves as a protection/savity layer.
     */

    public static Action shotTrigger; //Action allows to dynamically call/assign multiple functions for one var.
    public static Action reloadInput;

    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shotTrigger?.Invoke(); //Will check and avoid null reference. I use this to avoid nested code (if statement).
        }

        if (Input.GetKeyDown(reloadKey))
        {
            reloadInput?.Invoke(); //I use this to start reload functions.
        }
    }
}
