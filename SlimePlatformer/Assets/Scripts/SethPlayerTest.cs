using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SethPlayerTest : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 currentNormal = Vector2.up;
    private Rigidbody2D rb;

    private float xInput;
    private Vector2 vel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // store horizontal input into the xInput variable
        xInput = Input.GetAxisRaw("Horizontal");

        Ray ray = new Ray(transform.position, -currentNormal);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction.normalized, 1, groundLayer);

        Debug.DrawRay(ray.origin, ray.direction.normalized * 1, Color.red);

        if (hit) {
            Debug.Log("HELP!");
            Ray r2 = new Ray(hit.point, currentNormal);
            //transform.position = new Vector2(hit.point.x + r2.GetPoint(0.5f).x, hit.point.y + r2.GetPoint(0.5f).y);
            
            //Debug.DrawRay(r2.origin, currentNormal, Color.cyan);
        }

        //Debug.DrawRay(ray.origin, ray.direction * 0.5f, Color.blue);
        //Debug.DrawRay(ray.origin, currentNormal * 2.0f, Color.yellow);

        vel = new(Vector2.Perpendicular(currentNormal).x * -xInput * moveSpeed, Vector2.Perpendicular(currentNormal).y * -xInput * moveSpeed);
    }

    private void FixedUpdate()
    {
        rb.velocity = vel;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        currentNormal = collision.contacts[0].normal;
    }
}
