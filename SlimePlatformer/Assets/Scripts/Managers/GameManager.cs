using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioSource coinSource;


    public Transform player;
    // this will handle all the game stuff - ssseth triidnelssfdale
    public int coins = 0;

    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<SethPlayerTest>().transform;
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void AddCoin()
    {
        coins++;

        if (!coinSource) {
            Debug.Log("so there's no audio source on the game manager.");
            return;
        }
        coinSource.PlayOneShot(coinSound);
    }

    /// <summary>
    /// This is responsible for calling coroutine responsible for lerping timescale. 
    /// </summary>
    /// <param name="newTimeScale"></param>
    /// <param name="time"></param>
    public void SetTimeScale(float newTimeScale, float time)
    {
        StopAllCoroutines();
        StartCoroutine(TimeChange(newTimeScale, time));
    }

    /// <summary>
    /// Lerps the timescale from its initial time to the newTimeScale variable.
    /// </summary>
    /// <param name="newTimeScale"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator TimeChange(float newTimeScale, float time)
    {
        float elapsedTime = 0.0f;
        float initialTimeScale = Time.timeScale;

        while (elapsedTime < time)
        {
            Time.timeScale = Mathf.Lerp(initialTimeScale, newTimeScale, elapsedTime / time);
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = newTimeScale;

        yield return null;
    }
}
