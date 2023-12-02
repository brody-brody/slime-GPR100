using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFinish : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public GameObject finishMenu;
    public Image coin1, coin2, coin3;
    private SethPlayerTest playerMovement;

    private bool playerTouchedMe;


    // on start, grab player movement script, set the players coins to 0, and set the finish flag to false
    private void Start() {
        playerMovement = player.GetComponent<SethPlayerTest>();
        coin1.enabled = false;
        coin2.enabled = false;
        coin3.enabled = false;
        playerTouchedMe = false;
    }

    // on collision with the end flag, disable movement, enable finish menu, suspend velocity, and start coroutine ShowCoins
    private void OnTriggerEnter2D(Collider2D other) 
	{
        if(playerTouchedMe) return;

        if (other.transform != player.transform) return;
        playerTouchedMe = true;
        playerMovement.enabled = false;
        finishMenu.SetActive(true);
        playerMovement.GetComponent<SethPlayerTest>().SuspendAll();
        StartCoroutine(ShowCoins());
    }

    // wait 2 seconds, show first coin if collected with camera shake, wait a second, etc etc
    IEnumerator ShowCoins()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (gameManager.coins >= 1)
        {
            coin1.enabled = true;
            CameraShake.instance.Shake(0.2f, 0.2f);
        }
        yield return new WaitForSecondsRealtime(1f);
        if (gameManager.coins >= 2)
        {
            coin2.enabled = true;
            CameraShake.instance.Shake(0.2f, 0.2f);
        }
        yield return new WaitForSecondsRealtime(1f);
        if (gameManager.coins >= 3)
        {
            coin3.enabled = true;
            CameraShake.instance.Shake(0.2f, 0.2f);
        }
    }

}
