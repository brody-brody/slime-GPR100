using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // For team members who don't know what SerializeField does:
    // This is a PropertyAttribute. The [SerializeField] attribute makes it so private fields can be accessed and changed in the inspector while still being inaccessible to other scripts.
    [SerializeField] private float baseSpeed = 4.0f;

    // Jump fields
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityStrength = 9.81f;

    // Collision
    [SerializeField] private float surfaceCheckOffset = 0.5f; // Assuming that the player is 1x1 units, this will be used to check nearby surfaces along each side of the player
    [SerializeField] private LayerMask surfaceLayer;

    private Vector2 lastNormal; // normal vector of the last surface
    private float xInput; // stores the players input
    private bool isGrounded;

    private Rigidbody2D rb;


    private void Awake()
    {
        // Store the reference to the players Rigidbody2D inside the rb variable. Assume this script is attached to the player's GameObject which holds the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // store horizontal input into the xInput variable
        xInput = Input.GetAxisRaw("Horizontal");
    }

    // Physics forces and velocity changes should take place in the FixedUpdate() loop
    private void FixedUpdate()
    {
        // Get the vector perpendicular to the last normal vector and multiply it by the players input and the base speed
        rb.velocity = new(Vector2.Perpendicular(lastNormal).x * -xInput * baseSpeed, Vector2.Perpendicular(lastNormal).y * -xInput * baseSpeed);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        lastNormal = collision.contacts[0].normal;
        // You can see this debug ray by enabling Gizmos in Play Mode
        Debug.DrawRay(transform.position, lastNormal * 2.0f, Color.red);
        Debug.DrawRay(transform.position, Vector2.Perpendicular(lastNormal) * 2.0f * -xInput, Color.green);
    }
}
