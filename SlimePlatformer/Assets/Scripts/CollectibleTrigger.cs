using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleTrigger : MonoBehaviour
{
	// initializing # of coins and images of coins present on canvas
	public GameManager gameManager;
	public Image coin1, coin2, coin3;

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
			coin1.color = new Color32(255,255,0,100);
		}
		else if (gameManager.coins == 1)
		{
			gameManager.coins++;
			coin2.color = new Color32(255,255,0,100);
		}
		else if (gameManager.coins == 2)
		{
			gameManager.coins++;
			coin3.color = new Color32(255,255,0,100);
		}
		Destroy(gameObject);
	}
	
}
