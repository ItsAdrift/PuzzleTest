using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPart : MonoBehaviour
{
    // The position and rotation of each part relative to the parent this
    // is required so that each part can go back exactly as it was.
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    bool returning = false;
    float speed;

    Vector3 temp;

    void Start()
    {
        // Set the initialPosition and rotation
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Check if the player has collapsed and should be going back together
        if (!returning)
            return;

        // Lerp (Linearly interpoles between 2 points) between the current position and target position each frame.
        // The creates smooth movement for each body part.
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, speed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, speed * Time.deltaTime);
    }

    public void Return(Transform t, float _speed, float endReturn)
    {
        transform.position = temp;

        returning = true;
        speed = _speed;
        StartCoroutine(StopReturning(endReturn));
    }

    public void Cache()
    {
        // store the current position to go back to to in Return()
        temp = transform.position;
    }

    IEnumerator StopReturning(float endReturn)
    {
        yield return new WaitForSeconds(endReturn);

        returning = false;
    }

}
