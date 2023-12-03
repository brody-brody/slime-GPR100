using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBrick : MonoBehaviour
{
    // must be moving fast to break
    [SerializeField] private ParticleSystem breakParticle;
    [SerializeField] private float minYVelocityToBreak = 5.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the player
        if (collision.gameObject.TryGetComponent<SethPlayerTest>(out SethPlayerTest player))
        {
            // Check if the velocity is faster than the minimum velocity to break the brick, the player must also be in the air
            if(player.LastVelocityBeforeCollision.y >= minYVelocityToBreak && !player.IsGrounded)
            {
                // set the velocity to what the velocity was before collision
                player.GetComponent<Rigidbody2D>().velocity = player.LastVelocityBeforeCollision;
                Break();
            }
        }
    }

    private void Break()
    {
        if (GameManager.instance != null && CameraShake.instance != null) StartCoroutine(BreakAnim());
        // if the stuff that is necesarry for doing the break animation is null, just break normally
        else SimpleBreak();
    }

    private void SimpleBreak()
    {
        // spawn particle
        Instantiate(breakParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator BreakAnim()
    {
        // cool slowdown effect w/ shake
        GameManager.instance.SetTimeScale(0, 0.01f);
        CameraShake.instance.Shake(0.35f, 0.3f);

        yield return new WaitForSecondsRealtime(0.15f);
        GameManager.instance.SetTimeScale(1, 0.01f);

        // break
        SimpleBreak();
    }
}
