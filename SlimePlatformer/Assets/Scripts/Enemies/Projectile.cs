using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float speed = 3;
    [SerializeField] private float lifetime = 5;

    private Transform player;

    private Rigidbody2D rb;
    [HideInInspector] public Vector3 target;
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

        //Destroys bullet after lifetime expires
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(gameObject);   

        //Detect collision with player
        if (Vector2.Distance(transform.position, player.position) < 0.5f)
        {
            Health.takeDamage(1);
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        //Apply forces
        rb.velocity = speed * Time.fixedDeltaTime * target;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
