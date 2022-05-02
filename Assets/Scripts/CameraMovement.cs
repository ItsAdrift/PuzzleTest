using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;

    private Vector3 targetPosition;

    void FixedUpdate()
    {
        if (targetPosition.z != -10)
            return;

        transform.position = Vector3.Lerp(transform.position, targetPosition, movementSpeed * Time.fixedDeltaTime);
    }

    public void MoveTo(GameObject position)
    {
        targetPosition = position.transform.position;
        targetPosition.z = -10;
    }

}
