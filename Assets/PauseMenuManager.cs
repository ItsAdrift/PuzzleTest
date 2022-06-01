using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;
    public Menu[] menus;

    [HideInInspector] public bool paused = false;

    // Pause Menu Methods

    public void Pause()
    {
        // Set the 'paused' boolean to true
        paused = true;
        // Set Unity's 'timeScale' to 0.
        Time.timeScale = 0;

        // Disable the PlayerMovement script
        FindObjectOfType<PlayerMovement>().isEnabled = false;
        // Activate the Pause Menu
        ActivateMenu("main");
    }

    public void Resume()
    {
        // Set the 'paused' boolean to false
        paused = false;
        // Set Unity's 'timeScale' back to 1
        Time.timeScale = 1;

        // Reenable the PlayerMovement Script
        FindObjectOfType<PlayerMovement>().isEnabled = true;

        // Deactivate all paused-related menus
        DeactivateAll();
    }

    public void Quit()
    {
        Time.timeScale = 1;

        MenuManager.Instance.ActivateMenu("main");
        FindObjectOfType<MainMenuController>().LoadGameScene();
        SceneManager.UnloadSceneAsync(1);
    }

    private void Start()
    {
        instance = this;   
    }

    // MenuManager Methods
    public void ActivateMenu(string id)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].id == id)
            {
                ActivateMenu(menus[i]);
            }
        }
    }

    public void ActivateMenu(Menu menu)
    {
        menu.gameObject.SetActive(true);

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i] != menu)
                menus[i].gameObject.SetActive(false);
        }
    }

    public void DeactivateAll()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].gameObject.SetActive(false);
        }
    }

}
