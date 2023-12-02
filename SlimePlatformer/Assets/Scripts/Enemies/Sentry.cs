using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    track,
    rotate,
}
public class Sentry : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform spawnPoint;

    private Projectile currentProjectile;
    [SerializeField] private GameObject bullet;

    [SerializeField] private Type currentType;

    private ParticleSystem particle;

    [Header("Sentry Settings")]
    [SerializeField] private float detectionRange = 4;
    [SerializeField] private float fireRate = 3;
    [SerializeField] private float rotationSpeed = 10;


    private readonly float rotationModifier = 180;
    private float fireRateTime = 3;
    private float remainingDistance;

    private bool playerDetected;

    // Start is called before the first frame update
    void Start()
    {
        //Scalable so you don't have to change anything within script
        fireRateTime = fireRate;

        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Find distance between player and enemy
        remainingDistance = Vector2.Distance(player.position, transform.position);

        //Player Detected Variable
        if (remainingDistance < detectionRange)
            playerDetected = true;
        else
            playerDetected = false;
        
        //Switch between enemy behaviors within inspector
        switch (currentType)
        {
            //Track Player
            case Type.track:

                //Find target Vector3
                Vector3 target = (player.position - transform.position);

                //Return to default position
                if (!playerDetected)
                    target = Vector3.down;

                //We find the angle between the x-axis and our vector. Then we convert the radians into degrees
                //and subtract it by our rotation modifier to have it properly face the player.
                float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - rotationModifier;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                //Fires Projectile
                if (playerDetected)
                    Fire();
                break;

            //Constantly Rotate
            case Type.rotate:
                gameObject.transform.Rotate(0, 0, rotationSpeed / 10);
                Fire();
                break;
        }
    }
    /// <summary>
    /// Instantiates Projectile from origin point
    /// </summary>
    void Fire()
    {
        Debug.DrawLine(transform.position, player.position);
        fireRateTime -= Time.deltaTime;
        if (fireRateTime < 0)
        {
            currentProjectile = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation).GetComponent<Projectile>();
            currentProjectile.target = spawnPoint.up;
            Debug.DrawRay(spawnPoint.position, spawnPoint.up * 3, Color.cyan, 5f);
            fireRateTime = fireRate;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            SelfDestruct();
        }
    }
    void SelfDestruct()
    {
        particle.Play();
        //Add in delay at some point so particle can play
        Destroy(gameObject);
    }
}
