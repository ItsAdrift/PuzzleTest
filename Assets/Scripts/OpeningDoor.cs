using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    public bool oneUse;
    public bool hasAnimation;
    Vector3 origin;
    public float openOffset;
    public float openSpeed = 1f;
    public Animation animation;

    bool used = false;
    bool closed = false;
    bool open = false;
    bool opening = false;
    Vector3 target;

    private void Start()
    {
        origin = transform.position;
    }

    public void Update()
    {
        if (opening)
        {
            if (transform.position == target)
            {
                opening = false;
            }
            transform.position = Vector3.Lerp(transform.position, target, openSpeed * Time.deltaTime);
        }
            
    }

    public void SetOpen(bool b)
    {
        //Debug.Log(open);
        if (b && !(oneUse && used) && !open)
        {
            if (hasAnimation)
            {
                animation.Play();
                open = true;
                used = true;
            } else
            {
                target = transform.position;
                target.y = transform.position.y + openOffset;
                opening = true;
                open = true;
                used = true;
            }
            
            
            closed = false;
        } else
        {
            Close(0);
        }
    }

    public void Close(int delay)
    {
        if (closed)
            return;

        StartCoroutine(_Close(delay));
    }

    public void CloseIfOpen(int delay)
    {
        if (!closed)
        {
            if (opening)
            {
                opening = false;
                open = false;
                transform.position = origin;
                return;
            } else
                Close(delay);

            used = false;
        }
    }

    private IEnumerator _Close(int delay)
    {
        yield return new WaitForSeconds(delay);   

        if (hasAnimation)
        {
            animation.Rewind();
            animation.Play();
            animation.Sample();
            animation.Stop();
        } else
        {
            target = origin;
            opening = true;
            //transform.position;
            //target.y = transform.position.y - openOffset;
        }

        closed = true;
        open = false;

    }

}
