using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourableObject : MonoBehaviour
{
    [SerializeField] Color[] colours;

    public void SetColor(int colour)
    {
        GetComponent<SpriteRenderer>().color = colours[colour];
    }
}
