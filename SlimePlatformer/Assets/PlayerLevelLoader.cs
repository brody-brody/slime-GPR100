using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevelLoader : MonoBehaviour
{
    public string scene;
    public bool collision = false;

    public TMP_Text enterText;
    // Start is called before the first frame update
    void Start()
    {
      //enterText = GetComponentInParent<TextMeshProUGUI>();
      enterText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (collision)
        {
            enterText.enabled = true;
            if (Input.GetKey(KeyCode.W))
            {
                SceneManager.LoadScene(scene);
            }
        } else
        {
            enterText.enabled = false;
        }
    }
}
