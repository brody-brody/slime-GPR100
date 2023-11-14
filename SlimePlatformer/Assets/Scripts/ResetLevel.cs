using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class ResetLevel : MonoBehaviour
{
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
        SceneManager.LoadScene("Scenes/" + SceneManager.GetActiveScene().name); 
    }
}