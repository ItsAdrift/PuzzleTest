using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public int levelNumber;
    public Level level;

    public TMP_Text text;

    public void Init()
    {
        text.text = ""+levelNumber;
    }

    public void OnClick()
    {
        level.JumpTo();
        MenuManager.Instance.DeactivateAll();
    }

}
