using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
        float angle = Vector3.Angle(transform.up, movement.CurrentNormal);
        if (movement.CurrentNormal.x > 0) angle = -angle;

        Quaternion fromQuat = renderer.transform.localRotation;
        Quaternion toQuat = movement.IsGrounded ? Quaternion.AngleAxis(angle, Vector3.forward) : Quaternion.Euler(0,0,0);
        float smoothingFactor = movement.IsGrounded ? upSmoothingOnGround : upSmoothingInAir;

        renderer.transform.localRotation = Quaternion.Lerp(fromQuat, toQuat, Time.deltaTime * smoothingFactor);
    }
}
