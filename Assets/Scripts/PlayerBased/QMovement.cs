using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains the command the user wishes upon the character

public class QMovement : MonoBehaviour
{
    /*
    **KEEP IN MIND: This is a PORT of the Quake 3 movement. It is NOT FROM SCRATCH. I used the quake 3 source code to help me port this code.**
    **I tried porting with a rigidBody but this was pretty tough since you had to apply all kinds of none needed Physics**
     */
    QCam qCam;
    //^Scripts
    public float gravity = 20.0f;

    public float friction = 6; //Friction for the character controller.

    /* Movement stuff */
    public float moveSpeed = 7.0f;         // Ground move speed
    public float runAccel = 14.0f;         // Ground accel
    public float runDeaccel = 10.0f;       // Deacceleration that occurs when running on the ground
    public float airAccel = 2.0f;          // Air accel
    public float airDeccel = 2.0f;         // Deacceleration experienced when ooposite strafing
    public float airControl = 0.3f;        // How precise air control is
    public float strafeAccel = 50.0f;      // How fast acceleration occurs to get up to sideStrafeSpeed when
    public float strafeSpeed = 1.0f;       // What the max speed to generate when side strafing
    public float jumpSpeed = 8.0f;         // The speed at which the character's up axis gains when hitting jump
    private float dashAccel = 4.0f;
    public bool holdJumpToBhop = true;     // When enabled allows player to just hold jump button to keep on bhopping perfectly. Beware: smells like casual.

    public CharacterController body;
    /*The CharacterController where the physics will be applied on*/

    private Vector3 moveDirNorm; //Empty normal move direction
    public Vector3 playerVelocity; // gets a vector(0,0,0);
    public float playerTopVelocity = 0.0f; //Checks the peak velocity throught the game

    // Q3: players can queue the next jump just before he hits the ground
    private bool wishJump = true;

    // Used to display real time fricton values
    private float playerFriction = 0.0f;

    /*If you want to maintain a more legacy vector aprouch you can use a struct.
     However this is not advised since it doesn't add value.*/

    float moveZ; //Move forward and backwards using W and S.
    float moveX; //Move Right and left using A and D.

    public void Awake() //assign values here instead of using the inspector.
    {
        qCam = GetComponent<QCam>();
        body = GetComponent<CharacterController>();
    }

    private void Update()
    {
        QueueJump();

        GroundCheck();

        /*Add force based on the playerVelocity and a time multiplication*/
        body.Move(playerVelocity * Time.deltaTime);

        /*Calling camera to prevent a 1 frame following delay*/
        qCam.MoveCam();
    }

    /*******************************************************************************************************\
   |* MOVEMENT
   \*******************************************************************************************************/

    /// <summary>
    /// Set the movement direction based on the players input.
    /// </summary>
    private void SetMovementDir() //Using Unity new input system for easy use.
    {
        moveZ = Input.GetAxisRaw("Vertical");
        moveX = Input.GetAxisRaw("Horizontal");
    }

    /**
     * Queues the next jump just like in Q3
     */
    /// <summary>
    /// Checks if but not limited for a jump being planned after OnGround = true
    /// </summary>
    private void QueueJump()
    {
        if (holdJumpToBhop) //early returns with a jump in the next action buffer.
        {
            wishJump = Input.GetKey(KeyCode.Space);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !wishJump)
        { wishJump = true; } //if pressed down but wishJump != jump then return true to jump
        if (Input.GetKeyUp(KeyCode.Space))
        { wishJump = false; }
    }

