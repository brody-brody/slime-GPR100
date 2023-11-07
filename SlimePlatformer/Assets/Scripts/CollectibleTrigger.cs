using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleTrigger : MonoBehaviour
{
	int coins = 0;
	public Image coin1, coin2, coin3;
    
	private void OnTriggerEnter2D(Collider2D other) 
	{
		Debug.Log("OnTriggerEnter2D");
		if (coins == 0)
		{
			coin1.color = new Color32(255,255,0,100);
		}
		else if (coins == 1)
		{
			coin2.color = new Color32(255,255,0,100);
		}
		else if (coins == 2)
		{
			coin3.color = new Color32(255,255,0,100);
		}
		Destroy(gameObject);
	}
	
}
