using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class TimedAction : MonoBehaviour
{
    public bool isConsistent = true;

    [Header("Consistent")]
    public float timeEntry;
    public float timeExit;

    [Header("Random")]
    public float randomMin;
    public float randomMax;

    public UnityEvent OnAction;

    void Awake()
    {
        if (OnAction == null)
            OnAction = new UnityEvent();

        if (isConsistent)
        {
            StartCoroutine(Consistent(timeEntry, timeExit));
        } else
        {
            StartCoroutine(_Random(randomMin, randomMax));
        }
    }

    IEnumerator Consistent(float timeA, float timeB)
    {
        bool a = false;

        while (true)
        {
            yield return new WaitForSeconds(a ? timeA : timeB);
            a = !a;

            OnAction.Invoke();
        }
    }

    IEnumerator _Random(float randomMin, float randomMax)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(randomMin, randomMax));

            OnAction.Invoke();
        }
    }
}
