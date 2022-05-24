using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.SetInt("level_1", 1);
    }
}
