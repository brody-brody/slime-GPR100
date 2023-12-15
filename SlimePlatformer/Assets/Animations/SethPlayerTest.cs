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
    [SerializeField]
    private float maxAirVelocity = 5.0f;

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

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioClip hitMagmaClip;
    [SerializeField] private AudioClip soarMagmaClip;

    [SerializeField] private Collider2D collider;

    private Vector2 currentNormal = Vector2.up;
    private Rigidbody2D rb;

    // jump variables
    private float jumpTimeStamp;

    private bool onMagma = false;
    public bool OnMagma { get { return onMagma; } }

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
    private float lastInput;

    private bool allowAirControl = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Suspends all player activity
    /// </summary>
    public void SuspendAll()
    {
        canMove = false;
        rb.isKinematic = true;
        collider.enabled = false;
        rb.velocity = Vector2.zero;
        magmaParticles.Stop();
        jumpTrail.emitting = false;
    }

    /// <summary>
    /// Unsuspends all player activity
    /// </summary>
    public void UnsuspendAll()
    {
        canMove = true;
        rb.isKinematic = false;
        collider.enabled = true;
        rb.velocity = Vector2.zero;
    }

    void Update()
    {
        // store horizontal input into the xInput variable
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (!canMove) return;

        // get if the player is holding the jump key
        holdingJumpKey = Input.GetKey(KeyCode.Space);

        TryQueueJump();

        // set current gravity based on whether or the not the player wants to leave the ground
        if (!isGrounded || jumpFlag) {
            currentGravity = new Vector2(0.0f, 0.0f);
            rb.gravityScale = gravityMultiplier;
            lastInput = xInput;
        }
        if(isGrounded && !jumpFlag)
        {
            currentGravity = -currentNormal.normalized * stickToNormalForce;
            rb.gravityScale = 0.0f;
            jumpTrail.emitting = false;
            onMagma = false;
        }
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
            sfxSource.PlayOneShot(jumpClip);
            wantsToLeaveGround = true;
            jumpBufferTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        // Set the velocity before collision so other scripts can access it before OnCollisionStay overrides it
        lastVelocityBeforeCollision = rb.velocity;

        // Add the gravity from the normal on ground to the velocity
        vel += currentGravity;

        // If the player wants to jump, jump
        if (wantsToLeaveGround) Jump();
        // If the player is grounded and the jump flag isnt on 
        else if(!jumpFlag && isGrounded) rb.velocity = vel;

        // allow key press while jumping
        if(Time.time < jumpTimeStamp + jumpTime && holdingJumpKey && !isGrounded) {
            if(currentNormal.y > -0.5f) {
                rb.AddForce(Vector2.up * additionalJumpForce, ForceMode2D.Force);
            }
        }

        // some slight air control while in air. This is very bad. It's constantly adding force so itll continoually speed up
        if (!isGrounded)
        {
            //if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(xInput) || Mathf.Abs(rb.velocity.x) < 0.1f)
            //{
            //Debug.Log("Aircontrol baby T-T UWU WUUWUW OWO ");
            //if (rb.velocity.magnitude > maxAirVelocity)

            Debug.Log("Sign of Vel: " + Mathf.Sign(rb.velocity.x) + " Sign of X INPUT: " + Mathf.Sign(xInput));

            if(Mathf.Sign(rb.velocity.x) != Mathf.Sign(xInput) || Mathf.Abs(rb.velocity.x) < 0.01f)
            {
                allowAirControl = true;
            }

            Vector2 flatVelocity = new Vector2(rb.velocity.x, 0.0f);
            Vector2 desiredVel = Vector2.right * airControl * xInput;
            // slight air control
            if(allowAirControl) rb.AddForce(desiredVel - flatVelocity, ForceMode2D.Force);
            //}

        }
    }

    //
    private void ResetJumpFlag() => jumpFlag = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // This is in on collision enter just so it doesnt call the same function multiple times
        if (((1 << collision.gameObject.layer) & groundLayer) == 0)
        {
            if (((1 << collision.gameObject.layer) & nonstickLayer) != 0)
            {
                // start the hit magma coroutine
                StartCoroutine(HitMagma());
            }
            else
            return;
        }
    }

    IEnumerator HitMagma()
    {
        // Play the magma clip
        sfxSource.PlayOneShot(hitMagmaClip);

        yield return new WaitForSeconds(0.08f);

        // slow the time down and shake the screen
        GameManager.instance.SetTimeScale(0f, 0.01f);
        CameraShake.instance.Shake(0.4f, 0.4f);

        yield return new WaitForSecondsRealtime(0.1f);
        sfxSource.PlayOneShot(soarMagmaClip);
        yield return new WaitForSecondsRealtime(0.08f);
        GameManager.instance.SetTimeScale(1f, 0.01f);

        yield return null;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // ensure the player hit the ground mask
        if (((1 << collision.gameObject.layer) & groundLayer) == 0) {
            if(((1 << collision.gameObject.layer) & nonstickLayer) != 0)
            {
                // if the player hit the magma, play the particles
                magmaParticles.Play();

                jumpFlag = true;
                Invoke(nameof(ResetJumpFlag), 0.06f);

                rb.velocity = Vector2.zero;
                onMagma = true;

                // boost the player in the direction of the normal
                rb.AddForce(collision.contacts[0].normal * magmaBoostForce, ForceMode2D.Impulse);
            }

            isGrounded = false;

            return;
        }

        allowAirControl = false;
        // stop the magma particles if the players grounded
        if (isGrounded && !jumpFlag) magmaParticles.Stop();

        isGrounded = true;

        // update the normal
        currentNormal = collision.contacts[0].normal;

        // change the normal to the other contact
        if(collision.contactCount > 1) {
            currentNormal = collision.contacts[1].normal;
        }

        // This is all ghost code that would have done the WASD movement, but it didnt work correctly

        // Set the velocity while the player is on the ground
        vel = new(Vector2.Perpendicular(currentNormal).x * -xInput * moveSpeed, Vector2.Perpendicular(currentNormal).y * -xInput * moveSpeed);

        Debug.DrawRay(transform.position, currentNormal.normalized, Color.cyan);
    }

    /// <summary>
    /// Causes the player to jump
    /// </summary>
    private void Jump()
    {
        wantsToLeaveGround = false;

        // Set the jump flag
        jumpFlag = true;
        jumpTimeStamp = Time.time;
        Invoke(nameof(ResetJumpFlag), 0.06f);

        // set the jump trail on
        jumpTrail.emitting = true;

        // flatten the y vel
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);

        // as long as the player isnt on the ceiling, add jump force
        if (currentNormal.y != -1)
            rb.AddForce(Vector2.up * jumpUpForce, ForceMode2D.Impulse);

        // add force in the direction of the current normal.x
        rb.AddForce(Vector2.right * currentNormal.x * sideJumpForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Sets the jump flag temporarily. This is for enemies to call externally so they can bounce the player properly with 
    /// </summary>
    public void SetJumpFlagTemporarily()
    {
        jumpFlag = true;
        Invoke(nameof(ResetJumpFlag), 0.06f);
    }

    private void OnCollisionExit2D(Collision2D collision) => isGrounded = false;
}
