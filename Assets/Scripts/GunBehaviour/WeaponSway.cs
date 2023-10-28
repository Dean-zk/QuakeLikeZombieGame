using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway settings:")] //The sway intensity. Can be ReWritten so its sway can be changed based on the gun.
    private float swaySmoothing = 8; //The smoothing of the sway. 
    private float swayMulti = 4; //The multiplier for the sway.

    /// <summary>
    /// Here I add the sway to the gun.
    /// </summary>
    private void Update()
    {
        float x = Input.GetAxisRaw("Mouse X") * swayMulti;
        float y = Input.GetAxisRaw("Mouse Y") * swayMulti;

        Quaternion rotX = Quaternion.AngleAxis(-y, Vector3.right); //Will rotate around the -y and Vector3 right axis.
        Quaternion rotY = Quaternion.AngleAxis(x, Vector3.up);

        Quaternion targetRot = rotX * rotY; //the target rotation will be based around the x and y rotation.

        /*Spherical interpolation calculation based on the localRotation of the targetRot the smoothing * Time.deltaTime*/
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, swaySmoothing * Time.deltaTime);
    }
}
