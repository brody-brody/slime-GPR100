using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 

public class ResetLevel : MonoBehaviour
{
    public GameObject player;
    private SimpleCamera camera;
    private system _system;

    // find the camera object
    void Start()
    {
        camera = FindObjectOfType<SimpleCamera>();
        _system = FindObjectOfType<system>();
    }
    // constantly testing if the player hits the r key, and runs Restart() if they do
    void Update()
    {
        if (Input.GetKey(KeyCode.R)) {
            Restart();
        }
        if (Input.GetKey(KeyCode.Escape)) {
            _system.GetComponent<system>().ClearVariable();
            SceneManager.LoadScene("MenuScene");
        }
    }
 
    // reloads current scene if function is ran
    void Restart()
    {
        SceneManager.LoadScene("Scenes/Levels/" + SceneManager.GetActiveScene().name); 
        GameManager.instance.SetTimeScale(1f, 0.5f);
    }

    // when function is ran, time is slowed to 1/10 speed, the camera is locked in place, and the fallDeath coroutine is ran
    public void Death()
    {
        GameManager.instance.SetTimeScale(0.1f, 0.01f);
        camera.GetComponent<SimpleCamera>().Lock(true);
        StartCoroutine(fallDeath());
    }

    // Wait two seconds, revert the time scale to normal speed, and then reload the current scene
    IEnumerator fallDeath()
    {
        yield return new WaitForSecondsRealtime(2f);
        GameManager.instance.SetTimeScale(1f, 0.01f);
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene("Scenes/Levels/" + SceneManager.GetActiveScene().name); 
        yield return new WaitForSecondsRealtime(0.2f);
        GameManager.instance.SetTimeScale(1f, 0.2f);
        
    }

    // runs Death() function if player enters the death floor trigger
    private void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.transform != player.transform) 
                return;
        Death();
    }
}