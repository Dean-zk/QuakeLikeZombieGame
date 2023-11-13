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
        SetMovementDir();
        //Accelerate(moveX, moveSpeed, jumpForce);
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

    void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        currentSpeed = Vector3.Dot(rb.velocity, wishdir);
        Vector3 wishDir = (forward * moveZ + right * moveX).normalized;
    }
}
