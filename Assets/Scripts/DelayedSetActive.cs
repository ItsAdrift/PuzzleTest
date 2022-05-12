using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSetActive : MonoBehaviour
{
    public GameObject child;

    public void Activate(float delay)
    {
        StartCoroutine(SetActiveDelay(true, delay));
    }

    public void Deactivate(float delay)
    {
        StartCoroutine(SetActiveDelay(false, delay));
    }

    private IEnumerator SetActiveDelay(bool active, float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.GetComponent<SpriteRenderer>().enabled = active;
        if (child != null)
        {
            child.SetActive(active);
        }
    }
}
