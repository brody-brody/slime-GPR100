using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class system : MonoBehaviour
{
    // Example variable to transfer
    public int level = 0;
    public GameObject player;
    public GameObject loadingScreen;
    public float spawnX;
    public float spawnY;
    private system _system;
    private void Start()
    {
        _system = FindObjectOfType<system>();
       if(SceneManager.GetActiveScene().name == "LevelSelect")
        {
            loadingScreen.SetActive(true);
            LoadVariable();
            switch (level)
            {
                case 0:
                    spawnX = (float)-1.55;
                    spawnY = (float)-1.46;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 1:
                    spawnX = (float)9.96;
                    spawnY = (float)0.58;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 2:
                    spawnX = (float)27.21;
                    spawnY = (float)-7.67;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 3:
                    spawnX = (float)16.23;
                    spawnY = (float)-15.43;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 4:
                    spawnX = (float)44;
                    spawnY = (float)-12.56;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 5:
                    spawnX = (float)68.98;
                    spawnY = (float)-19.31;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 6:
                    spawnX = (float)83.1;
                    spawnY = (float)-16.36;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 7:
                    spawnX = (float)90;
                    spawnY = (float)-17.37;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 8:
                    spawnX = (float)96.98;
                    spawnY = (float)-16.44;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 9:
                    spawnX = (float)107.99;
                    spawnY = (float)-8.53;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 10:
                    spawnX = (float)98.76;
                    spawnY = (float)-0.15;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 11:
                    spawnX = (float)103.72;
                    spawnY = (float)15.67;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 12:
                    spawnX = (float)95.5;
                    spawnY = (float)24.51;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 13:
                    spawnX = (float)85.18;
                    spawnY = (float)16.6;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 14:
                    spawnX = (float)72.85;
                    spawnY = (float)24.51;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
                case 15:
                    spawnX = (float)70.45;
                    spawnY = (float)-0.47;
                    player.transform.position = new Vector3(spawnX, spawnY, 0);
                    break;
            }
            StartCoroutine(LoadBuffer());
            Debug.Log("Set Player SpawnPoint");
        }
    }
    void OnApplicationQuit()
    {
        Debug.Log("Quitting the game. Resetting PlayerPrefs.");

        // Reset or clear PlayerPrefs as needed
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }



    // Function to save the variable to PlayerPrefs
    public void SaveVariable()
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();
        Debug.Log("SAVED LEVEL");
    }

    public void setLevel(int _level)
    {
        level = _level;
    }

    // Function to load the variable from PlayerPrefs
    public void LoadVariable()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }
    }

    // Function to clear the variable from PlayerPrefs
    public void ClearVariable()
    {
        PlayerPrefs.DeleteKey("Level");
    }

    // Function to load the next level
    public void LoadNextLevel(string level)
    {
        SaveVariable(); // Save the variable before loading the next level
        SceneManager.LoadScene(level);
    }

    IEnumerator LoadBuffer()
    {
        yield return new WaitForSeconds(0.7f);
        loadingScreen.SetActive(false);
    }
}