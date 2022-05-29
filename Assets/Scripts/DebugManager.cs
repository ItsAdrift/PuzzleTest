using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool debug = true;

    public void UnlockAllLevels()
    {
        LevelManager l = FindObjectOfType<LevelManager>();
        for (int i = 0; i < l.levels.Length; i++)
        {
            PlayerPrefs.SetInt("level_" + l.levels[i].level, 1);
        }
    }

    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("level_1", 1);
    }
}
