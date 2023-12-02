using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float speed = 3;
    [SerializeField] private float lifetime = 5;


    private Rigidbody2D rb;
    [HideInInspector] public Vector3 target;
    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(gameObject);   
    }
    private void FixedUpdate()
    {
        rb.velocity = speed * Time.fixedDeltaTime * target;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 3)
        {
            Health.takeDamage(1);
            Destroy(gameObject);
        }
    }
}
