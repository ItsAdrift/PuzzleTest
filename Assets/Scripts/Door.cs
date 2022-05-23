using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Collider2D playerCollider;
    [SerializeField] ObjectPickup objectPickup;

    void Update()
    {
        if (objectPickup != null && !objectPickup.HasPickedUpObject())
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, false);
        }
    }

}
