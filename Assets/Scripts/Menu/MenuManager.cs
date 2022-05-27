using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public Menu[] menus;

    [Header("Titles")]
    public Sprite defaultTitle;
    public Sprite secondaryTitle;
    public float secondaryTitleChance;
    public Image[] titles;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Sprite title = defaultTitle;
        if (secondaryTitleChance > Random.Range(0, 100))
        {
            title = secondaryTitle;
        }

        for (int i = 0; i < titles.Length; i++)
        {
            titles[i].sprite = title;
        }    
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
