using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovementTest : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 6.0f;
    [SerializeField] private LayerMask surfaceLayer;

    private Vector2 input;
    private Vector2 currentNormal;
    private Vector2 groundVel;

    // -1 if left / down, 1 if up / right
    Vector2 lastInput;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();

        CheckForNewSurface();
    }

    /// <summary>
    /// Get player input
    /// </summary>
    private void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if(input != Vector2.zero) {
            lastInput = input;
        }

        rb.velocity = new Vector2(input.x * playerSpeed, input.y * playerSpeed);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // if theres more than 1 normal that was hit, compare with the players input direction to get the correct one to move on

        /*
        if (collision.contactCount > 2) {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 dir = Vector2.Perpendicular(new Vector2(contact.normal.x, Mathf.Abs(contact.normal.y)));

                // doesnt quite work derpp

                Debug.Log("Dir: " + dir);
                if (dir.y == input.y || dir.y == -input.y || dir.x == input.x || dir.x == -input.x) {
                    currentNormal = dir;
                    break;
                }
            }

        } else {
            currentNormal = collision.contacts[0].normal;
        }*/

        currentNormal = collision.contacts[0].normal;
        Debug.DrawRay(transform.position, currentNormal, Color.green);
    }

    private Ray lastExitedRay;

    private void CheckForNewSurface()
    {
        // Get the vector perpendicular to the last normal vector and multiply it by the players input and the base speed
        Vector2 up = currentNormal;

        // should be multiplied by dir
        Vector2 forward = Vector2.zero;
        if (Mathf.Abs(currentNormal.x) > 0.1f) forward = -Vector2.Perpendicular(currentNormal) * -lastInput.y;
        else if (Mathf.Abs(currentNormal.y) > 0.1f) forward = -Vector2.Perpendicular(currentNormal) * -lastInput.x;

        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 dir = -up * 0.6f;

        Ray leftRayCheck = new Ray(playerPos + (-up * 0.5f + forward * 0.5f), dir);
        Ray rightRayCheck = new Ray(playerPos + (-up * 0.5f + forward * -0.5f), dir);

        bool checkOnRightRay = false;

        if(Mathf.Abs(currentNormal.x) > 0.1f) {
            if (input.y > 0)
            {
                checkOnRightRay = true;
                Debug.DrawRay(leftRayCheck.origin, leftRayCheck.direction, Color.red);
                Debug.DrawRay(rightRayCheck.origin, rightRayCheck.direction, Color.cyan);
            }
            if (input.y < 0)
            {
                checkOnRightRay = false;
                Debug.DrawRay(leftRayCheck.origin, leftRayCheck.direction, Color.cyan);
                Debug.DrawRay(rightRayCheck.origin, rightRayCheck.direction, Color.red);
            }

        }
        else if(Mathf.Abs(currentNormal.y) > 0.1f) {
            if(input.x > 0) {
                checkOnRightRay = true;
                Debug.DrawRay(leftRayCheck.origin, leftRayCheck.direction, Color.red);
                Debug.DrawRay(rightRayCheck.origin, rightRayCheck.direction, Color.cyan);
            }
            if(input.x < 0) {
                checkOnRightRay = false;
                Debug.DrawRay(leftRayCheck.origin, leftRayCheck.direction, Color.cyan);
                Debug.DrawRay(rightRayCheck.origin, rightRayCheck.direction, Color.red);
            }
        }

        if(!Physics2D.Raycast(leftRayCheck.origin, leftRayCheck.direction, 0.02f, surfaceLayer) && !Physics2D.Raycast(rightRayCheck.origin, rightRayCheck.direction, 0.02f, surfaceLayer)) {
            Debug.Log("nothing was found!");

            if (checkOnRightRay) {
                Ray checkRay = new Ray(rightRayCheck.GetPoint(0.1f), -forward);
                Debug.DrawRay(checkRay.origin, checkRay.direction, Color.magenta);
            }
            else {
                Ray checkRay = new Ray(leftRayCheck.GetPoint(0.1f), forward);
                Debug.DrawRay(checkRay.origin, checkRay.direction, Color.magenta);
            }
        }

        // get the first one that leaves and keep that recorded

            /*
            // Fire two rays starting from
            // This raycast essentially checks both edges of the player's bottom face (respective to the normal of the last touched surface)
            if (!Physics2D.Raycast(playerPos + (-up * 0.5f + forward * 0.5f), dir, surfaceCheckRayLength, surfaceLayer) && !Physics2D.Raycast(playerPos + (-up * 0.5f + forward * -0.5f), dir, surfaceCheckRayLength, surfaceLayer) && rb.velocity != Vector2.zero)
            {
                // Create a new ray starting from the corner of player and try to find the next surface
                Ray raycast = new Ray(playerPos + (-up * 0.6f + forward * -0.45f), new Vector2(-Vector2.Perpendicular(-lastNormal).x * (int)direction, -Vector2.Perpendicular(-lastNormal).y * (int)direction));
                RaycastHit2D rayHit = Physics2D.Raycast(raycast.origin, raycast.direction, 0.5f, surfaceLayer);

                // snap to the found surface
                if (rayHit.collider != null)
                {
                    transform.position = rayHit.point + (-up * -0.5f + forward * 0.5f);

                    // set the new normal to use in velocity calculations to wrap around the surface
                    //lastNormal = new Vector2(Vector2.Perpendicular(-lastNormal).x * (int)direction, Vector2.Perpendicular(-lastNormal).y * (int)direction);
                }
            }*/

    }
}
