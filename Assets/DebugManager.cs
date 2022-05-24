using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public bool debug = true;

    public GameObject mainMenu;

    void Start()
    {
        mainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
