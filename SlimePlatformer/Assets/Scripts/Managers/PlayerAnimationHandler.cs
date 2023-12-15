using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Transform targetRotation; 
    [SerializeField] private Animator myAnimator;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private float upSmoothingInAir = 2.2f;
    [SerializeField] private float upSmoothingOnGround = 0.25f;
    [SerializeField] private ParticleSystem deathParticles;

    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;

    [SerializeField] private AudioSource deathSoundsSource;
    [SerializeField] private AudioClip deathEnemySoundClip;
    [SerializeField] private AudioClip deathFallSoundClip;
    [SerializeField] private AudioClip deathSpikeSoundClip;

    private SethPlayerTest movement;

    private bool jumping;

    private bool awaitingFall;

    private void Awake()
    {
        movement = GetComponent<SethPlayerTest>();
    }

    void Update()
    {
        UpdateAnimation();

        //renderer.transform.up = movement.CurrentNormal;
        UpdateRotation();
    }

    private void UpdateAnimation()
    {
        // Set the sprite to the left / right sprite based on input
        if (movement.HorizontalInput > 0) renderer.sprite = spriteRight; 
        else if (movement.HorizontalInput < 0) renderer.sprite = spriteLeft;

        // Set walking based on if the player is moving
        if (movement.IsMoving)  myAnimator.SetBool("Walking", true);    
        else myAnimator.SetBool("Walking", false);

        // Jump animation trigger
        if (movement.PlayerJumpFlag && !jumping)
        {
            myAnimator.SetTrigger("Jump");
            // This is just a flag that ensures it doesnt trigger multiple
            jumping = true;
        }

        // cancel jump flag
        if (!movement.PlayerJumpFlag)
        {
            jumping = false;
        }

        // While the player isnt on the ground, assume falling
        if (!movement.IsGrounded)
        {
            awaitingFall = true;
        }

        // if falling, trigger the land animation
        if (movement.IsGrounded && awaitingFall)
        {
            myAnimator.SetTrigger("Land");
            awaitingFall = false;
        }

    }

    /// <summary>
    /// Updates the player rotation to align with the current surface
    /// </summary>
    private void UpdateRotation()
    {
        // Hey guys. I don't remember how the fuck this shit works. Thanks :D

        float angle = Vector3.Angle(transform.up, movement.CurrentNormal);

        // make angle negative if normal is left wall
        if (movement.CurrentNormal.x > 0) angle = -angle;

        Quaternion fromQuat = targetRotation.transform.localRotation;
        Quaternion toQuat = movement.IsGrounded ? Quaternion.AngleAxis(angle, Vector3.forward) : Quaternion.Euler(0, 0, 0);
        float smoothingFactor = movement.IsGrounded ? upSmoothingOnGround : upSmoothingInAir;

        // Smooth the rotation
        targetRotation.transform.localRotation = Quaternion.Lerp(fromQuat, toQuat, Time.deltaTime * smoothingFactor);
    }

    public void CallSpikeDeath()
    {
        renderer.enabled = false;
        deathParticles.Play();

        deathSoundsSource.PlayOneShot(deathSpikeSoundClip);
    }

    public void CallFallDeath()
    {
        myAnimator.SetTrigger("Die");

        deathSoundsSource.PlayOneShot(deathFallSoundClip);
    }

    public void CallEnemyDeath()
    {
        renderer.enabled = false;
        deathParticles.Play();

        deathSoundsSource.PlayOneShot(deathEnemySoundClip);
    }
}
