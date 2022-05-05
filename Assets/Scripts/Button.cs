using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public Animation animation;
    public string tag;

    [Header("Broken Particles")]
    public ParticleSystem brokenParticles;
    public int bursts;
    public float burstRate;
    public bool broken;
    public float randMin = 1;
    public float randMax = 3;

    public Color repairedColour;


    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [Header("Events")]
    [Space]
    public BoolEvent OnButtonStateChangeEvent;
    public UnityEvent OnButtonPress;
    public UnityEvent OnButtonRelease;

    void Start()
    {
        if (brokenParticles != null)
            StartCoroutine(BurstForever());
    }

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
        if (!broken && (tag == "" || collision.gameObject.tag == tag))
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

    public void Repair()
    {
        broken = false;
        gameObject.GetComponent<SpriteRenderer>().color = repairedColour;
    }

    IEnumerator BurstForever()
    {
        while (broken) // loop forever
        {

            int burstCount = bursts;
            while (burstCount > 0) // burst of 5 shots
            {
                brokenParticles.Play();
                burstCount--;
                yield return new WaitForSeconds(burstRate);
            }

            yield return new WaitForSeconds(Random.Range(randMin, randMax));
        }
    }

}
