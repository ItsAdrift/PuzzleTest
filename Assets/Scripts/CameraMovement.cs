using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 1f;

    bool locked = false;

    private Vector3 targetPosition;

    void FixedUpdate()
    {
        if (targetPosition.z != -10)
            return;

        transform.position = Vector3.Lerp(transform.position, targetPosition, movementSpeed * Time.fixedDeltaTime);
    }

    public void MoveTo(GameObject position)
    {
        if (locked)
            return;

        targetPosition = position.transform.position;
        targetPosition.z = -10;
    }
    
    public void DelayedMoveTo(GameObject position, float delay)
    {
        StartCoroutine(Delay(position, delay));
    }

    public void FourSecondDelay(GameObject position)
    {
        DelayedMoveTo(position, 4);
    }

    /**
     * Delays CamerMovement by delay seconds.
     * Automatically Unlocks the camera.
     */
    IEnumerator Delay(GameObject position, float delay)
    {

        yield return new WaitForSeconds(delay);

        Unlock();
        MoveTo(position);
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    public void SetSpeed(float speed)
    {
        movementSpeed = speed;
    }

}
