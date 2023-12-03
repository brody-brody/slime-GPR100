using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Animator animator;
    private SethPlayerTest movement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<SethPlayerTest>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            animator.SetTrigger("Die");
            player.isStatic = true;
            movement.enabled = false;
        }
    }

    private void DeathScreen()
    {
        DeathManager.enableScreen?.Invoke();
        animator.enabled = false;
    }

}
