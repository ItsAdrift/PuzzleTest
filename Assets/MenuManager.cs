using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }

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
