using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float range;
    public float speed;

    private Vector3 positionDisplacement;
    private Vector3 positionOrigin;
    private float _timePassed;
    private void Start()
    {
        positionOrigin = transform.localPosition;
    }

    private void Update()
    {
        positionDisplacement = new Vector3(range, 0, -10);

        _timePassed += Time.deltaTime;
        transform.localPosition = Vector3.Lerp(positionOrigin, positionOrigin + positionDisplacement, Mathf.PingPong(_timePassed * speed, 1));
    }

}
