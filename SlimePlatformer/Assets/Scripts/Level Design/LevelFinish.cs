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
    private bool levelExists;


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
        int newCoins = 0;

        if(playerTouchedMe) 
            return;
        // derp
        if (other.transform != player.transform) 
            return;
        playerTouchedMe = true;
        playerMovement.enabled = false;
        finishMenu.SetActive(true);
        playerMovement.GetComponent<SethPlayerTest>().SuspendAll();
        StartCoroutine(ShowCoins());
        gameManager.OnFinishLevel();

        LevelData.LevelStorage levelComplete = new LevelData.LevelStorage(SceneManager.GetActiveScene().name, gameManager.coins);
        
        Debug.Log(SceneManager.GetActiveScene().name + " saved with " + gameManager.coins + " coins");
        
        levelExists = false;
        foreach (var data in LevelData.levelInfo)
        {
            if (data.levelName == SceneManager.GetActiveScene().name)
            {
                if (gameManager.coins > data.coins)
                {
                    newCoins = gameManager.coins - data.coins;
                    Debug.Log("player collected " + newCoins + " new coins this level\n" + gameManager.coins + " gm\n" + data.coins +" data");
                    data.coins = gameManager.coins;
                }
                levelExists = true;
            }
        }
        if (!levelExists)
        {
            LevelData.levelInfo.Add(levelComplete);
            LevelData.totalCoins += gameManager.coins;
            Debug.Log(LevelData.totalCoins + " coins total");
        }
        else if (levelExists)
        {
            LevelData.totalCoins += newCoins;
            Debug.Log(LevelData.totalCoins + " coins total");
        }
    }

    // wait 2 seconds, show first coin if collected with camera shake, wait a second, etc etc
    IEnumerator ShowCoins()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (gameManager.coins >= 1)
        {
            coin1.enabled = true;
            CameraShake.instance.Shake(0.2f, 0.2f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        if (gameManager.coins >= 2)
        {
            coin2.enabled = true;
            CameraShake.instance.Shake(0.2f, 0.2f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        if (gameManager.coins >= 3)
        {
            coin3.enabled = true;
            CameraShake.instance.Shake(0.2f, 0.2f);
        }
    }

}
