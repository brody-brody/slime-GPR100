using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleTrigger : MonoBehaviour
{
	// initializing # of coins and images of coins present on canvas
	int coins = 0;
	public Image coin1, coin2, coin3;
    
	// on collision with coin
	private void OnTriggerEnter2D(Collider2D other) 
	{
		// remove coin from scene, and then change the color of one of the canvas images
		// to properly represent number of coins collected.
		if (coins == 0)
		{
			coins++;
			coin1.color = new Color32(255,255,0,100);
		}
		else if (coins == 1)
		{
			coins++;
			coin2.color = new Color32(255,255,0,100);
		}
		else if (coins == 2)
		{
			coins++;
			coin3.color = new Color32(255,255,0,100);
		}
		Destroy(gameObject);
	}
	
}
