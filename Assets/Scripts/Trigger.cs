using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] string tag;
    [SerializeField] string disallowTag;
    [SerializeField] bool oneUse;
    [SerializeField] bool canExitIfOneUse = true;

    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerExit;

    private bool used;

    void Awake()
    {
        if (OnTriggerEnter == null)
            OnTriggerEnter = new UnityEvent();
        if (OnTriggerExit == null)
            OnTriggerExit = new UnityEvent();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (used && oneUse)
            return;

        if ((tag == "" && disallowTag == "") || (collision.tag == tag && collision.tag != disallowTag))
        {
            OnTriggerEnter.Invoke();
            if (oneUse)
                used = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (used && !canExitIfOneUse)
            return;

        if (collision.tag == tag)
        {
            OnTriggerExit.Invoke();
        }
    }

}
