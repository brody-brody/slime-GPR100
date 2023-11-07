using UnityEngine;

public class ChrisMovement : MonoBehaviour
{
    public enum Direction
    {
        Left = -1,
        Right = 1
    }
    // For team members who don't know what SerializeField does:
    // This is a PropertyAttribute. The [SerializeField] attribute makes it so private fields can be accessed and changed in the inspector while still being inaccessible to other scripts.
    [SerializeField]
    private float baseSpeed = 4.0f;

    // Jump fields
    [SerializeField] private float 
        jumpForce = 5.0f, 
        gravityStrength = -9.81f;

    // Collision
    [SerializeField] private float surfaceCheckOffset = 0.5f; // Assuming that the player is 1x1 units, this will be used to check nearby surfaces along each side of the player
    [SerializeField] private LayerMask surfaceLayer;

    private Vector2 lastNormal = Vector2.up; // normal vector of the last surface
    private float xInput; // stores the players input
    private bool isGrounded = true;

    private Rigidbody2D rb;

    private Direction direction;
    private bool 
        leaveGroundFlag = false, 
        jumpFlag;

    Vector2 vel;

    private void Awake()
    {
        // Store the reference to the players Rigidbody2D inside the rb variable. Assume this script is attached to the player's GameObject which holds the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // store horizontal input into the xInput variable
        xInput = Input.GetAxisRaw("Horizontal");

        if (jumpFlag)
            return;

        // Set the direction enum (this can be used in calculations, since left is -1 and right is +1)
        if (xInput > 0) direction = Direction.Right;
        else if (xInput < 0) direction = Direction.Left;

        // Get the vector perpendicular to the last normal vector and multiply it by the players input and the base speed
        Vector2 up = lastNormal;
        Vector2 forward = -Vector2.Perpendicular(lastNormal) * (int)direction;

        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 dir = -up * 0.6f;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpFlag = true;
            if (lastNormal != Vector2.down)
            {
                rb.AddForce(up * gravityStrength, ForceMode2D.Impulse);
            }
        }

        // This raycast essentially checks both edges of the player's bottom face (respective to the normal of the last touched surface)
        if (!Physics2D.Raycast(playerPos + (-up * 0.5f + forward * 0.5f), dir, 0.55f, surfaceLayer) && !Physics2D.Raycast(playerPos + (-up * 0.5f + forward * -0.5f), dir, 0.55f, surfaceLayer) && !leaveGroundFlag)
        {
            // This could be whats causing the problem. This line is attempting to "wrap" the normal to match the next expected surface. This wouldn't work on weird angles, so be careful with level design
            lastNormal = new Vector2(Vector2.Perpendicular(-lastNormal).x * (int)direction, Vector2.Perpendicular(-lastNormal).y * (int)direction);
            Debug.DrawRay(transform.position, lastNormal, Color.yellow, 2.0f);

            // set a flag so the normal doesn't get over
            leaveGroundFlag = true;
            Invoke(nameof(ResetGroundFlag), 0.1f);
        }

            // Set the velocity variable
            vel = new(Vector2.Perpendicular(lastNormal).x * -xInput * baseSpeed, Vector2.Perpendicular(lastNormal).y * -xInput * baseSpeed);
    }

    // Physics forces and velocity changes should take place in the FixedUpdate() loop
    private void FixedUpdate()
    {
        if (jumpFlag)
        {
            rb.AddForce(Vector2.down * gravityStrength, ForceMode2D.Force);
        }

        if (jumpFlag)
            return;
        // set the rigidbody velocity   
        rb.velocity = vel;
    }

    private void ResetGroundFlag()
    {
        leaveGroundFlag = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (leaveGroundFlag) return;

        if (jumpFlag)
            return;

        // get the last normal
        lastNormal = collision.contacts[0].normal;

        // You can see this debug ray by enabling Gizmos in Play Mode
        Debug.DrawRay(transform.position, lastNormal * 2.0f, Color.red);
        Debug.DrawRay(transform.position, Vector2.Perpendicular(lastNormal) * 2.0f * -xInput, Color.green);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpFlag = false;
    }
}