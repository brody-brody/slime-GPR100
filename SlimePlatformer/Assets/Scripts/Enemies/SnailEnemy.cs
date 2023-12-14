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
        // set the origin position  to reference later
        originEyePosLeft = eyePosLeft;
        originEyePosRight = eyePosRight;

        // randomize the sin speeds for more variation
        eyeballSinSpeedLeft = Random.Range(randomEyeSpeed.x, randomEyeSpeed.y);
        eyeballSinSpeedRight = Random.Range(randomEyeSpeed.x, randomEyeSpeed.y);
    }

    private void Update()
    {
        UpdateDirection();
        UpdateAntennas();

        Vector2 pos = transform.position;

        // Set the positions of the eye holders 
        eyeLeftHolder.transform.position = pos + eyePosLeft;
        eyeRightHolder.transform.position = pos + eyePosRight;

        if (playingDamageAnim) return;

        UpdateEyePositions();

        rb.velocity = new Vector2(speed * dir, rb.velocity.y);

        // This checks two different positions from the enemy. It does this for accuracy so it can flip directions
        Vector2 check1Pos = new Vector2(transform.position.x, transform.position.y + 0.25f);
        Vector2 check2Pos = new Vector2(transform.position.x, transform.position.y - 0.15f);

        // If either of them hit something in front of the enemy
        if(Physics2D.Raycast(check1Pos, Vector2.right * dir * 1.5f, 1.5f, hitWallDetectionMask) || Physics2D.Raycast(check2Pos, Vector2.right * dir * 1.5f, 1.5f, hitWallDetectionMask)) {
            FlipDirection();
        }
        // no ground?? (sad face)
        if(!Physics2D.Raycast(check1Pos + (Vector2.right * dir * 1.2f), Vector2.down * 1.5f, 1.5f, hitWallDetectionMask))
        {
            FlipDirection();
        }
    }

    /// <summary>
    /// Updates the antenna points
    /// </summary>
    private void UpdateAntennas()
    {
        Vector2 antennaPos = dir == 1 ? antennaPositionRight : antennaPositionLeft;

        // Set the end pos of the left and right eye antenna to the position of the eyes
        leftEyeAntenna.SetPosition(0, new Vector3(eyeLeft.transform.position.x, eyeLeft.transform.position.y));
        rightEyeAntenna.SetPosition(0, new Vector3(eyeRight.transform.position.x, eyeRight.transform.position.y));

        // Set the initial pos to the antenna positions
        leftEyeAntenna.SetPosition(1, new Vector3(transform.position.x + antennaPos.x, transform.position.y + antennaPos.y));
        rightEyeAntenna.SetPosition(1, new Vector3(transform.position.x + antennaPos.x, transform.position.y + antennaPos.y));
    }

    /// <summary>
    /// Updates the sprites for the 21
    /// </summary>
    private void UpdateDirection()
    {
        // Is facing right?
        if (dir == 1)
        {
            // Flip both eye sprites
            eyeLeft.flipX = true;
            eyeRight.flipX = true;

            // Update the target positions for the eyes right positiong
            eyePosLeft = leftEyesFacingRightPos;
            eyePosRight = rightEyesFacingRightPos;

            // Flip the shell
            sprite.flipX = true;
        }

        // Is facing left;
        else if (dir == -1)
        {
            // Flip both eye sprites
            eyeLeft.flipX = false;
            eyeRight.flipX = false;

            // Update target positions for left position
            eyePosLeft = leftEyesFacingLeftPos;
            eyePosRight = rightEyesFacingLeftPos;

            // flip the shell
            sprite.flipX = false;
        }
    }

    /// <summary>
    /// Update the position of the eyeballs to follow a sin function
    /// </summary>
    private void UpdateEyePositions()
    {
        // moves in a circle based on time multiplied by speed
        eyeLeft.transform.localPosition += new Vector3(Mathf.Sin(Time.time * eyeballSinSpeedLeft) * Time.deltaTime * maxEyeSinDistance, Mathf.Cos(Time.time * eyeballSinSpeedLeft) * Time.deltaTime * maxEyeSinDistance, 0);
        eyeRight.transform.localPosition -= new Vector3(Mathf.Sin(Time.time * eyeballSinSpeedRight) * Time.deltaTime * maxEyeSinDistance, Mathf.Cos(Time.time * eyeballSinSpeedRight) * Time.deltaTime * maxEyeSinDistance, 0);
    }

    /// <summary>
    /// Flips the direction 
    /// </summary>
    private void FlipDirection() => dir = -dir;

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if the hit was the player
        if (other.transform.TryGetComponent<SethPlayerTest>(out SethPlayerTest player))
        {
            // get the players rigidbody
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            // did the player hit the top of the shell
            if (player.transform.position.y > transform.position.y + 0.45)
            {
                // bounce player upward
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                rb.AddForce(Vector2.up * playerBoostUpForce, ForceMode2D.Impulse);

                // call the hit function
                Hit();
            }
            else
            {
                // cause the player to take damage
                player.GetComponent<Health>().TakeDamage(1);

                // boost the player backward
                player.SetJumpFlagTemporarily();
                rb.velocity = new Vector2(0.0f, 0.0f);
                rb.AddForce(new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x) * playerHurtForce.x, playerHurtForce.y), ForceMode2D.Impulse);

                FlipDirection();
            }
        }
    }

    private void Hit()
    {
        // decrement health
        health--;

        // play the sound effect when you crack his shell
        source.PlayOneShot(crackShellSound);
        CameraShake.instance.Shake(0.15f, 0.2f);
        StartCoroutine(TakeDamage());

        // set the damage sprite based on the health
        if (health == 2) sprite.sprite = shellDamageSprites[0]; 
        else if (health == 1) sprite.sprite = shellDamageSprites[1];
    }

    private IEnumerator TakeDamage()
    {
        rb.velocity = Vector2.zero;
        playingDamageAnim = true;

        // set the sprites of the eyes to hurt sprites
        eyeLeft.sprite = eyeballLeftHurt;
        eyeRight.sprite = eyeballRightHurt;

        // set the origin of the eyes to a random position within a range (makes the eyes pop around)
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

            // make the left eyes shake
            float xLeftAmt = Random.Range(-0.2f, 0.2f) * shakePower;
            float yLeftAmt = Random.Range(-0.2f, 0.2f) * shakePower;

            // make the right eyes shake
            float xRightAmt = Random.Range(-0.2f, 0.2f) * shakePower;
            float yRightAmt = Random.Range(-0.2f, 0.2f) * shakePower;

            Vector2 position = transform.position;

            // set the eye positions to the determined shake position
            eyeLeft.transform.position = new Vector3(leftEyePosition.x + xLeftAmt + position.x, leftEyePosition.y + yLeftAmt + position.y);
            eyeRight.transform.position = new Vector3(rightEyePosition.x + xRightAmt + position.x, rightEyePosition.y + yRightAmt + position.y);

            // reduce shake power overtime
            shakePower = Mathf.Lerp(initialShakePower, 0.0f, elapsedTime / duration);

            // lerp the eye positions back to their origin pos
            leftEyePosition = Vector2.Lerp(leftEyeOrigin, eyePosLeft, elapsedTime / duration);
            rightEyePosition = Vector2.Lerp(rightEyeOrigin, eyePosRight, elapsedTime / duration);

            yield return null;
        }

        // reset sprites
        eyeLeft.sprite = eyeballNormal;
        eyeRight.sprite = eyeballNormal;

        // kill enemy if dies
        if(health <= 0)
        {
            // spawn the children
            SpawnChildren();

            // instantiate the death particles and destroy self
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(gameObject.transform.parent.gameObject);
        }

        FlipDirection();
        playingDamageAnim = false;
    }

    /// <summary>
    /// Spawns the babiess :DD
    /// </summary>
    private void SpawnChildren()
    {
        // continue until the childs have been birthed
        for(int i = 0; i < childCount; i++)
        {
            // create baby
            SlimeBaby baby = Instantiate(child, transform.position, Quaternion.identity);
            baby.RandomizeDirection();

            
            Rigidbody2D rb = baby.GetComponent<Rigidbody2D>();

            // pop em out (add force)
            rb.AddForce(new Vector2(Random.Range(-3, 3), 4.0f), ForceMode2D.Impulse);
        }
    }
}






















// Hi Eric