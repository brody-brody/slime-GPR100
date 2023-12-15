using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private ResetLevel resetLevel;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator animator;

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

        animator.SetInteger("Health", health);

        //Start flashing
        if (health > 0)
            StartCoroutine(IFrame());
        else
            StopAllCoroutines();

        //Death
        if (health <= 0)
        {
            if (!resetLevel) resetLevel = FindObjectOfType<ResetLevel>();
            resetLevel.Death(ResetLevel.DeathType.Enemy);
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
    private void OnEnable()
    {
        takeDamage += TakeDamage;
    }
    private void OnDisable()
    {
        takeDamage -= TakeDamage;
    }
}