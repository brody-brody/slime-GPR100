using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBaby : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private float speedVariation = 0.5f;
    [SerializeField] private float playerBoostForce = 12.0f;
    [SerializeField] private Vector2 playerHurtForce = new Vector2(6, 5);
    [SerializeField] private LayerMask groundLayer;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider;
    [SerializeField] private SpriteRenderer sprite;

    [Header("SFX")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip deathClip;

    bool isGrounded = false;
    bool isDead = false;

    int dir = 1;

    private void Start()
    {
        speed = speed + Random.Range(-speedVariation, speedVariation);
    }

    private void Update()
    {
        if (isDead) return;

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

        /**
         * 
         *             Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (player.transform.position.y > transform.position.y + 0.45)
            {
                // bounce player
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                rb.AddForce(Vector2.up * playerBoostUpForce, ForceMode2D.Impulse);

                Hit();
            }
            else
            {
                player.SetJumpFlagTemporarily();
                rb.velocity = new Vector2(0.0f, 0.0f);
                
                rb.AddForce(new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x) * playerHurtForce.x, playerHurtForce.y), ForceMode2D.Impulse);

                Debug.Log(Mathf.Sign(player.transform.position.x - transform.position.x));*/
        if (collision.gameObject.TryGetComponent<SethPlayerTest>(out SethPlayerTest player))
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (player.transform.position.y > transform.position.y + 0.45f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                rb.AddForce(Vector2.up * playerBoostForce, ForceMode2D.Impulse);

                DeathAnim();
            }
            else
            {
                player.SetJumpFlagTemporarily();
                rb.velocity = new Vector2(0.0f, 0.0f);

                rb.AddForce(new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x) * playerHurtForce.x, playerHurtForce.y), ForceMode2D.Impulse);
            }
            dir = -dir;
        }
    }

    private void DeathAnim()
    {
        collider.enabled = false;
        rb.velocity = Vector2.zero;
        isDead = true;

        rb.freezeRotation = false;
        rb.angularVelocity = Random.Range(150, 200) * dir;
        rb.AddForce(Vector2.up * 12.0f, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * dir * 3.0f, ForceMode2D.Impulse);

        source.clip = deathClip;
        source.Play();

        Destroy(gameObject, 1.5f);
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
