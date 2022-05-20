using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public void Play()
    {
        GetComponent<Animation>().Play("FadeAnim");
    }
}
