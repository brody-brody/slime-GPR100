using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : MonoBehaviour
{

    [SerializeField] private float gravityMultiplier = 9.806f;
    [SerializeField] private float stickToNormalForce = 15.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, 10 / 10);

        currentGravity = -currentNormal.normalized * stickToNormalForce;
    }
    private void FixedUpdate()
    {
        currentVelocity += currentGravity;

        rb.velocity = currentVelocity;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contactCount > 1)
        {
            currentNormal = collision.contacts[1].normal;
        }
        currentVelocity = new(Vector2.Perpendicular(currentNormal).x * speed, speed * Vector2.Perpendicular(currentNormal).y);
        Debug.DrawRay(transform.position, currentNormal * 2, Color.gray);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
            Health.takeDamage(3);
    }
}
