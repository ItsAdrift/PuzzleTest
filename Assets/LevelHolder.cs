using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    public LevelManager levelManager;

    public Transform levelHolder;
    public GameObject levelPrefab;

    private void Start()
    {
        for (int i = 0; i < levelManager.levels.Length; i++)
        {
            GameObject obj = Instantiate(levelPrefab, levelHolder);
            LevelUI ui = obj.GetComponent<LevelUI>();
            ui.level = levelManager.levels[i];
            ui.levelNumber = i+1;
            ui.Init();
        }
    }
}
