using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    public bool hasAnimation;
    public float openOffset;
    public float openSpeed = 1f;
    public Animation animation;

    bool used;
    bool opening = false;
    Vector3 target;

    public void Update()
    {
        if (opening)
            transform.position = Vector3.Lerp(transform.position, target, openSpeed * Time.deltaTime);
    }

    public void SetOpen(bool b)
    {
        if (b && !used)
        {
            if (hasAnimation)
            {
                animation.Play();
            } else
            {
                target = transform.position;
                target.y = transform.position.y + openOffset;
                opening = true;
            }
            
            used = true;
        } /*else
        {
            animation.Rewind();
        }*/
    }

    public void Close(int delay)
    {
        StartCoroutine(_Close(delay));
    }

    private IEnumerator _Close(int delay)
    {
        yield return new WaitForSeconds(delay);
        
        animation.Rewind();
        animation.Play();
        animation.Sample();
        animation.Stop();

    }

}
