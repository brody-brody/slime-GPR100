using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squish : MonoBehaviour
{
    public GameObject player;
    
    
    private void OnTriggerEnter2D(Collider2D other) 
	{
        Debug.Log("Tunnel trigger");
        player.transform.localScale = new Vector3(1, 0.5f, 1);
        player.transform.position += new Vector3(0, -0.25f, 0);
    }
}
