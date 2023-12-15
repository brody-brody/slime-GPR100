using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCoinUI : MonoBehaviour
{
    public TMP_Text numCoins;
    public GameObject coinWallA;
    public GameObject coinWallB;
    public GameObject coinWallC;

    void Update()
    {
        if(LevelData.totalCoins >= 10) 
        { 
            openWallA(); 
        }
        if(LevelData.totalCoins >= 20) 
        { 
            openWallB(); 
        }
        if(LevelData.totalCoins >= 30) 
        { 
            openWallC(); 
        }
        numCoins.SetText((LevelData.totalCoins).ToString());
    }
    
    void openWallA()
    {
        coinWallA.SetActive(false);
    }
    void openWallB()
    {
        coinWallB.SetActive(false);
    }
    void openWallC()
    {
        coinWallC.SetActive(false);
    }
}
