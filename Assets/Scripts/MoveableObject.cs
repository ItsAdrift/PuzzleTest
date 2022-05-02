using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject: MonoBehaviour
{
    public bool canPickUp = true;
    public Rigidbody2D rb;

    [HideInInspector] public bool isPickedUp;
}
