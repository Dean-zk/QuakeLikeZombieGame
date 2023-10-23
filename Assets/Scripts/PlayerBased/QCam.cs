using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QCam : MonoBehaviour
{
    public Transform cam;
    public float playerViewYOffset = 0.6f; // The height at which the camera is bound to
    public float xMouseSensitivity = 30.0f;
    public float yMouseSensitivity = 30.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    public float FOV = 90.0f;
    // Start is called before the first frame update
    private void Start() //TODO: Make a seperate class for this code and run it into its respected start function [v*]
    {
        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (cam == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                cam = mainCamera.gameObject.transform;
        }

        // Put the camera inside the capsule collider
        cam.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        CamSetup();
    }

    private Transform CamSetup()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetButtonDown("Fire1"))
                Cursor.lockState = CursorLockMode.Locked;
        }

        /* Camera rotation stuff, mouse controls this shit */
        x -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity * 0.02f;
        y += Input.GetAxisRaw("Mouse X") * yMouseSensitivity * 0.02f;

        // Clamp the X rotation
        if (x < -90)
            x = -90;
        else if (x > 90)
            x = 90;

        this.transform.rotation = Quaternion.Euler(0, y, 0); // Rotates the collider
        cam.rotation = Quaternion.Euler(x, y, 0); // Rotates the camera

        return cam;
    }

    public void MoveCam()
    {
        cam.position = new Vector3(
           transform.position.x,
           transform.position.y + playerViewYOffset,
           transform.position.z);
    }
}
