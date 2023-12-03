using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public delegate void DeathScreen();
    public static DeathScreen enableScreen;

    [SerializeField] private GameObject deathScreen;
    private void EnableDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    private void OnEnable()
    {
        enableScreen += EnableDeathScreen;
    }
    private void OnDisable()
    {
        enableScreen -= EnableDeathScreen;
    }
}
