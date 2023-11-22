using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    public GameObject player;
    private UpdatedPlayerMovement playerMovement;

    private void Start() {
        playerMovement = player.GetComponent<UpdatedPlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.transform != player.transform) 
                return;
        Debug.Log("finish");
        playerMovement.enabled = false;
    }

}
