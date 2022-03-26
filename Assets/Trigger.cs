using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] string tag;
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
        if (used)
            return;

        if (collision.tag == tag)
        {
            OnTriggerEnter.Invoke();
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
