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

    [SerializeField] private float eyePositionSmoothing;
    [SerializeField] private Vector2 randomEyeSpeed;

    private void Update()
    {
       // eyeLeft.transform.localPosition = Vector3.Lerp()
    }
}
