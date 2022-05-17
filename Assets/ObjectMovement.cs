using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float range;
    public float speed;

    private float initial;
    private float _range;

    private void Start()
    {
        initial = transform.localPosition.x;
        _range = UnityEngine.Random.Range(range, range);
    }

    private void Update()
    {
        float x = Mathf.PingPong(Time.time * speed, _range);
        Vector3 position = transform.localPosition;
        position.x = initial + x;
        transform.localPosition = position;
    }

}
