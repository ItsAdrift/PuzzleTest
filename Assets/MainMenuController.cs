using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static UnityEvent OnLevelLoad;

    // Start is called before the first frame update
    void Start()
    {
        if (OnLevelLoad == null)
            OnLevelLoad = new UnityEvent();

        LoadGameScene();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}
