using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] ObjectPickup objectPickup;

    [Header("UI Components")]
    [SerializeField] TMP_Dropdown throwing;

    public void Start()
    {
        if (PlayerPrefs.HasKey("throwing"))
        {
            int i = PlayerPrefs.GetInt("throwing");
            SetDragBack(i);
            throwing.value = i;
        }
    }

    private void SetDragBack(int i)
    {
        if (objectPickup == null)
            return;

        if (i == 0) // Drag back
        {
            objectPickup.dragBack = true;
        } else if (i == 1)
        {
            objectPickup.dragBack = false;
        }
    }

    public void UpdateThrowing()
    {
        SetDragBack(throwing.value);
        PlayerPrefs.SetInt("throwing", throwing.value);
        //Debug.Log("PlayerPrefs value = " + PlayerPrefs.GetInt("throwing"));
    }
}
