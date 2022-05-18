using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPart : MonoBehaviour
{
    private Vector3 initalPostion;
    private Quaternion initialRotation;

    bool returning = false;
    float speed;

    Vector3 temp;

    // Start is called before the first frame update
    void Start()
    {
        initalPostion = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (!returning)
            return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, initalPostion, speed * Time.deltaTime);
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
        temp = transform.position;
    }

    IEnumerator StopReturning(float endReturn)
    {
        yield return new WaitForSeconds(endReturn);

        returning = false;
    }

}
