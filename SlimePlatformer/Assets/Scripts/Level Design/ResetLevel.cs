using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 

public class ResetLevel : MonoBehaviour
{
    public GameObject player;

    // constantly testing if the player hits the r key, and runs Restart() if they do
    void Update()
    {
        if (Input.GetKey(KeyCode.R)) {
            Restart();
        }
    }
 
    // reloads current scene if function is ran
    void Restart()
    {
        SceneManager.LoadScene("Scenes/Levels/" + SceneManager.GetActiveScene().name); 
    }

    // reloads current scene if player enters the death floor trigger
    private void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.transform != player.transform) 
                return;
        Debug.Log("skill issue");
        Restart();
    }
}