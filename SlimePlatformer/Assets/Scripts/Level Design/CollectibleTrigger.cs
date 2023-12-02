using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleTrigger : MonoBehaviour
{
	// initializing # of coins and images of coins present on canvas
	public GameManager gameManager;
	public Image coin1, coin2, coin3;

	// automatically set UI coins into inspector slots
	private void Start()
	{
		coin1 = GameObject.Find("Coin1UI").GetComponent<Image>();
		coin2 = GameObject.Find("Coin2UI").GetComponent<Image>();
		coin3 = GameObject.Find("Coin3UI").GetComponent<Image>();

		gameManager = FindObjectOfType<GameManager>();
	}
    
	// on collision with coin, set opacity of UI coin to 100%, and get rid of the coin on the map
	private void OnTriggerEnter2D(Collider2D other) 
	{
		// remove coin from scene, and then change the color of one of the canvas images
		// to properly represent number of coins collected.
		if (gameManager.coins == 0)
		{
			gameManager.coins++;
			coin1.GetComponent<Image>().color = new Color32(255,255,255,255);
		}
		else if (gameManager.coins == 1)
		{
			gameManager.coins++;
			coin2.GetComponent<Image>().color = new Color32(255,255,255,255);
		}
		else if (gameManager.coins == 2)
		{
			gameManager.coins++;
			coin3.GetComponent<Image>().color = new Color32(255,255,255,255);
		}
		Destroy(gameObject);
	}
	
}
