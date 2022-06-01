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
    // A hidden public variable to store the active camera, zoomed in or regular 
    [HideInInspector] public Camera cam; 
    public enum CameraMode { NORMAL, FAR };
    [HideInInspector] public CameraMode camMode = CameraMode.NORMAL;

    [Header("Basic")]
    [SerializeField] CharacterController2D controller;
    [SerializeField] Transform gfx;
    [SerializeField] float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    [Header("Respawn")]
    [SerializeField] float respawnDelay = 2f;
    [SerializeField] float respawnSpeed = 2f;
    [SerializeField] float respawnEnd = 1f;

    [Header("Events")]
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if the game is paused, resume
            if (PauseMenuManager.instance.paused)
                PauseMenuManager.instance.Resume();
            // else (the game is playing), pause
            else
                PauseMenuManager.instance.Pause();
        }

        // Make sure that the PlayerMovement is enabled & not paused
        if (!isEnabled) 
            return;

        // Movement
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
        }

        // Zoomed Out Camera
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // If the camMode is far, set to normal
            if (camMode == CameraMode.FAR)
            {
                SetCameraMode(CameraMode.NORMAL);
            }
            // Else, camera mode is normal, set to far
            else
            {
                SetCameraMode(CameraMode.FAR);
            }
            
        }
    }

    public void SetCameraMode(CameraMode mode)
    {
        // Make sure that the level has a 'zoomedComera' variable
        if (level.zoomedCamera == null)
            return;

        // If we are setting the camera to 'NORMAL'
        if (mode == CameraMode.FAR)
        {
            // Set the 'cam' variable to the zoomedCamera's 'Camera' component
            // This 'cam' reference is used in the ObjectPickup script
            cam = level.zoomedCamera.GetComponent<Camera>();
            // Enable the zoomed camera
            level.zoomedCamera.SetActive(true);
        }
        else
        {
            // Set the 'cam' variable back to mainCamera
            cam = mainCamera;
            // Disable the zoomedCamera
            level.zoomedCamera.SetActive(false);
        }
        camMode = mode;
    }

    void FixedUpdate()
    {
        // Make sure that the PlayerMovement is enabled & not paused
        if (!isEnabled)
            return;

        // Apply the movement through the CharacterController2D
        // Multiplied by Time.fixedDeltaTime to ensure that movement is consistent
        // on all framerates
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        // Reset Jump
        jump = false;
    }

    public void Die()
    {
        level?.OnDeath.Invoke();
        OnDeath.Invoke();

        Respawn(level.GetSpawnPoint());
    }

    public void Collapse()
    {
        // Get the number of children (each bodypart is a seperate object)
        int count = gfx.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = gfx.GetChild(i);

            // Make sure that the object doesn't already have a Rigidbody
            if (child.GetComponent<Rigidbody2D>() != null)
                return;

            // Add a BoxCollider2D and Rigidbody2D (To give each part
            // it's own physics)
            child.gameObject.AddComponent<BoxCollider2D>();
            Rigidbody2D rb = child.gameObject.AddComponent<Rigidbody2D>();
            // Apply the force of the player to these parts, to make
            // the collapse seamless
            rb.velocity = controller.GetRigidbody().velocity;

            // If the child is the "Head" object, add some Y velocity
            // to make the head pop up and out a bit.
            if (child.name == "Head")
            {
                Vector2 v = rb.velocity;
                v.y = 1;
                rb.velocity = v;
            }

            // Disable the PlayerController movemnt
            isEnabled = false;
        }

        // Disable the PlayerController's main BoxCollider2D to
        // sell the effect that it has split up
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
        // Wait for second's 'delay'
        yield return new WaitForSeconds(delay);

        // For each child object, store it's current position
        for (int i = 0; i < gfx.childCount; i++)
        {
            Transform child = gfx.GetChild(i);

            // Remove it's Rigidbody and Collider (remove it's physics)
            Destroy(child.GetComponent<Rigidbody2D>());
            Destroy(child.GetComponent<BoxCollider2D>());

            // Call Cache() on the PlayerPart script
            child.GetComponent<PlayerPart>()?.Cache();
        }

        // Move the player to the spawnpoint
        transform.position = t.position;
        // reenable the player's collider
        GetComponent<BoxCollider2D>().enabled = true;

        // for each of the child objects, start moving back to it's 
        // attatched position
        for (int i = 0; i < gfx.childCount; i++)
        {
            Transform child = gfx.GetChild(i);

            // Call Return() on the PlayerPart script.
            child.GetComponent<PlayerPart>()?.Return(t, respawnSpeed, 3);
        }

        // reenable the PlayerController after 'respawnEnd' seconds.
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
        // Unlock the level (To display it on the main menu)
        PlayerPrefs.SetInt("level_" + level.level, 1);
    }

    public void GoToSpawnpoint()
    {
        transform.position = level.GetSpawnPoint().position;
    }

}
