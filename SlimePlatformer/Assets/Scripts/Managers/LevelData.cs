using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelData
{
    public class LevelStorage {
        public string levelName;
        public int coins;

        public LevelStorage(string levelName, int coins)
        {
            this.levelName = levelName;
            this.coins = coins;
        }
    }

    public static List<LevelStorage> levelInfo = new List<LevelStorage>();
}
