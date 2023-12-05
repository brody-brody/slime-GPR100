using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject player;
    public GameObject pauseMenu;
    public static bool paused;
    private SethPlayerTest playerMovement;

    void Start()
    {
        playerMovement = player.GetComponent<SethPlayerTest>();
        paused = false;
    }

    // if not paused, pause, if paused, not pause
    public void Pause()
    {
        if (!paused)
        {
            pauseMenu.SetActive(true);
            playerMovement.GetComponent<SethPlayerTest>().SuspendAll();
        }
        if (paused)
        {
            pauseMenu.SetActive(false);
            playerMovement.GetComponent<SethPlayerTest>().UnsuspendAll();
        }
        paused = !paused;
    }
}
