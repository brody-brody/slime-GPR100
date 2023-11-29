using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethPlayerTest : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Gravity Forces")]
    [SerializeField] private float gravityMultiplier = 9.806f;
    [SerializeField] private float stickToNormalForce = 15.0f;

    [Header("Jump")]
    [SerializeField] private float sideJumpForce = 5.0f;
    [SerializeField] private float jumpUpForce = 5.0f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    private Vector2 currentNormal = Vector2.up;
    private Rigidbody2D rb;

    private float xInput;
    private Vector2 vel;

    private Vector2 currentGravity;
    private bool isGrounded = false;
    private bool wantsToLeaveGround = false;
    private bool jumpAttempted = false;
    private bool jumpFlag = false;

    private float jumpBufferTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // store horizontal input into the xInput variable
        xInput = Input.GetAxisRaw("Horizontal");

        TryQueueJump();

        //Ray ray = new Ray(transform.position, -currentNormal);
        //Debug.DrawRay(ray.origin, ray.direction * 3f, Color.cyan);

        // set current gravity based on whether or the not the player wants to leave the ground

        if(!isGrounded || jumpFlag) {
            currentGravity = new Vector2(0.0f, 0.0f);
            rb.gravityScale = gravityMultiplier;
        }

        if(isGrounded && !jumpFlag)
        {
            currentGravity = -currentNormal.normalized * stickToNormalForce;
            rb.gravityScale = 0.0f;
        }
        

        // set the velocity
        vel = new(Vector2.Perpendicular(currentNormal).x * -xInput * moveSpeed, Vector2.Perpendicular(currentNormal).y * -xInput * moveSpeed);
    }

    private void TryQueueJump()
    {
        // Reset counter when space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimer = jumpBufferTime;
        }
        // Countdown the jump buffer when not pressing space
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (jumpBufferTimer > 0 && isGrounded)
        {
            wantsToLeaveGround = true;
            jumpBufferTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        vel += currentGravity;

        if (wantsToLeaveGround) {
            wantsToLeaveGround = false;

            jumpFlag = true;
            Invoke(nameof(ResetJumpFlag), 0.06f);

            rb.velocity = new Vector2(rb.velocity.x, 0.0f);

            // as long as the player isnt on the ceiling
            if(currentNormal.y != -1)
                rb.AddForce(Vector2.up * jumpUpForce, ForceMode2D.Impulse);

            rb.AddForce(Vector2.right * currentNormal.x * sideJumpForce, ForceMode2D.Impulse);
            //rb.velocity = new Vector2(currentNormal.x * jumpForce, currentNormal.y * jumpForce);
        }
        else if(!jumpFlag && isGrounded)
        {
            rb.velocity = vel;
        }
    }

    private void ResetJumpFlag() => jumpFlag = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;

        currentNormal = collision.contacts[0].normal;

        // change the normal to the other contact
        if(collision.contactCount > 1)
        {
            currentNormal = collision.contacts[1].normal;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
