using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Animator animator;
    private SethPlayerTest movement;

    [SerializeField] private float invFrames;

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
    }
    private void TakeDamage(int damage)
    {
        //Guard Clause
        if (!canDamage)
            return;

        canDamage = false;
        health -= damage;

        StartCoroutine(IFrame(invFrames));

        if (health <= 0)
        {
            animator.SetTrigger("Die");
            player.isStatic = true;
            movement.enabled = false;
        }
    }

    IEnumerator IFrame(float frames)
    {
        //Player Flash
        for (int i = 0; i < frames; i++)
        {
            sprite.enabled = !sprite.enabled;
        }
        canDamage = true;
        yield return null;
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
