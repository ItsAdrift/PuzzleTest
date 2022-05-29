using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    public LevelManager levelManager;

    public Transform levelHolder;
    public GameObject levelPrefab;

    private void OnEnable()
    {
        levelManager = FindObjectOfType<LevelManager>();
        Regen();
    }

    public void Generate()
    {
        for (int i = 0; i < levelManager.levels.Length; i++)
        {
            if (levelManager.levels[i].level <= 0)
                continue;
            GameObject obj = Instantiate(levelPrefab, levelHolder);
            LevelUI ui = obj.GetComponent<LevelUI>();
            ui.level = levelManager.levels[i];
            ui.levelNumber = i -2;
            ui.Init();
        }
    }

    public void Clear()
    {
        for (int i = 0; i < levelHolder.childCount; i++)
        {
            Destroy(levelHolder.GetChild(i).gameObject);
        }
    }

    public void Regen()
    {
        Clear();
        Generate();
    }
}
