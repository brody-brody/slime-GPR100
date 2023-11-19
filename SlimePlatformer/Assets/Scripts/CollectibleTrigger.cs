using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleTrigger : MonoBehaviour
{
	// initializing # of coins and images of coins present on canvas
	public GameManager gameManager;


	private void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
	}
    
	// on collision with coin
	private void OnTriggerEnter2D(Collider2D other) 
	{
		// remove coin from scene, and then change the color of one of the canvas images
		// to properly represent number of coins collected.
		if (gameManager.coins == 0)
		{
			gameManager.coins++;
		}
		else if (gameManager.coins == 1)
		{
			gameManager.coins++;
		}
		else if (gameManager.coins == 2)
		{
			gameManager.coins++;
		}
		Destroy(gameObject);
	}
	
}
