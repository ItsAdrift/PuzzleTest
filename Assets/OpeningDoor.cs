using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    public Animation animation;

    public void SetOpen(bool b)
    {
        if (b)
        {
            animation.Play();
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
