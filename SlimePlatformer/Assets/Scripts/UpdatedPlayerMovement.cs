using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedPlayerMovement : MonoBehaviour
{
    // For team members who don't know what SerializeField does:
    // This is a PropertyAttribute. The [SerializeField] attribute makes it so private fields can be accessed and changed in the inspector while still being inaccessible to other scripts.
    [SerializeField]
    private float baseSpeed = 4.0f;

    // Jump fields
    [SerializeField] private float 
        jumpForce = 5.0f, 
        gravityStrength = 9.81f;

    // Dash fields
    [SerializeField] private float 
        dashCooldown = 0.5f,
        dashForce = 15.0f;

    // Collision
    [SerializeField] private float surfaceCheckOffset = 0.5f; // Assuming that the player is 1x1 units, this will be used to check nearby surfaces along each side of the player
    [SerializeField] private LayerMask surfaceLayer;

    private Vector2 lastNormal; // normal vector of the last surface
    private float xInput; // stores the players input
    private bool isGrounded;

    private Rigidbody2D rb;

    private bool isDashing;


    private void Awake()
    {
        // Store the reference to the players Rigidbody2D inside the rb variable. Assume this script is attached to the player's GameObject which holds the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // store horizontal input into the xInput variable
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Jump") && !isDashing)
            if (Input.GetButtonDown("Vertical"))
            {
                Vector2 direction = new(0, Input.GetAxisRaw("Vertical"));
                Dash(direction);
            }
    }

    // Physics forces and velocity changes should take place in the FixedUpdate() loop
    private void FixedUpdate()
    {
        

        //If isDashing is true, it prevents the code following it from running
        if (isDashing)
            return;

        // Get the vector perpendicular to the last normal vector and multiply it by the players input and the base speed
        rb.velocity = new(Vector2.Perpendicular(lastNormal).x * -xInput * baseSpeed, Vector2.Perpendicular(lastNormal).y * -xInput * baseSpeed);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //If the normal is facing any way but up it inverses the horizontal controls
        if (collision.GetContact(0).normal != Vector2.up)
            lastNormal = -collision.GetContact(0).normal;
        else 
            lastNormal = collision.GetContact(0).normal;
        

        // You can see this debug ray by enabling Gizmos in Play Mode
        Debug.DrawRay(transform.position, lastNormal * 2.0f, Color.red);
        Debug.DrawRay(transform.position, Vector2.Perpendicular(lastNormal) * 2.0f * -xInput, Color.green);
    }
    /// <summary>
    /// Takes a Vertical movement from the player and applies force when the dash key and movement key are pressed
    /// </summary>
    /// <param name="direction"></param>
    void Dash(Vector2 direction)
    {
        //Debug ray for seeing dash direction
        Debug.DrawRay(transform.position, direction * 2, Color.blue);

        isDashing = true;

        //Add Forces to dash
        rb.AddForce(direction * dashForce, ForceMode2D.Impulse);

        //Calls Reset dash function after a certain time had passed. In this case it is dashCooldown which is 1 second
        Invoke(nameof(ResetDash), dashCooldown);
    }
    /// <summary>
    /// Resets Dash by changing bool to false
    /// </summary>
    void ResetDash() => isDashing = false;
}
