using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QMoveRB : MonoBehaviour
{
    Rigidbody rb;
    float moveSpeed = 7.0f;
    float currentSpeed;
    float moveX;
    float moveZ;


    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    void Update()
    {
        SetMovementDir(); 
    }
    
    void SetMovementDir()
    {
        moveZ = Input.GetAxisRaw("Vertical");
        moveX = Input.GetAxisRaw("Horizontal");
    }

    void Jump()
    {

    }
}
