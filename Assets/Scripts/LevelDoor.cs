using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] Level level;
    [SerializeField] bool blockObjects = false;

    Collider2D playerCollider;
    Collider2D collider;

    OpeningDoor openingDoor;

    private void Start()
    {
        playerCollider = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<Collider2D>();
        collider = GetComponent<Collider2D>();

        openingDoor = GetComponent<OpeningDoor>();
    }

    void FixedUpdate()
    {
        Physics2D.IgnoreCollision(collider, playerCollider, openingDoor.open);

        if (blockObjects)
            return;
        for (int i = 0; i < level.objects.Length; i++)
        {
            if (level.objects[i] == null)
                return;
            Physics2D.IgnoreCollision(collider, level.objects[i].GetComponent<Collider2D>(), openingDoor.open);
        }
    }

}