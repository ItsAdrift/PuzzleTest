using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool isEnabled = true;
    public Level level; // change this u nerd

    public CharacterController2D controller;
    public Transform gfx;

    public float respawnDelay = 2f;
    public float respawnSpeed = 2f;
    public float respawnEnd = 1f;
    
    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    void Update()
    {
        if (!isEnabled)
            return;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        if (!isEnabled)
            return;

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void Respawn()
    {
        Respawn(level.GetSpawnPoint());
    }

    public void Respawn(Transform position)
    {
        StartCoroutine(_Respawn(position, respawnDelay));  
    }

    IEnumerator _Respawn(Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < gfx.childCount; i++)
        {
            Transform child = gfx.GetChild(i);

            Destroy(child.GetComponent<Rigidbody2D>());
            Destroy(child.GetComponent<BoxCollider2D>());

            child.GetComponent<PlayerPart>()?.Cache();
        }

        transform.position = t.position;

        for (int i = 0; i < gfx.childCount; i++)
        {
            Transform child = gfx.GetChild(i);

            Destroy(child.GetComponent<Rigidbody2D>());
            Destroy(child.GetComponent<BoxCollider2D>());

            child.GetComponent<PlayerPart>()?.Return(t, respawnSpeed, 3);
        }

        StartCoroutine(Enable(respawnEnd));
    }

    IEnumerator Enable(float delay)
    {
        yield return new WaitForSeconds(delay);

        isEnabled = true;
    }
}
