using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] CharacterController2D controller;

    [SerializeField] Transform carry;
    [SerializeField] float distance;
    [SerializeField] Transform check;

    [Header("Throw")]
    [SerializeField] Camera cam;
    [SerializeField] LineTrajectory lt;

    [SerializeField] float power;
    [SerializeField] Vector2 minPower;
    [SerializeField] Vector2 maxPower;

    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    MoveableObject pickedUpObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedUpObject == null)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, distance);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.GetComponent<MoveableObject>() != null)
                    {
                        MoveableObject obj = colliders[i].gameObject.GetComponent<MoveableObject>();
                        if (obj.canPickUp && !obj.isPickedUp)
                        {
                            obj.gameObject.transform.SetParent(carry);
                            obj.gameObject.transform.localPosition = new Vector3(0, 0, 0);
                            obj.rb.simulated = false;

                            obj.isPickedUp = true;

                            pickedUpObject = obj;
                        }
                        break;
                    }
                }
            } else
            {
                // Drop infront of the player
                /*pickedUpObject.transform.SetParent(null);
                pickedUpObject.rb.simulated = true;
                Vector3 v = cam.ScreenToWorldPoint(Input.mousePosition) - pickedUpObject.transform.position; 
                pickedUpObject.rb.velocity = v * force;
                pickedUpObject.isPickedUp = false;

                pickedUpObject = null;*/
            }
            
        }

        if (pickedUpObject == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;
            //Debug.Log(startPoint);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;

            Debug.Log("LT IS null? " + lt == null);
            Debug.Log("Start Point: " + startPoint);
            Debug.Log("Current Point: " + currentPoint);

            lt.RenderLine(startPoint, currentPoint);
        }

        if (Input.GetMouseButtonUp(0))
        {
            pickedUpObject.transform.SetParent(null);
            pickedUpObject.rb.simulated = true;

            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
            pickedUpObject.rb.AddForce(force * power, ForceMode2D.Impulse);

            pickedUpObject.isPickedUp = false;
            pickedUpObject = null;
        }

    }

    public bool HasPickedUpObject()
    {
        return pickedUpObject != null;
    }

}