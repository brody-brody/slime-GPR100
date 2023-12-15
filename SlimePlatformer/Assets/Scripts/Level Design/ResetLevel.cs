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

    public bool isSpikes = false;
    private bool isResetting = false;

    // find the camera object
    void Start()
    {
        camera = FindObjectOfType<SimpleCamera>();
        _system = FindObjectOfType<system>();
        player = FindObjectOfType<SethPlayerTest>().gameObject;
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

    public enum DeathType
    {
        Spike,
        Enemy,
        Fall
    }

    // when function is ran, time is slowed to 1/10 speed, the camera is locked in place, and the fallDeath coroutine is ran
    public void Death(DeathType deathType)
    {
        if (isResetting) return;
        isResetting = true;

        GameManager.instance.SetTimeScale(0.1f, 0.01f);
        player.GetComponent<SethPlayerTest>().SuspendAll();
        camera.GetComponent<SimpleCamera>().Lock(true);
        StartCoroutine(fallDeath());

        PlayerAnimationHandler animationHandler = player.GetComponent<PlayerAnimationHandler>();
        switch (deathType)
        {
            case DeathType.Spike:
                animationHandler.CallSpikeDeath();
                break;
            case DeathType.Enemy:
                animationHandler.CallEnemyDeath();
                break;
            case DeathType.Fall:
                animationHandler.CallFallDeath();
                break;
        }
    }

    // Wait two seconds, revert the time scale to normal speed, and then reload the current scene
    IEnumerator fallDeath()
    {
        yield return new WaitForSecondsRealtime(1f);
        GameManager.instance.SetTimeScale(1f, 0.4f);

        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene("Scenes/Levels/" + SceneManager.GetActiveScene().name); 
    }

    // runs Death() function if player enters the death floor trigger
    private void OnTriggerEnter2D(Collider2D other) 
	{
        if (other.transform != player.transform) 
                return;

        if (isSpikes) Death(DeathType.Spike);
        else Death(DeathType.Fall);
    }
}