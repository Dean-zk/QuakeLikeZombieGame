using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class QMoveRB : MonoBehaviour
{
    public LayerMask isGround;
    bool onGround;
    Rigidbody rb;
    float playerHeight = 2;
    float moveSpeed = 7.0f;
    float jumpForce = 3f;
    float currentSpeed;
    float moveX;
    float moveZ;
    Vector3 wishDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
    
    void SetMovementDir()
    {
        moveZ = Input.GetAxisRaw("Vertical");
        moveX = Input.GetAxisRaw("Horizontal");
    }

    /*MOVEMENT LOGIC:
     *Save the direction you are going to in variables.
     *when going 1 direction add force to the player.
     *when going into both directions. slowly move the player more to the wishdir
     */
}
