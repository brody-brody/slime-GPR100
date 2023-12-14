using System;
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
    public GameObject UIParent;
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
        if (other.transform != player.transform) 
                return;
        levelText.SetText(gameObject.name);

        UIParent.transform.position = transform.position;
        UIParent.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform != player.transform) 
                return;
        UIParent.SetActive(false);
    }
}
