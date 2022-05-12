using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    [SerializeField] bool enabled = true;
    [SerializeField] Vector2 size;

    void OnDrawGizmos()
    {
        if (!enabled)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0));
    }
}
