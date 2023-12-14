using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class newGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button button;
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
        SceneManager.LoadScene("TutorialLevel");
    }
}
