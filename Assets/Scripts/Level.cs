using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public Transform cameraPosition;
    public GameObject zoomedCamera;

    [SerializeField] public GameObject[] objects;
    [SerializeField] Trigger exitTrigger;
    [SerializeField] Transform spawnPoint;

    bool objectsCleared = false;

    public UnityEvent OnDeath;
    public UnityEvent OnRestart;

    void Awake()
    {
        if (OnDeath == null)
            OnDeath = new UnityEvent();
        if (OnRestart == null)
            OnRestart = new UnityEvent();
    }

    public void Start()
    {
        if (exitTrigger == null)
            return;

        exitTrigger.OnTriggerExit.AddListener(TriggerExit);
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

    public void TriggerExit()
    {
        ClearObjects();

        if (zoomedCamera != null)
        {
            zoomedCamera.SetActive(false);
            FindObjectOfType<PlayerMovement>().camMode = PlayerMovement.CameraMode.NORMAL;
        }
            

        /*PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player.camMode == PlayerMovement.CameraMode.FAR)
        {
            if (player.level.zoomedCamera != null)
            {
                player.level.zoomedCamera.SetActive(true);
            }
            if (zoomedCamera != null)
                zoomedCamera.SetActive(false);
        }*/
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public void Restart()
    {
        OnRestart.Invoke();
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

}
