using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SethPlayerTest : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private Vector2 magmaBoostForce = new Vector2(4.0f, 6.0f);
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask nonstickLayer;

    [Header("Gravity Forces")]
    [SerializeField]
    private float gravityMultiplier = 9.806f;
    [SerializeField]
    private float stickToNormalForce = 15.0f;
    [SerializeField]
    private float airControl = 3.0f;

    [Header("Jump")]
    [SerializeField]
    private float sideJumpForce = 5.0f;
    [SerializeField]
    private float jumpUpForce = 5.0f;
    [SerializeField]
    private float additionalJumpForce = 5.0f;
    [SerializeField]
    private float jumpTime = 0.5f;
    [SerializeField]
    private float jumpBufferTime = 0.2f;

    [Header("VFX")]
    [SerializeField] private ParticleSystem magmaParticles;
    [SerializeField] private TrailRenderer jumpTrail;

    [SerializeField] private Collider2D collider;

    private Vector2 currentNormal = Vector2.up;
    private Rigidbody2D rb;

    // jump variables
    private float jumpTimeStamp;

    // input variables
    private float xInput;
    private float yInput;

    // velocity variables
    private Vector2 vel;
    private Vector2 currentGravity;
    private Vector2 lastVelocityBeforeCollision;

    // bools
    private bool holdingJumpKey = false;
    private bool canMove = true;
    private bool isGrounded = false;
    private bool wantsToLeaveGround = false;
    private bool jumpAttempted = false;
    private bool jumpFlag = false;

    #region Accessors
    public bool IsMoving { get { return rb.velocity.magnitude > 0.1f && xInput != 0 && isGrounded; } }
    public bool IsGrounded { get { return isGrounded; } }
    public bool PlayerJumpFlag { get { return jumpFlag; } }
    public Vector2 CurrentNormal { get { return currentNormal; } }
    public Vector2 LastVelocityBeforeCollision { get { return lastVelocityBeforeCollision; } }
    public float HorizontalInput { get { return xInput; } }
    #endregion

    private float jumpBufferTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SuspendAll()
    {
        canMove = false;
        rb.isKinematic = true;
        collider.enabled = false;
        rb.velocity = Vector2.zero;
    }

    public void UnsuspendAll()
    {
        canMove = true;
        rb.isKinematic = false;
        collider.enabled = true;
        rb.velocity = Vector2.zero;
    }

    void Update()
    {
        if (!canMove) return;

        // store horizontal input into the xInput variable
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        holdingJumpKey = Input.GetKey(KeyCode.Space);

        TryQueueJump();

        // set current gravity based on whether or the not the player wants to leave the ground

        if (!isGrounded || jumpFlag) {
            currentGravity = new Vector2(0.0f, 0.0f);
            rb.gravityScale = gravityMultiplier;
        }

        if(isGrounded && !jumpFlag)
        {
            currentGravity = -currentNormal.normalized * stickToNormalForce;
            rb.gravityScale = 0.0f;
            jumpTrail.emitting = false;
        }
        

        // set the velocity
      
       // else if(Mathf.Abs(currentNormal.x) > 0.01f)
         //   vel = new(Vector2.Perpendicular(currentNormal).x * yInput * moveSpeed, Vector2.Perpendicular(currentNormal).y * yInput * moveSpeed);
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
        if (!canMove) return;

        lastVelocityBeforeCollision = rb.velocity;

        vel += currentGravity;

        if (wantsToLeaveGround) {
            wantsToLeaveGround = false;

            jumpFlag = true;
            jumpTimeStamp = Time.time;
            Invoke(nameof(ResetJumpFlag), 0.06f);

            jumpTrail.emitting = true;

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

        // allow key press while jumping
        if(Time.time < jumpTimeStamp + jumpTime && holdingJumpKey && !isGrounded) {
            if(currentNormal.y > -0.5f) {
                rb.AddForce(Vector2.up * additionalJumpForce, ForceMode2D.Force);
            }
        }

        // air control
        if (!isGrounded)
        {
            rb.AddForce(Vector2.right * airControl * xInput, ForceMode2D.Force);
        }
    }

    private void ResetJumpFlag() => jumpFlag = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        // ensure the player hit the ground mask
        if (((1 << collision.gameObject.layer) & groundLayer) == 0) {
            if(((1 << collision.gameObject.layer) & nonstickLayer) != 0)
            {
                magmaParticles.Play();
                CameraShake.instance.Shake(0.3f, 0.4f);

                jumpFlag = true;
                Invoke(nameof(ResetJumpFlag), 0.06f);

                Debug.Log("Hit non stick!");

                rb.velocity = Vector2.zero;

                //if (currentNormal.y < 0.9f)
                   // rb.AddForce(Vector2.up * magmaBoostForce.y, ForceMode2D.Impulse);

                rb.AddForce(collision.contacts[0].normal * magmaBoostForce, ForceMode2D.Impulse);
            }

            isGrounded = false;

            return;
        }

        if(!jumpFlag && isGrounded)
        {
            magmaParticles.Stop();
        }

        isGrounded = true;

        currentNormal = collision.contacts[0].normal;

        // change the normal to the other contact
        if(collision.contactCount > 1) {
            currentNormal = collision.contacts[1].normal;
        }

        /*
        if (currentNormal.y > 0.01f)
        {
            Debug.Log("On up!");
            vel = new(Vector2.Perpendicular(currentNormal).x * -xInput * moveSpeed, Vector2.Perpendicular(currentNormal).y * -xInput * moveSpeed);
        }
        else if (Mathf.Abs(currentNormal.y) < 0.01f && currentNormal.x < 0)
        { // sides
            Debug.Log("On right side!");
            vel = new(Vector2.Perpendicular(currentNormal).x * -yInput * moveSpeed, Vector2.Perpendicular(currentNormal).y * -yInput * moveSpeed);
        }
        else if (Mathf.Abs(currentNormal.y) < 0.01f && currentNormal.x > 0)
        {
            Debug.Log("On left side!");
            vel = new(Vector2.Perpendicular(currentNormal).x * yInput * moveSpeed, Vector2.Perpendicular(currentNormal).y * yInput * moveSpeed);
        }
        else if (currentNormal.y < 0)
        {
            vel = new(Vector2.Perpendicular(currentNormal).x * xInput * moveSpeed, Vector2.Perpendicular(currentNormal).y * xInput * moveSpeed);
            Debug.Log("On down!");
        }*/

        vel = new(Vector2.Perpendicular(currentNormal).x * -xInput * moveSpeed, Vector2.Perpendicular(currentNormal).y * -xInput * moveSpeed);

        Debug.DrawRay(transform.position, currentNormal.normalized, Color.cyan);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
