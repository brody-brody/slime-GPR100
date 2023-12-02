using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBaby : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;

    bool isGrounded = false;
    int dir = 1;

    private void Update()
    {
        if (isGrounded)
        {
            Ray ray = new Ray(new Vector2(transform.position.x, transform.position.y + 0.53f), Vector2.right * dir);

            if (Physics2D.Raycast(ray.origin, ray.direction * 0.55f, 0.55f, groundLayer))
            {
                dir = -dir;
            }
            // no ground??
            if (!Physics2D.Raycast(ray.origin + (ray.direction * 0.5f), Vector2.down * 1.5f, 1.5f, groundLayer))
            {
                dir = -dir;
            }
        }


        if (dir == -1) sprite.flipX = false;
        else sprite.flipX = true;

        if(isGrounded) rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SethPlayerTest>())
        {
            dir = -dir;
        }
    }

    public void RandomizeDirection()
    {
        int r = Random.Range(-20, 20);

        dir = (int)Mathf.Sign(r) * dir;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
