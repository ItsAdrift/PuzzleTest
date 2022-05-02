using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public Animation animation;
    public string tag;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [Header("Events")]
    [Space]
    public BoolEvent OnButtonStateChangeEvent;
    public UnityEvent OnButtonPress;
    public UnityEvent OnButtonRelease;

    void Awake()
    {
        if (OnButtonStateChangeEvent == null)
            OnButtonStateChangeEvent = new BoolEvent();
        if (OnButtonPress == null)
            OnButtonPress = new UnityEvent();
        if (OnButtonRelease == null)
            OnButtonRelease = new UnityEvent();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == tag)
        {
            animation.Play();
            OnButtonStateChangeEvent.Invoke(true);
            OnButtonPress.Invoke();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == tag)
        {
            OnButtonStateChangeEvent.Invoke(false);
            OnButtonRelease.Invoke();
        }
    }

}
