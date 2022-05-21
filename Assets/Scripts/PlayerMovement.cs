using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool isEnabled = true;
    public Level level;

    [Header("Camera")]
    public Camera mainCamera;
    [HideInInspector] public Camera cam;
    public enum CameraMode { NORMAL, FAR };
    [HideInInspector] public CameraMode camMode = CameraMode.NORMAL;

    [Header("Basic")]
    public CharacterController2D controller;
    public Transform gfx;
    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    [Header("Respawn")]
    public float respawnDelay = 2f;
    public float respawnSpeed = 2f;
    public float respawnEnd = 1f;

    [Header("Movement")]
    public UnityEvent OnStart;
    public UnityEvent OnDeath;

    void Awake()
    {
        if (OnStart == null)
            OnStart = new UnityEvent();
        if (OnDeath == null)
            OnDeath = new UnityEvent();
    }

    void Start()
    {
        OnStart.Invoke();
        cam = mainCamera;
    }

    void Update()
    {
        if (!isEnabled)
            return;

        // Movement
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
        }

        // Zoomed Out Camera
        if (Input.GetKeyDown(KeyCode.Z) && level.zoomedCamera != null)
        {
            if (camMode == CameraMode.FAR)
            {
                cam = mainCamera;
                level.zoomedCamera.SetActive(false);
                camMode = CameraMode.NORMAL;
            } else
            {
                cam = level.zoomedCamera.GetComponent<Camera>();
                level.zoomedCamera.SetActive(true);
                camMode = CameraMode.FAR;
            }
            
        }
    }

    void FixedUpdate()
    {
        if (!isEnabled)
            return;

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void Die()
    {
        level?.OnDeath.Invoke();

        Respawn(level.GetSpawnPoint());

        OnDeath.Invoke();
    }

    public void Collapse()
    {
        int count = gfx.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = gfx.GetChild(i);

            if (child.GetComponent<Rigidbody2D>() != null)
                return;

            child.gameObject.AddComponent<BoxCollider2D>();
            Rigidbody2D rb = child.gameObject.AddComponent<Rigidbody2D>();
            rb.velocity = controller.GetRigidbody().velocity;

            if (child.name == "Head")
            {
                Vector2 v = rb.velocity;
                v.y = 1;
                rb.velocity = v;
            }

            isEnabled = false;
        }

        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Respawn(Transform position)
    {
        StartCoroutine(_Respawn(position, respawnDelay));  
    }

    public void Respawn(float delay)
    {
        StartCoroutine(_Respawn(level.GetSpawnPoint(), delay));
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
        GetComponent<BoxCollider2D>().enabled = true;

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

    public void SetLevel(Level level)
    {
        this.level = level;
    }

}
