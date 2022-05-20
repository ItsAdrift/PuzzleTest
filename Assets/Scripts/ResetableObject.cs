using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResetableObject : MonoBehaviour
{
    [HideInInspector] public Vector3 originPosition;

    public UnityEvent OnReset;

    private void Awake()
    {
        if (OnReset == null)
            OnReset = new UnityEvent();
    }

    private void Start()
    {
        originPosition = transform.position;
    }

    public void Reset(float delay)
    {
        StartCoroutine(_Reset(delay));
    }

    IEnumerator _Reset(float delay)
    {
        yield return new WaitForSeconds(delay);

        Reset();
    }

    public void Reset()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        transform.position = originPosition;

        OnReset.Invoke();
    }

}
