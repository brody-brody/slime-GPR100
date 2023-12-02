using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailEnemy : MonoBehaviour
{
    [Header("Renderer References")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private SpriteRenderer eyeLeft;
    [SerializeField] private SpriteRenderer eyeRight;

    [Header("Sprite References")]
    [SerializeField] private Sprite[] shellDamageSprites;
    [SerializeField] private Sprite eyeballNormal;
    [SerializeField] private Sprite eyeballLeftHurt;
    [SerializeField] private Sprite eyeballRightHurt;

    [Header("Eye Position")]
    [SerializeField] private Transform eyePositionLeft;
    [SerializeField] private Transform eyePositionRight;

    [SerializeField] private float eyeLeftPositionSmoothing = 10.5f;
    [SerializeField] private float eyeRightPositionSmoothing = 7.0f;
    [SerializeField] private Vector2 randomEyeSpeed;
    [SerializeField] private float maxEyeSinDistance = 0.2f;

    Vector2 eyeballLeftVel;
    Vector2 eyeballRightVel;

    float eyeballSinSpeedLeft;
    float eyeballSinSpeedRight;

    private void Awake()
    {
        eyeballSinSpeedLeft = Random.Range(randomEyeSpeed.x, randomEyeSpeed.y);
        eyeballSinSpeedRight = Random.Range(randomEyeSpeed.x, randomEyeSpeed.y);
    }

    private void Update()
    {
        // eyeLeft.transform.localPosition = Vector3.Lerp()

        //eyeLeft.transform.parent.transform.position = Vector3.Lerp(eyeLeft.transform.position, eyePositionLeft.transform.position, Time.deltaTime * eyeLeftPositionSmoothing);
        //eyeRight.transform.parent.transform.position = Vector3.Lerp(eyeRight.transform.position, eyePositionRight.transform.position, Time.deltaTime * eyeRightPositionSmoothing);

        eyeLeft.transform.localPosition += new Vector3(Mathf.Sin(Time.time * eyeballSinSpeedLeft) * Time.deltaTime * maxEyeSinDistance, Mathf.Cos(Time.time * eyeballSinSpeedLeft) * Time.deltaTime * maxEyeSinDistance, 0);
        eyeRight.transform.localPosition += new Vector3(Mathf.Sin(Time.time * eyeballSinSpeedRight) * Time.deltaTime * maxEyeSinDistance, Mathf.Cos(Time.time * eyeballSinSpeedRight) * Time.deltaTime * maxEyeSinDistance, 0);
    }
}
