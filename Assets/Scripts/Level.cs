using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
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
            fade.time = 0.5f;
            fade.FadeOut();

            ObjectPickup objectPickup = FindObjectOfType<ObjectPickup>();
            
            if (objectPickup.pickedUpObject != null && objectPickup.pickedUpObject.gameObject.GetInstanceID() == objects[i].GetInstanceID())
            {
                objectPickup.pickedUpObject = null;
            }
            Destroy(objects[i], 1);
        }

        objectsCleared = true;
    }

    public void TriggerExit()
    {
        ClearObjects();
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public void Restart()
    {
        OnRestart.Invoke();
    }

}
