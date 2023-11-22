using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethPlayerTest : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float gravityMultiplier = 9.81f;
    [SerializeField] private float stickToNormalForce = 15.0f;
    [SerializeField] private float jumpForce = 5.0f;

    private Vector2 currentNormal = Vector2.up;
    private Rigidbody2D rb;

    private float xInput;
    private Vector2 vel;

    private Vector2 currentGravity;
    private bool wantsToLeaveGround = false;
    private bool jumpFlag = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // store horizontal input into the xInput variable
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space)) {
            wantsToLeaveGround = true;
        }

        Ray ray = new Ray(transform.position, -currentNormal);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction.normalized, 0.25f, groundLayer);

        Debug.DrawRay(ray.origin, ray.direction.normalized * 0.25f, Color.red);

        // set current gravity based on whether or the not the player wants to leave the ground

        if(wantsToLeaveGround || jumpFlag) {
            currentGravity = new Vector2(0.0f, -gravityMultiplier);
        }
        else {
            currentGravity = -currentNormal * stickToNormalForce;
        }
        

        // set the velocity
        vel = new(Vector2.Perpendicular(currentNormal.normalized).x * -xInput * moveSpeed, Vector2.Perpendicular(currentNormal.normalized).y * -xInput * moveSpeed);
    }

    private void FixedUpdate()
    {
        vel += currentGravity;

        if (wantsToLeaveGround) {
            wantsToLeaveGround = false;

            jumpFlag = true;
            Invoke(nameof(ResetJumpFlag), 0.15f);

            rb.velocity = new Vector2(currentNormal.x * jumpForce, currentNormal.y * jumpForce);
        }

        rb.velocity = vel;
    }

    private void ResetJumpFlag() => jumpFlag = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        currentNormal = collision.contacts[0].normal;
    }
}
