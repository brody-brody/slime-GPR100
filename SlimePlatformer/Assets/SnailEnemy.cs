using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask hitWallDetectionMask;
    
    [Header("Player Interactions")]
    [SerializeField] private float playerBoostUpForce = 16.0f;
    [SerializeField] private Vector2 playerHurtForce = new Vector2(12, 15);
    [SerializeField] private SlimeBaby child;
    [SerializeField] private int childCount = 3;

    [Header("Renderer References")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private SpriteRenderer eyeLeft;
    [SerializeField] private SpriteRenderer eyeRight;

    [Header("Sprite References")]
    [SerializeField] private Sprite[] shellDamageSprites;
    [SerializeField] private Sprite eyeballNormal;
    [SerializeField] private Sprite eyeballLeftHurt;
    [SerializeField] private Sprite eyeballRightHurt;

    [Header("VFX References")]
    [SerializeField] private LineRenderer leftEyeAntenna;
    [SerializeField] private LineRenderer rightEyeAntenna;
    [SerializeField] private ParticleSystem deathParticle;

    [Header("SFX")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip crackShellSound;

    [Header("Eye Position")]
    [SerializeField] private Transform eyeLeftHolder;
    [SerializeField] private Transform eyeRightHolder;

    [SerializeField] private Vector2 leftEyesFacingRightPos;
    [SerializeField] private Vector2 leftEyesFacingLeftPos;
    [SerializeField] private Vector2 rightEyesFacingLeftPos;
    [SerializeField] private Vector2 rightEyesFacingRightPos;
    [SerializeField] private Vector2 antennaPositionLeft;
    [SerializeField] private Vector2 antennaPositionRight;

    [SerializeField] private float eyeLeftPositionSmoothing = 10.5f;
    [SerializeField] private float eyeRightPositionSmoothing = 7.0f;
    [SerializeField] private Vector2 randomEyeSpeed;
    [SerializeField] private float maxEyeSinDistance = 0.2f;

    Vector2 eyeballLeftVel;
    Vector2 eyeballRightVel;

    Vector2 originEyePosLeft;
    Vector2 originEyePosRight;

    Vector2 eyePosLeft;
    Vector2 eyePosRight;

    float eyeballSinSpeedLeft;
    float eyeballSinSpeedRight;

    bool playingDamageAnim;

    int health = 3;

    int dir = -1;

    private void Awake()
    {
        originEyePosLeft = eyePosLeft;
        originEyePosRight = eyePosRight;

        eyeballSinSpeedLeft = Random.Range(randomEyeSpeed.x, randomEyeSpeed.y);
        eyeballSinSpeedRight = Random.Range(randomEyeSpeed.x, randomEyeSpeed.y);
    }

    private void Update()
    {
        Vector2 antennaPos = antennaPositionRight;

        if (dir == 1) {
            eyeLeft.flipX = true;
            eyeRight.flipX = true;

            eyePosLeft = leftEyesFacingRightPos;
            eyePosRight = rightEyesFacingRightPos;
            antennaPos = antennaPositionRight;

            sprite.flipX = true;
        }
        else if(dir == -1)
        {
            eyeLeft.flipX = false;
            eyeRight.flipX = false;

            eyePosLeft = leftEyesFacingLeftPos;
            eyePosRight = rightEyesFacingLeftPos;
            antennaPos = antennaPositionLeft;

            sprite.flipX = false;
        }

        leftEyeAntenna.SetPosition(0, new Vector3(eyeLeft.transform.position.x, eyeLeft.transform.position.y));
        rightEyeAntenna.SetPosition(0, new Vector3(eyeRight.transform.position.x, eyeRight.transform.position.y));

        leftEyeAntenna.SetPosition(1, new Vector3(transform.position.x + antennaPos.x, transform.position.y + antennaPos.y));
        rightEyeAntenna.SetPosition(1, new Vector3(transform.position.x + antennaPos.x, transform.position.y + antennaPos.y));

        Vector2 pos = transform.position;
        eyeLeftHolder.transform.position = pos + eyePosLeft;
        eyeRightHolder.transform.position = pos + eyePosRight;

        if (playingDamageAnim) return;

        eyeLeft.transform.localPosition += new Vector3(Mathf.Sin(Time.time * eyeballSinSpeedLeft) * Time.deltaTime * maxEyeSinDistance, Mathf.Cos(Time.time * eyeballSinSpeedLeft) * Time.deltaTime * maxEyeSinDistance, 0);
        eyeRight.transform.localPosition -= new Vector3(Mathf.Sin(Time.time * eyeballSinSpeedRight) * Time.deltaTime * maxEyeSinDistance, Mathf.Cos(Time.time * eyeballSinSpeedRight) * Time.deltaTime * maxEyeSinDistance, 0);

        rb.velocity = new Vector2(speed * dir, rb.velocity.y);

        Vector2 check1Pos = new Vector2(transform.position.x, transform.position.y + 0.25f);
        Vector2 check2Pos = new Vector2(transform.position.x, transform.position.y - 0.15f);

        if(Physics2D.Raycast(check1Pos, Vector2.right * dir * 1.5f, 1.5f, hitWallDetectionMask) || Physics2D.Raycast(check2Pos, Vector2.right * dir * 1.5f, 1.5f, hitWallDetectionMask)) {
            FlipDirection();
        }
        // no ground??
        if(!Physics2D.Raycast(check1Pos + (Vector2.right * dir * 1.2f), Vector2.down * 1.5f, 1.5f, hitWallDetectionMask))
        {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        dir = -dir;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.TryGetComponent<SethPlayerTest>(out SethPlayerTest player))
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (player.transform.position.y > transform.position.y + 0.45)
            {
                // bounce player
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                rb.AddForce(Vector2.up * playerBoostUpForce, ForceMode2D.Impulse);

                Hit();
            }
            else
            {
                player.GetComponent<Health>().TakeDamage(1);

                player.SetJumpFlagTemporarily();
                rb.velocity = new Vector2(0.0f, 0.0f);
                
                rb.AddForce(new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x) * playerHurtForce.x, playerHurtForce.y), ForceMode2D.Impulse);

                Debug.Log(Mathf.Sign(player.transform.position.x - transform.position.x));

                FlipDirection();
            }
        }
    }

    private void Hit()
    {
        health--;

        source.PlayOneShot(crackShellSound);
        CameraShake.instance.Shake(0.15f, 0.2f);
        StartCoroutine(TakeDamage());

        if (health == 2)
        {
            sprite.sprite = shellDamageSprites[0];
        }
        else if (health == 1)
        {
            sprite.sprite = shellDamageSprites[1];
        }
    }

    private IEnumerator TakeDamage()
    {
        rb.velocity = Vector2.zero;
        playingDamageAnim = true;

        eyeLeft.sprite = eyeballLeftHurt;
        eyeRight.sprite = eyeballRightHurt;

        Vector2 leftEyeOrigin = new Vector2(eyePosLeft.x + Random.Range(-0.3f, 0.3f), eyePosLeft.y + Random.Range(-.3f, .3f));
        Vector2 rightEyeOrigin = new Vector2(eyePosRight.x + Random.Range(-.3f, .3f), eyePosRight.y + Random.Range(-.3f, .3f));

        Vector2 leftEyePosition = leftEyeOrigin;
        Vector2 rightEyePosition = rightEyeOrigin;

        float initialShakePower = 1;

        float elapsedTime = 0.0f;
        float duration = 0.5f;
        float shakePower = 2;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float xLeftAmt = Random.Range(-0.2f, 0.2f) * shakePower;
            float yLeftAmt = Random.Range(-0.2f, 0.2f) * shakePower;
            // amt
            float xRightAmt = Random.Range(-0.2f, 0.2f) * shakePower;
            float yRightAmt = Random.Range(-0.2f, 0.2f) * shakePower;

            Vector2 position = transform.position;

            eyeLeft.transform.position = new Vector3(leftEyePosition.x + xLeftAmt + position.x, leftEyePosition.y + yLeftAmt + position.y);
            eyeRight.transform.position = new Vector3(rightEyePosition.x + xRightAmt + position.x, rightEyePosition.y + yRightAmt + position.y);

            shakePower = Mathf.Lerp(initialShakePower, 0.0f, elapsedTime / duration);

            leftEyePosition = Vector2.Lerp(leftEyeOrigin, eyePosLeft, elapsedTime / duration);
            rightEyePosition = Vector2.Lerp(rightEyeOrigin, eyePosRight, elapsedTime / duration);

            yield return null;
        }

        eyeLeft.sprite = eyeballNormal;
        eyeRight.sprite = eyeballNormal;

        if(health <= 0)
        {
            SpawnChildren();

            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(gameObject.transform.parent.gameObject);
        }

        FlipDirection();

        playingDamageAnim = false;
    }

    private void SpawnChildren()
    {
        for(int i = 0; i < childCount; i++)
        {
            SlimeBaby baby = Instantiate(child, transform.position, Quaternion.identity);
            baby.RandomizeDirection();
            Rigidbody2D rb = baby.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(Random.Range(-3, 3), 4.0f), ForceMode2D.Impulse);
        }
    }
}
