using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squish : MonoBehaviour
{
    public GameObject player;
    
    void OnCollisionEnter(Collision collision)
    {
        // To be implemented
        /*
        if (collision.player.layer == 8)
        {
            player.transform.position = Vector3.Lerp (oldPos, oldPos + 5, Time.deltaTime * 2.0f);
            player.transform.position = newPos;
        }
        */
    }
}
