using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [SerializeField] bool active = true;

    public bool leftRightMovement = true;

    public float range;
    public float speed;
    float lastSpeed;

    private float initial;
    private Vector3 originPosition;

    private float time = 0;

    bool pingpong = true;

    bool moveTo = false;
    Vector3 moveToPosition;

    private void Start()
    {
        originPosition = transform.position;

        if (leftRightMovement)
        {
            initial = transform.localPosition.x;
        } else
        {
            initial = transform.localPosition.y;
        }
    }

    private void Update()
    {
        if (!active)
            return;

        if (pingpong)
        {
            time += Time.deltaTime;
            float offset = Mathf.PingPong(time * speed, range);
            Vector3 position = transform.localPosition;
            if (leftRightMovement)
            {
                position.x = initial + offset;
            }
            else
            {
                position.y = initial - offset;
            }

            transform.localPosition = position;
        } else if (moveTo)
        {
            transform.position = Vector3.Lerp(transform.position, moveToPosition, speed * Time.deltaTime);
        }
    }

    public void Reset()
    {
        time = 0;
        transform.position = originPosition;
    }

    public void Stop()
    {
        time = 0;

        active = false;
        pingpong = false;
        moveTo = false;
    }

    public void PingPong()
    {
        active = true;
        pingpong = true;
        moveTo = false;
    }

    public void MoveTo(Transform transform)
    {
        MoveTo(transform.position);
    }

    public void MoveTo(Vector3 position)
    {
        active = true;
        pingpong = false;
        moveTo = true;

        moveToPosition = position;
    }

    public void SetSpeed(float speed)
    {
        lastSpeed = this.speed;
        this.speed = speed;
    }

    public void ResetSpeed()
    {
        speed = lastSpeed;
    }

}