    /// <summary>
    /// Movement behaviour for the player when in the air.
    /// </summary>
    private void AirMove()
    {
        Vector3 wishdir; //Direction the player wants to move to
        float wishvel = airAccel; //assign vars for this function only
        float accel;

        SetMovementDir(); //Set the direction the player wants to move to.

        /*Set wish direction to the desired direction*/
        wishdir = new Vector3(moveX, 0, moveZ);
        /*Transform the wishdir from local space to world space.*/
        wishdir = transform.TransformDirection(wishdir);

        /*Save the distance between the origin vector and the end vector*/
        float wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed; //multiply for part of the accel glitch

        //because Quake movement isnt normalized you need to do it here
        wishdir.Normalize(); 
        moveDirNorm = wishdir; 

        // CPM: Aircontrol
        float wishspeed2 = wishspeed; //Save wishspeed in a new var
        if (Vector3.Dot(playerVelocity, wishdir) < 0)
            accel = airDeccel;
        else
            accel = airAccel;
        // If the player is ONLY strafing left or right
        if (moveZ == 0 && moveX != 0)
        {
            if (wishspeed > strafeSpeed)
                wishspeed = strafeSpeed;
            accel = strafeAccel;
        }

        Accelerate(wishdir, wishspeed, accel); //call the accelerate function
        if (airControl > 0)
            AirControl(wishdir, wishspeed2); //give these vars to AirControl.
        // !CPM: Aircontrol

        // Apply gravity
        playerVelocity.y -= gravity * Time.deltaTime;
    }

    
    /// <summary>
    ///  Air control occurs when the player is in the air, it allows
    /// players to move side to side much faster rather than being
    /// 'sluggish' when it comes to cornering.
    /// </summary>
    private void AirControl(Vector3 wishdir, float wishspeed)
    {
        float yspeed; //Changed ZSpeed to YSpeed Because Z in Quake is Y in Unity.
        float speed;
        float dot;     //Will hold the Dot value of a Vector
        float acMulti = 32; //Acceleration Multiplier

        // Can't control movement if not moving forward or backward so early return.
        if (Mathf.Abs(moveZ) < 0.001 || Mathf.Abs(wishspeed) < 0.001)
        { return; }
        yspeed = playerVelocity.y; //yspeed will save the y velocity
        /*Clear the y velocity to prevent calculation bugs*/
        playerVelocity.y = 0; 
        /* Next two lines are equivalent to idTech's VectorNormalize() */
        speed = playerVelocity.magnitude; //saving magnitude to use later here.
        playerVelocity.Normalize();

        /*find out where playerVelovity and wishdir their points
         are facing relative to each other to decide actual destination.*/
        dot = Vector3.Dot(playerVelocity, wishdir);
        //in the air use dot*dot so you can use the strafe bug.
        acMulti *= airControl * dot * dot * Time.deltaTime;

        // Change direction while slowing down
        if (dot > 0)
        {
            /*Putting the values in the velocity and then normalize the results.*/
            playerVelocity.x = playerVelocity.x * speed + wishdir.x * acMulti;
            playerVelocity.y = playerVelocity.y * speed + wishdir.y * acMulti;
            playerVelocity.z = playerVelocity.z * speed + wishdir.z * acMulti;

            playerVelocity.Normalize();
            moveDirNorm = playerVelocity; //Save the Velocity in the Normal dir.
        }

        playerVelocity.x *= speed; //Put saved values back in the velocity.
        playerVelocity.y = yspeed;
        playerVelocity.z *= speed;
    }

   ///<summary>
   /// Called every frame when the engine detects that the player is on the ground
   ///</summary> 
    private void GroundMove()
    {
        Vector3 wishdir; //The direction the player wants to move to.

        // Do not apply friction if the player is queueing up the next jump
        if (!wishJump)
            ApplyFriction(1.0f);
        else
            ApplyFriction(0);

        SetMovementDir();

        wishdir = new Vector3(moveX, 0, moveZ);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();
        moveDirNorm = wishdir;

        var wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        Accelerate(wishdir, wishspeed, runAccel); //call the accelerate class.

        // Reset the gravity velocity
        playerVelocity.y = -gravity * Time.deltaTime;

        if (wishJump)
        {
            playerVelocity.y = jumpSpeed; //set the speed for in the air.
            wishJump = false;
        }
    }

     ///<summary>
     /// Applies friction to the player, called in both the air and on the ground
     ///</summary>

    private void ApplyFriction(float t)
    {
        Vector3 vec = playerVelocity; // Equivalent to: VectorCopy();
        float speed;
        float newspeed;
        float control;
        float drop;

        vec.y = 0.0f;
        speed = vec.magnitude;
        drop = 0.0f;

        /* Only if the player is on the ground then apply friction */
        if (body.isGrounded)
        {
            control = speed < runDeaccel ? runDeaccel : speed; //Checks if speed is lower then runDeaccel or runDeaccel == speed;
            drop = control * friction * Time.deltaTime * t;
        }

        newspeed = speed - drop;
        playerFriction = newspeed;
        if (newspeed < 0)
            newspeed = 0;
        if (speed > 0)
            newspeed /= speed;

        playerVelocity.x *= newspeed;
        playerVelocity.z *= newspeed;
    }

    private void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addspeed;
        float accelspeed;
        float curspeed;

        curspeed = Vector3.Dot(playerVelocity, wishdir);
        addspeed = wishspeed - curspeed;
        if (addspeed <= 0)
        { return; }
        accelspeed = accel * Time.deltaTime * wishspeed;
        if (accelspeed > addspeed)
        { accelspeed = addspeed; }

        playerVelocity.x += accelspeed * wishdir.x;
        playerVelocity.z += accelspeed * wishdir.z;
    }

    /*private void Dash(Vector3 wishdir)
    {
        if (!Input.GetKeyDown(KeyCode.LeftControl))
        {
            return; 
        }

        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Vector3 v;
            v = playerVelocity;

            playerVelocity.x += dashAccel * wishdir.x;
            playerVelocity.z += dashAccel * wishdir.z;
        }
        /*If input == ctrl then ->
         playerVelocity *= dashAccel;
        
         Body.Move(playerVelocity * Time.timedelta);
    }*/

    /// <summary>
    /// Simply checks if the player is on the ground and what function to acces.
    /// </summary>
    bool GroundCheck()
    {
        if (body.isGrounded)
        { GroundMove(); print("Is on ground"); }
        else if (!body.isGrounded)
        { AirMove(); print("Is in air"); }

        return body.isGrounded;
    }
}
