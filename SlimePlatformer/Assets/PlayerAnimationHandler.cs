using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private float upSmoothingInAir = 2.2f;
    [SerializeField] private float upSmoothingOnGround = 0.25f;
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
        if (movement.IsMoving)
        {
            myAnimator.SetBool("Walking", true);
        }
        else
        {
            myAnimator.SetBool("Walking", false);
        }

        if (movement.PlayerJumpFlag && !jumping)
        {
            myAnimator.SetTrigger("Jump");
            jumping = true;

        }

        if (!movement.PlayerJumpFlag)
        {
            jumping = false;
        }

        if (!movement.IsGrounded)
        {
            awaitingFall = true;
        }

        if (movement.IsGrounded && awaitingFall)
        {
            myAnimator.SetTrigger("Land");
            awaitingFall = false;
        }

    }

    private void UpdateRotation()
    {
        if (!movement.IsGrounded) {
            renderer.transform.up = Vector3.Lerp(renderer.transform.up, Vector3.up, Time.deltaTime * upSmoothingInAir);
        }
        else {
            renderer.transform.up = Vector3.Lerp(renderer.transform.up, movement.CurrentNormal, Time.deltaTime * upSmoothingOnGround);
        }
        
    }
}
