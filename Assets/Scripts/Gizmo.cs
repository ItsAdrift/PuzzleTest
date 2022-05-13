using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    [SerializeField] Color color = Color.yellow;
    [SerializeField] bool enabled = true;
    [SerializeField] Vector2 size;

    void OnDrawGizmos()
    {
        if (!enabled)
            return;
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0));
    }
}
