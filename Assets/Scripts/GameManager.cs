using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LayerMask objectMask;
    public LayerMask groundMask;

    public float div = 5f;
    public float min = 0.5f;

    private void Awake()
    {
        PlayerPrefs.SetInt("level_1", 1);

        GameObject[] objs = FindObjectsOfType<GameObject>();
        foreach (GameObject g in objs)
        {
            if (objectMask == (objectMask | (1 << g.layer)))
            {
                CubeCollision c = g.AddComponent<CubeCollision>();
                c.div = div;
                c.min = min;
                c.collisionMask = groundMask;
            }
        }
    }
}
