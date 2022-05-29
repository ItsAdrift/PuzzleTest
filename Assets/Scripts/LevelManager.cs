using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Level[] levels;

    private void Start()
    {
        instance = this;
    }

    public Level[] GetLevels()
    {
        return levels;
    }

    public Level FindLevel(int level)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].level == level)
            {
                return levels[i];
            }
        }

        return null;
    }

}
