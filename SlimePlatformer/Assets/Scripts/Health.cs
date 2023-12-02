using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Animator animator;
    private SethPlayerTest movement;

    private Rigidbody2D rb;

    [SerializeField] private float 
        invFrames = 2,
        interval = 0.1f;

    [SerializeField] private SpriteRenderer sprite;

    private int health = 3;

    private bool canDamage = true;

    //Call Delegate to deal damage. Called by enemies
    public delegate void Damage(int damage);
    public static Damage takeDamage;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<SethPlayerTest>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        //Guard Clause
        if (!canDamage)
            return;

        canDamage = false;
        health -= damage;

        StartCoroutine(IFrame());

        if (health <= 0)
        {
            animator.SetTrigger("Die");
            movement.enabled = false;
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator IFrame()
    {
        //Player Flash
        for (int i = 0; i < invFrames; i++)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(interval);
        }

        yield return null;
        canDamage = true;
        sprite.enabled = true;
    }
    /// <summary>
    /// Called by Animator
    /// </summary>
    private void DeathScreen()
    {
        DeathManager.enableScreen?.Invoke();
        animator.enabled = false;
    }
    private void OnEnable()
    {
        takeDamage += TakeDamage;
    }
    private void OnDisable()
    {
        takeDamage -= TakeDamage;
    }
}
