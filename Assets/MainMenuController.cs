using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static UnityEvent OnLevelLoad = new UnityEvent();
    public static UnityEvent OnOptionsChange = new UnityEvent();

    // Start is called before the first frame update
    void Awake()
    {
        /*if (OnLevelLoad == null)
            OnLevelLoad = new UnityEvent();*/

        LoadGameScene();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void Quit()
    {
        Application.Quit();
        //EditorApplication.ExitPlaymode();
    }

}
