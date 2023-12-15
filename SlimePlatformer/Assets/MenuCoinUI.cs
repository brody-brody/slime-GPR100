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

    static bool wall_A_opened = false;
    static bool wall_B_opened = false;
    static bool wall_C_opened = false;
    static bool addedCoin = false;

    void Update()
    {
        if(addedCoin == false)
        {
            LevelData.totalCoins = 29;
            addedCoin = true;
        }
        numCoins.SetText((LevelData.totalCoins).ToString());
        if(LevelData.totalCoins >= 10 && wall_A_opened == false) { openWallA(); }
        if(LevelData.totalCoins >= 20 && wall_B_opened == false) { openWallB(); }
        if(LevelData.totalCoins >= 30 && wall_C_opened == false) { openWallC(); }
    }
    
    void openWallA()
    {
        coinWallA.SetActive(false);
        wall_A_opened = true;
    }
    void openWallB()
    {
        coinWallB.SetActive(false);
        wall_B_opened = true;
    }
    void openWallC()
    {
        coinWallC.SetActive(false);
        wall_C_opened = true;
    }
}
