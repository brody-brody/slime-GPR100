using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    [SerializeField] private Vector2 constraints;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;

    Vector3 vel;

    void Update()
    {
        Vector3 currentPos = transform.position;
        Vector3 t = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        Vector3 t2 = Vector3.SmoothDamp(currentPos, t, ref vel, smoothTime);

        transform.position = t2;
    }
}
