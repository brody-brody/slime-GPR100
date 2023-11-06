using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class ResetLevel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("r")) {
            Restart();
        }
    }
 
    void Restart()
    {
        SceneManager.LoadScene("Scenes/" + SceneManager.GetActiveScene().name); 
    }
}