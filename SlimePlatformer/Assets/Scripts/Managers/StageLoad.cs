using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StageLoad : MonoBehaviour
{
    public GameObject player;
    public GameObject system;
    public string scene;
    // Start is called before the first frame update

    private PlayerLevelLoader playerLevelLoader;
    private system _system;
    void Start()
    {
       playerLevelLoader = player.GetComponent<PlayerLevelLoader>();
       _system = FindObjectOfType<system>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform != player.transform)
            return;
        playerLevelLoader.collision = true;
        playerLevelLoader.scene = scene;
        switch (scene)
        {
            case "Level1":
                _system.level = 1;
                break;
            case "Level2":
                _system.level = 2;
                break;
            case "Level3":
                _system.level = 3;
                break;
            case "Level4":
                _system.level = 4;
                break;
            case "Level5":
                _system.level = 5;
                break;
            case "Level6":
                _system.level = 6;
                break;
            case "Level7":
                _system.level = 7;
                break;
            case "Level8":
                _system.level = 8;
                break;
            case "Level9":
                _system.level = 9;
                break;
            case "Level10":
                _system.level = 10;
                break;
            case "Level11":
                _system.level = 11;
                break;
            case "Level12":
                _system.level = 12;
                break;
            case "Level13":
                _system.level = 13;
                break;
            case "Level14":
                _system.level = 14;
                break;
            case "Level15":
                _system.level = 15;
                break;
        }
        _system.SaveVariable();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform != player.transform)
            return;
        playerLevelLoader.collision = false;
        playerLevelLoader.scene = "";
    }
}
