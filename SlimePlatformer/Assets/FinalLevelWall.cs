using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalLevelWall : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject finalDoor;
    public bool doorOpen = false;

    void Update()
    {
        if (!doorOpen)
        {
            if (gameManager.coins == 3)
            {
                doorOpen = true;
                finalDoor.SetActive(false);
                CameraShake.instance.Shake(0.2f, 3f);
            }
        }
    }
}
