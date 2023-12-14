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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    
    }


    public void sceneSwitch()
    {
        Debug.Log("Scene Switched");
        
    }

    public void goToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
