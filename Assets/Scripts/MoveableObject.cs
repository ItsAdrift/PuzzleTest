using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject: MonoBehaviour
{
    public bool canPickUp = true;
    public bool canThrow = true;
    public Rigidbody2D rb;
    public float yOffset = 0f;
    public float xOffset = 0f;

    [HideInInspector] public bool isPickedUp;
}
