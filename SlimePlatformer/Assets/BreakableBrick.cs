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
        if (collision.gameObject.TryGetComponent<SethPlayerTest>(out SethPlayerTest player)) {
            if(player.LastVelocityBeforeCollision.y >= minYVelocityToBreak && !player.IsGrounded) {

                player.GetComponent<Rigidbody2D>().velocity = player.LastVelocityBeforeCollision;
                Break();
            }
        }
    }

    private void Break()
    {
        if (GameManager.instance != null && CameraShake.instance != null) StartCoroutine(BreakAnim());
        else SimpleBreak();
    }

    private void SimpleBreak()
    {
        Instantiate(breakParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator BreakAnim()
    {
        GameManager.instance.SetTimeScale(0, 0.01f);
        CameraShake.instance.Shake(0.35f, 0.3f);

        yield return new WaitForSecondsRealtime(0.15f);
        GameManager.instance.SetTimeScale(1, 0.01f);

        SimpleBreak();
    }
}
