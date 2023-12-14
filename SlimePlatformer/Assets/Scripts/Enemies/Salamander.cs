using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salamander : MonoBehaviour
{
    [Header("Salamander Movement Settings")]
    [SerializeField] private float gravityMultiplier = 9.806f;
    [SerializeField] private float stickToNormalForce = 15.0f;

    private Transform player; 

    private Vector2
        currentNormal = Vector2.up,
        currentVelocity,
        currentGravity;

    private Rigidbody2D rb;

    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //Get player location
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        //Set enemies gravity
        currentGravity = -currentNormal.normalized * stickToNormalForce;

        //Collision detection
        if (Vector2.Distance(transform.position, player.position) < 0.5f)
        {
            //Damages player for 3 health
            Health.takeDamage(3);
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(currentGravity, ForceMode2D.Force);

        rb.velocity = currentVelocity;

        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = Vector2.ClampMagnitude(currentVelocity, speed);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Find normal from current surface, takes only one normal at a time
        currentNormal = collision.contacts[0].normal;

        if (collision.contactCount > 1)
        {
            currentNormal = collision.contacts[1].normal;
        }

        currentVelocity = new(Vector2.Perpendicular(currentNormal).x * speed, speed * Vector2.Perpendicular(currentNormal).y);

        //Debug
        Debug.DrawRay(transform.position, currentNormal * 2, Color.green);
        Debug.DrawRay(transform.position, currentVelocity * 2, Color.blue);
    }
}
