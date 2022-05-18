using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public bool leftRightMovement = true;

    public float range;
    public float speed;

    private float initial;
    private float _range;

    private void Start()
    {
        if (leftRightMovement)
        {
            initial = transform.localPosition.x;
        } else
        {
            initial = transform.localPosition.y;
        }
        //_range = UnityEngine.Random.Range(range, range);
    }

    private void Update()
    {
        float offset = Mathf.PingPong(Time.time * speed, range);
        Vector3 position = transform.localPosition;
        if (leftRightMovement)
        {
            position.x = initial + offset;
        } else
        {
            position.y = initial - offset;
        }
        
        transform.localPosition = position;
    }

}
