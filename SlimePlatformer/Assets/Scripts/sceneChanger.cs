using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject titleCage;
    [SerializeField] GameObject levelCage;
    [SerializeField] GameObject levelTextCage;
    [SerializeField] GameObject level1;
    [SerializeField] GameObject level2;
    [SerializeField] GameObject level3;
    [SerializeField] GameObject level4;
    [SerializeField] GameObject level5;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Debug.Log("Clicked with Mouse");
            Debug.Log("X: " + mousePos.x + " Y: " + mousePos.y);
            if ((mousePos.x < 230) && (mousePos.x > 190) && (mousePos.y > 229) && (mousePos.y < 252))
            {
                SceneManager.LoadScene("Level1");
                Debug.Log("LEVEL 1");
            }
            if ((mousePos.x < 386) && (mousePos.x > 336) && (mousePos.y > 253) && (mousePos.y < 280))
            { 
                SceneManager.LoadScene("Level2");
                Debug.Log("LEVEL 2");
            }
            if ((mousePos.x < 546) && (mousePos.x > 497) && (mousePos.y > 276) && (mousePos.y < 308))
            {
                SceneManager.LoadScene("Level3");
                Debug.Log("LEVEL 3");
            }
            if ((mousePos.x < 711) && (mousePos.x > 665) && (mousePos.y > 248) && (mousePos.y < 274))
            {
                SceneManager.LoadScene("Level4");
                Debug.Log("LEVEL 4");
            }
            if ((mousePos.x < 713) && (mousePos.x > 665) && (mousePos.y > 154) && (mousePos.y < 177))
            {
                SceneManager.LoadScene("Level5");
                Debug.Log("LEVEL 5");
            }
            if ((mousePos.x < 230) && (mousePos.x > 190) && (mousePos.y > 229) && (mousePos.y < 252))
            {
                //SceneManager.LoadScene("Level1");
                //Debug.Log("LEVEL 6");
            }
        }
    }


    public void sceneSwitch()
    {
        Debug.Log("Scene Switched");
        SceneManager.LoadScene("TutorialLevel");
    }

    public void goToLevelSelect()
    {
        titleCage.SetActive(false);
        levelCage.SetActive(true);
        levelTextCage.SetActive(true);
    }
}
