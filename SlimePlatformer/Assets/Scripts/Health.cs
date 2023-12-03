using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Animator animator;
    private SethPlayerTest movement;

    private Rigidbody2D rb;

    [SerializeField]
    private float
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

    /// <summary>
    /// Deal damage to the player. Called with Health.takeDamage?.Invoke(1);
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        //Guard Clause
        if (!canDamage)
            return;

        //Deal damage
        canDamage = false;
        health -= damage;

        //Start flashing
        StartCoroutine(IFrame());

        //Death
        if (health <= 0)
        {
            animator.SetTrigger("Die");
            movement.enabled = false;
            rb.velocity = Vector2.zero;
        }
    }
    /// <summary>
    /// Makes player invincable and flash.
    /// </summary>
    /// <returns></returns>
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
        //DeathManager.enableScreen?.Invoke();
        animator.enabled = false;
        //SceneLoader.loadScene?.Invoke();
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