using UnityEngine;

public class UpdatedPlayerMovement : MonoBehaviour
{
    public enum Direction
    {
        Left = -1,
        Right = 1
    }
    // For team members who don't know what SerializeField does:
    // This is a PropertyAttribute. The [SerializeField] attribute makes it so private fields can be accessed and changed in the inspector while still being inaccessible to other scripts.
    [Header("Movement Variables")]
    [SerializeField] private float baseSpeed = 4.0f;

    // Jump fields
    [Header("Jump Variables")]
    [SerializeField] private float jumpUpForce = 5.0f;
    [SerializeField] private float jumpSideForce = 3.0f;
    [SerializeField] private AnimationCurve gravityOverTime;
    [SerializeField] private float gravityTimeMultiplier = 2.0f;

    [Header("Surface Variables")]
    [SerializeField] private float surfaceCheckOffset = 0.5f;
    [SerializeField] private float surfaceCheckRayLength = 0.15f;
    [SerializeField] private LayerMask surfaceLayer;

    // nonstick layers will trigger gravity immedietly
    [SerializeField] private LayerMask nonStickLayer;

    // input and directions
    private Vector2 lastNormal = Vector2.up; // normal vector of the last surface
    private float xInput; // stores the players input
    private Direction direction;
    private Vector2 vel;

    // references
    private Rigidbody2D rb;

    // flags
    private bool canResetJumpFlag;
    private bool jumpFlag;
    private bool isGrounded;

    private float airTime;

    private void Awake()
    {
        // Store the reference to the players Rigidbody2D inside the rb variable. Assume this script is attached to the player's GameObject which holds the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // store horizontal input into the xInput variable
        xInput = Input.GetAxisRaw("Horizontal");

        // Set the direction enum (this can be used in calculations, since left is -1 and right is +1)
        if (xInput > 0) direction = Direction.Right;
        else if (xInput < 0) direction = Direction.Left;

        // Get the vector perpendicular to the last normal vector and multiply it by the players input and the base speed
        Vector2 up = lastNormal;
        Vector2 forward = -Vector2.Perpendicular(lastNormal) * (int)direction;
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 dir = -up * 0.6f;

        // This bool checks to see if the player is touching something
        isGrounded = Physics2D.OverlapArea(new Vector2(playerPos.x - surfaceCheckOffset, playerPos.y - surfaceCheckOffset), new Vector2(playerPos.x + surfaceCheckOffset, playerPos.y + surfaceCheckOffset), surfaceLayer);
        bool isOnNonStick = Physics2D.OverlapArea(new Vector2(playerPos.x - surfaceCheckOffset, playerPos.y - surfaceCheckOffset), new Vector2(playerPos.x + surfaceCheckOffset, playerPos.y + surfaceCheckOffset), nonStickLayer);

        if (isOnNonStick) {
            jumpFlag = true;
            Invoke(nameof(ResetJumpFlag), 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpFlag = true;
            Invoke(nameof(ResetJumpFlag), 0.1f);
            if (lastNormal != Vector2.down)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(lastNormal.x * jumpSideForce, jumpUpForce), ForceMode2D.Impulse);
            }
        }

        if (isGrounded && canResetJumpFlag) {
            jumpFlag = false;
            canResetJumpFlag = false;
        }


        if (jumpFlag) return;

        // This raycast essentially checks both edges of the player's bottom face (respective to the normal of the last touched surface)
        if (!Physics2D.Raycast(playerPos + (-up * 0.5f + forward * 0.5f), dir, surfaceCheckRayLength, surfaceLayer) && !Physics2D.Raycast(playerPos + (-up * 0.5f + forward * -0.5f), dir, surfaceCheckRayLength, surfaceLayer) && xInput != 0)
        {
            // Create a new ray starting from the corner of player and try to find the next surface
            Ray raycast = new Ray(playerPos + (-up * 0.6f + forward * -0.45f), new Vector2(-Vector2.Perpendicular(-lastNormal).x * (int)direction, -Vector2.Perpendicular(-lastNormal).y * (int)direction));
            RaycastHit2D rayHit = Physics2D.Raycast(raycast.origin, raycast.direction, 0.5f, surfaceLayer);

            // snap to the found surface
            if (rayHit.collider != null)
            {
                transform.position = rayHit.point + (-up * -0.5f + forward * 0.5f);

                // set the new normal to use in velocity calculations to wrap around the surface
                lastNormal = new Vector2(Vector2.Perpendicular(-lastNormal).x * (int)direction, Vector2.Perpendicular(-lastNormal).y * (int)direction);
            }
        }

        // Set the velocity variable
        vel = new(Vector2.Perpendicular(lastNormal).x * -xInput * baseSpeed, Vector2.Perpendicular(lastNormal).y * -xInput * baseSpeed);
    }

    // Physics forces and velocity changes should take place in the FixedUpdate() loop
    private void FixedUpdate()
    {
        if (jumpFlag)
        {
            airTime += Time.deltaTime;

            float gravityMultiplier = gravityOverTime.Evaluate(airTime);
            rb.gravityScale = 3 + (3 * gravityMultiplier * gravityTimeMultiplier);
            return;
        }
        else {
            rb.gravityScale = 0;
            airTime = 0.0f;
        }

        // set the rigidbody velocity   
        rb.velocity = vel;
    }

    private void ResetJumpFlag() => canResetJumpFlag = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (jumpFlag) return;

        // get the last normal
        lastNormal = collision.contacts[0].normal;

        // You can see this debug ray by enabling Gizmos in Play Mode
        Debug.DrawRay(transform.position, lastNormal * 2.0f, Color.red);
        Debug.DrawRay(transform.position, Vector2.Perpendicular(lastNormal) * 2.0f * -(int)direction, Color.green);
    }
}
