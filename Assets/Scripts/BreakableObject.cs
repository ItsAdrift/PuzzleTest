using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableObject : MonoBehaviour
{
    public bool isEnabled = true;

    [SerializeField] bool damagedWhenHeld = true;

    [SerializeField] int health = 1;
    int startHealth;

    public UnityEvent OnBreak;

    void Awake()
    {
        startHealth = health;

        if (OnBreak == null)
            OnBreak = new UnityEvent();
    }

    public void Damage(int damage)
    {
        if (!isEnabled)
            return;

        if (!damagedWhenHeld && GetComponent<MoveableObject>() != null && GetComponent<MoveableObject>().isPickedUp)
        {
            return;
        }

        health -= damage;

        if (health >= 0)
        {
            Break();
        }
    }

    public void Heal()
    {
        health = startHealth;
    }

    public void Break()
    {
        OnBreak.Invoke();
    }

    public void SetEnabled(bool b)
    {
        isEnabled = b;
    }

}
