using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRenderer;

    [Header("Fade In")]
    [SerializeField] public float time;
    [SerializeField] float target;

    [Header("Ping Pong")]
    [SerializeField] float pingpongTime;
    [SerializeField] float pingpongTarget;

    bool fadeIn = false;
    bool fadeOut = false;
    bool pingpong = false;

    bool fadeInOut = false;

    private void Update()
    {
        if (fadeIn)
        {
            Color32 colour = spriteRenderer.color;
            colour.a = (byte)Mathf.Lerp(colour.a, target, time * Time.deltaTime);
            spriteRenderer.color = colour;

            if (colour.a == 255 && fadeInOut)
            {
                FadeOut();
                fadeInOut = false;
            }
        }
        else if (fadeOut) {
            Color32 colour = spriteRenderer.color;
            colour.a = (byte)Mathf.Lerp(colour.a, 0, time * Time.deltaTime);
            spriteRenderer.color = colour;
        } else if (pingpong)
        {
            Color32 colour = spriteRenderer.color;
            colour.a = (byte)Mathf.PingPong(Time.time * pingpongTime, pingpongTarget);
            spriteRenderer.color = colour;
        }
    }

    public void FadeOut() {
        fadeOut = true;
        fadeIn = false;
        pingpong = false;

    }

    public void FadeIn()
    {
        fadeIn = true;
        pingpong = false;
        fadeOut = false;
    }

    public void FadeInOut(float delay)
    {
        StartCoroutine(_FadeInOut(delay));
    }

    IEnumerator _FadeInOut(float delay)
    {
        yield return new WaitForSeconds(delay);

        fadeInOut = true;
        FadeIn();
    }

    public void PingPong()
    {
        fadeIn = false;
        pingpong = true;
        fadeOut = false;
    }
}
