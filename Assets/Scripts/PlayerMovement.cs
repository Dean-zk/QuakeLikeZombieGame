using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    //# RigidBody
    public Rigidbody rb; //# Replacing character controller with a rigidbody for more movement controll.
    RaycastHit hitInfo; // hiermee maak ik een rayCast genaamd hitinfo
    //[SerializeField] GameObject player;
    public float playerHeight;

    //# Movement stuff
    public float moveSpeed = 7.0f;         // Ground move speed
    public float runAccel = 14.0f;         // Ground accel
    public float runDeaccel = 10.0f;       // Deacceleration that occurs when running on the ground
    public float airAccel = 2.0f;          // Air accel
    public float airDeccel = 2.0f;         // Deacceleration experienced when ooposite strafing
    public float airControl = 0.3f;        // How precise air control is
    public float sideStrafeAccel = 50.0f;  // How fast acceleration occurs to get up to sideStrafeSpeed when
    public float sideStrafeSpeed = 1.0f;   // What the max speed to generate when side strafing
    public float jumpSpeed = 8.0f;         // The speed at which the character's up axis gains when hitting jump
    public bool holdJumpToBhop = true;     // When enabled allows player to just hold jump button to keep on bhopping perfectly. Beware: smells like casual.
    // Q3: players can queue the next jump just before he hits the ground
    private Vector3 moveDirNorm = Vector3.zero;
    private bool wishJump = true;
    private float moveX;
    private float moveZ;

    public float friction = 6; //! Dit is een var die frictie berekend. Kan fout gaan wegens rb gebruik in plaats van char controll.
    private float playerFriction = 0.0f; //display real time friction values
    private Vector3 rayOrigin;
    private float rayOfs = 0.1f;
    public LayerMask groundLayer; // The layer(s) you consider as ground

    private void Start()
    {
        //player = GetComponent<GameObject>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rayOrigin = transform.position - new Vector3(0f, 0.01f, 0f);
        RaycastHit hit;

        //? BUG: groundCheck not working, not in the air nor ground. 
        //TODO: Try making ground check with a raycast or set values.
        QueueJump();
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayOfs, groundLayer)) 
        {
            //GroundMove(); print(true); 
            if (hit.collider.CompareTag("ground"))
            {
                // The ray has hit an object with the specified tag, which you consider as ground
                Debug.Log("Player is grounded on an object with the tag " + "ground");
                print(true);
                // You can add additional code here for grounded behavior.
            }
        }

        else { AirMove(); print(false); }

        rb.AddForce(rb.velocity * Time.deltaTime); //add force to the rigidbody.
    }

    /*******************************************************************************************************\
   |* MOVEMENT
   \*******************************************************************************************************/

    private void SetMovementDir()
    {
        //# Is catching input (0 >> 1) but no force being applied.
        moveZ = Input.GetAxisRaw("Vertical");
        moveX = Input.GetAxisRaw("Horizontal");
    }


    void QueueJump()
    {
        if (holdJumpToBhop == true)
        {
            wishJump = Input.GetButton("Jump");
            return;
        }

        if (Input.GetButtonDown("Jump") && !wishJump) 
        { wishJump = true; }

        if (Input.GetButtonUp("Jump")) 
        { wishJump = false; }
    }

    void AirMove()
    {
        Vector3 wishDir;
        float wishVel = airAccel;
        float accel;
        Vector3 v = rb.velocity;

        SetMovementDir();

        wishDir = new Vector3(moveX, 0, moveZ);
        wishDir = transform.TransformDirection(wishDir);

        float wishSpeed = wishDir.magnitude;
        wishSpeed *= moveSpeed;

        wishDir.Normalize();
        moveDirNorm = wishDir;

        float wishSpeed2 = wishSpeed;
        if (Vector3.Dot(rb.velocity, wishDir) < 0) { accel = airDeccel; }

        else { accel = airAccel; }

        if (moveZ == 0 && moveX != 0)
        {
            if (wishSpeed > sideStrafeSpeed) { wishSpeed = sideStrafeSpeed; }
            accel = sideStrafeAccel;
        }

        Accelerate(wishDir, wishSpeed, accel);

        if (airControl > 0) { AirControl(wishDir, wishSpeed2); } //? FATAL BUG 1: X and Z vectors get assigned an infinite value.
        // !CPM: Aircontrol
    }

    private void AirControl(Vector3 pWishDir, float pWishSpeed) //? CAUSE OF FATAL BUG 1
    {
        float ySpeed;
        float speed;
        float dot;
        float acMulti;
        float tx; //! Might be causing FATAL BUG 1
        float ty;
        float tz;
        Vector3 v;

        if (Mathf.Abs(moveZ) < 0.001 || Mathf.Abs(pWishSpeed) < 0.001) 
        { return; }

        ySpeed = rb.velocity.y; //! keep in mind this can cause bugs.
        v = rb.velocity;
        v.y = 0;
        rb.velocity = v;
        
        /* Next two lines are equivalent to idTech's VectorNormalize() */
        speed = rb.velocity.magnitude;
        rb.velocity.Normalize();

        dot = Vector3.Dot(rb.velocity, pWishDir);
        acMulti = 32;
        acMulti *= airControl * dot * dot * Time.deltaTime;
        //change dir while slowing down
        if (dot > 0)
        {
            v.x = rb.velocity.x * speed + pWishDir.x * acMulti; //! This calc causes an error
            v.y = rb.velocity.y * speed + pWishDir.y * acMulti;
            v.z = rb.velocity.z * speed + pWishDir.z * acMulti;

            rb.velocity = v;

            rb.velocity.Normalize();
            moveDirNorm = rb.velocity;
        }

        tx = rb.velocity.x; ty = rb.velocity.y; tz = rb.velocity.z;
        tx *= speed; ty *= ySpeed; tz *= speed;

        rb.velocity = new Vector3(tx, ty, tz);
    }

    private void GroundMove()
    {
        Vector3 wishDir;
        Vector3 v;
        //No friction application when player is queueing next jump.

        if (!wishJump) 
        { ApplyFriction(1f); }

        else
        { ApplyFriction(0f); }

        SetMovementDir();

        wishDir = new Vector3(moveX, 0, moveZ);
        wishDir = transform.TransformDirection(wishDir);
        wishDir.Normalize();
        moveDirNorm = wishDir;

        float wishSpeed = wishDir.magnitude;
        wishSpeed *= moveSpeed;

        Accelerate(wishDir, wishSpeed, runAccel);

        //# If not working try to add a gravity reset to this.

        if (wishJump)
        {
            v = rb.velocity;
            v.y = jumpSpeed;

            rb.velocity = v;

            wishJump = false;
        }
    }

    private void ApplyFriction(float t)
    {
        Vector3 v;
        Vector3 vect3 = rb.velocity;
        float speed;
        float newSpeed;
        float control;
        float drop;

        vect3.y = 0.0f;
        speed = vect3.magnitude;
        drop = 0.0f;

        if (rb.velocity.y == 0)
        {
            control = speed < runDeaccel ? runDeaccel : speed; //TODO When refactoring change this to if for readability
            drop = control * friction;
            //! If first line of code above this does not work try calling the player material.
        }

        newSpeed = speed - drop;
        playerFriction = newSpeed;

        if (newSpeed < 0)
        { newSpeed = 0; }

        if (speed > 0)
        { newSpeed /= speed; }

        v = rb.velocity;
        v.x *= newSpeed;
        v.z *= newSpeed;

        rb.velocity = v;
    }


    private void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addSpeed;
        float accelSpeed;
        float currentSpeed;
        Vector3 v;

        currentSpeed = Vector3.Dot(rb.velocity, wishdir);
        addSpeed = wishspeed - currentSpeed;

        if (addSpeed <= 0)
        { return; }

        accelSpeed = accel * Time.deltaTime * wishspeed;

        if (accelSpeed > addSpeed)
        { accelSpeed = addSpeed; }

        v = rb.velocity;
        v.x += accelSpeed * wishdir.x;
        v.z += accelSpeed * wishdir.z;

        rb.velocity = v;
    }

    bool IsGrounded()
    {
        return rb.velocity.y == 0;
    }
}
