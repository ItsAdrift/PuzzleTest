using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        CameraMovement cam = FindObjectOfType<CameraMovement>();
        cam.ClearTarget();
        Vector3 pos = level.cameraPosition.position;
        pos.z = -10;
        cam.transform.position = pos;

        level.JumpTo();
        MenuManager.Instance.DeactivateAll();

        MainMenuController.OnLevelLoad.Invoke();
    }

}
