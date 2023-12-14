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
    
    void Update()
    {
        numCoins.SetText((LevelData.totalCoins).ToString());
    }
}
