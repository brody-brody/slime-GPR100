using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StageLoad : MonoBehaviour
{
    public GameObject player;
    public string scene;
    // Start is called before the first frame update

    private PlayerLevelLoader playerLevelLoader;
    void Start()
    {
       playerLevelLoader = player.GetComponent<PlayerLevelLoader>();
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
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform != player.transform)
            return;
        playerLevelLoader.collision = false;
        playerLevelLoader.scene = "";
    }
}
