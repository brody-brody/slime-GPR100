using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public static bool paused;

    void Start()
    {
        paused = false;
    }

    // if not paused, pause, if paused, not pause
    public void Pause()
    {
        if (!paused)
        {
            pauseMenu.SetActive(true);
        }
        if (paused)
        {
            pauseMenu.SetActive(false);
        }
        paused = !paused;
    }
}
