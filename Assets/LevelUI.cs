using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public TMP_Text text;
    public GameObject padlock;

    public int levelNumber;
    public Level level;

    bool unlocked = false;

    public void Init()
    {
        unlocked = PlayerPrefs.HasKey("level_" + level.level);

        if (unlocked)
            text.gameObject.SetActive(true);
        else
            padlock.gameObject.SetActive(true);

        text.text = ""+levelNumber;
    }

    public void OnClick()
    {
        if (!unlocked)
            return;

        level.JumpTo();
        MenuManager.Instance.DeactivateAll();
    }

}
