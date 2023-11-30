using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 smoothTime;

    Vector3 vel;

    void Update()
    {
        Vector3 currentPos = transform.position;
        Vector3 t = new Vector3(target.transform.position.x, target.transform.position.y, -10);

        float x = Mathf.SmoothDamp(currentPos.x, t.x, ref vel.x, smoothTime.x);
        float y = Mathf.SmoothDamp(currentPos.y, t.y, ref vel.y, smoothTime.y);
        Vector3 t2 = new Vector3(x, y, -10);

        transform.position = t2;
    }
}
