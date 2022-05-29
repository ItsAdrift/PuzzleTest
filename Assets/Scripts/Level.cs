using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public int level;
    private string playerTag = "player";

    [Header("Camera Settings")]
    public float cameraSpeed;
    public Transform cameraPosition;
    public GameObject zoomedCamera;

    [Header("Level Settings")]
    [SerializeField] BoxCollider2D levelCollider;
    [SerializeField] public GameObject[] objects;
    [SerializeField] Transform spawnPoint;
    [SerializeField] OpeningDoor exitDoor;

    bool objectsCleared = false;

    [Header("Events")]
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    public UnityEvent OnDeath;
    public UnityEvent OnRestart;

    void Awake()
    {
        if (OnEnter == null)
            OnEnter = new UnityEvent();
        if (OnExit == null)
            OnExit = new UnityEvent();
        if (OnDeath == null)
            OnDeath = new UnityEvent();
        if (OnRestart == null)
            OnRestart = new UnityEvent();


    }

    public void Start()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            Physics2D.IgnoreCollision(levelCollider, objects[i].GetComponent<Collider2D>());
        }
    }

    /*
     * Level Enter/Exit
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != playerTag)
            return;

        OnEnter.Invoke();

        CameraMovement cam = FindObjectOfType<CameraMovement>();
        cam.MoveTo(cameraPosition);
        cam.movementSpeed = cameraSpeed;

        // Change Level
        FindObjectOfType<PlayerMovement>().SetLevel(this);

        Level lastLevel = LevelManager.instance.FindLevel(level - 1);
        if (lastLevel != null)
        {
            lastLevel.OnExit.Invoke();
            lastLevel.OnLevelExit();
        }
    }

    public void OnLevelExit()
    {
        OnExit.Invoke();

        levelCollider.isTrigger = false;
        exitDoor.Close(1);

        ClearObjects();

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (zoomedCamera != null)
        {
            zoomedCamera.SetActive(false);
            player.SetCameraMode(PlayerMovement.CameraMode.NORMAL);
        }
    }

    public void JumpTo()
    {
        Vector3 pos = cameraPosition.position;
        pos.z = -10;
        Camera.main.transform.position = pos;

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        player.level = this;
        player.transform.position = spawnPoint.position;
    }

    // Other

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public void Restart()
    {
        OnRestart.Invoke();
    }

    public void ClearObjects()
    {
        if (objectsCleared)
            return;

        for (int i = 0; i < objects.Length; i++)
        {
            SpriteRenderer spritRenderer = objects[i].GetComponent<SpriteRenderer>();
            Fade fade = objects[i].AddComponent<Fade>();
            fade.spriteRenderer = spritRenderer;
            fade.time = 2f;
            fade.FadeOut();

            ObjectPickup objectPickup = FindObjectOfType<ObjectPickup>();

            if (objectPickup.pickedUpObject != null && objectPickup.pickedUpObject.gameObject.GetInstanceID() == objects[i].GetInstanceID())
            {
                objectPickup.pickedUpObject = null;
            }
            Destroy(objects[i], 1.5f);
        }

        objectsCleared = true;
    }

}
