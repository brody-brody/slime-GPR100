using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLevelLoader : MonoBehaviour
{
    public string scene;
    public bool collision = false;

    public TMP_Text enterText;
    public TMP_Text levelText;
    public GameObject UIParent, coin1, coin2, coin3;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
      //enterText = GetComponentInParent<TextMeshProUGUI>();
      enterText.enabled = false;
      UIParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (collision)
        {
            enterText.enabled = true;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                SceneManager.LoadScene(scene);
            }
        } 
        else
        {
            enterText.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string test, testName;
        if (other.transform != player.transform) 
                return;
        levelText.SetText(gameObject.name);

        UIParent.transform.position = transform.position;
        UIParent.SetActive(true);

        coin1.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,15);
        coin2.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,15);
        coin3.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,15);

        foreach (var LevelData in LevelData.levelInfo)
        {
            test = gameObject.name;
            testName = test.Replace(" ", "");
            Debug.Log(test);
            if (LevelData.levelName == testName)
            {
                if (LevelData.coins == 0)
                {
                    coin1.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,15);
                    coin2.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,15);
                    coin3.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,15);
                }
                else if (LevelData.coins == 1)
                {
                    coin1.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
                    coin2.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,15);
                    coin3.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,15);
                }
                else if (LevelData.coins == 2)
                {
                    coin1.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
                    coin2.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
                    coin3.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,15);
                }
                else if (LevelData.coins == 3)
                {
                    coin1.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
                    coin2.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
                    coin3.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform != player.transform) 
                return;
        UIParent.SetActive(false);
    }
}
