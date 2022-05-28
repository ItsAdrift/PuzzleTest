using UnityEngine.Events;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public string sound;

    public LayerMask collisionMask;
    public float div = 3;
    public float min = 0.5f;

    public float pitchMin = 0.9f;
    public float pitchMax = 1.2f;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer))) {

            float mag = collision.relativeVelocity.magnitude / div;
            Debug.Log(mag);

            if (mag >= min)
            {
                AudioManager.instance.Play(sound, mag, Random.Range(pitchMin, pitchMax));
            }
            
        }
    }

}
