using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    [SerializeField] GameObject titleCage;
    [SerializeField] GameObject levelCage;
    [SerializeField] GameObject levelTextCage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    
    }
    public void Quit()
    {
        Application.Quit();
    }

}
