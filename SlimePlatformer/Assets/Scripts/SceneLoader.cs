using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public delegate void LoadScene(int scene);
    static public LoadScene loadScene;

    /// <summary>
    ///Loads Scene passed through. Can be called with SceneLoader.loadScene?.Invoke(scene_number);
    /// </summary>
    /// <param name="scene"></param>
    private void PlayScene(int scene)
    {
        //Load passed through scene number
        SceneManager.LoadScene(scene);
    }

    //To Prevent the delegate from being called twice
    private void OnEnable()
    {
        loadScene += PlayScene;
    }
    private void OnDisable()
    {
        loadScene -= PlayScene;
    }
}
