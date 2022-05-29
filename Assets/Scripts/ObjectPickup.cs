using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    [SerializeField] CharacterController2D controller;

    [Header("Pickup")]
    [SerializeField] LayerMask pickupMask;
    [SerializeField] Transform carry;
    [SerializeField] Transform drop;

    [Header("Drop Check")]
    [SerializeField] LayerMask ground;
    [SerializeField] Transform wallCheck;
    [SerializeField] float radius = 1f;

    [SerializeField] float distance;
    [SerializeField] Transform check;

    [Header("Throw")]
    [SerializeField] public bool dragBack = true;
    //[SerializeField] Camera cam;
    [SerializeField] LineTrajectory lt;

    [SerializeField] float power;
    [SerializeField] float yAdd;
    [SerializeField] Vector2 minPower;
    [SerializeField] Vector2 maxPower;

    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    [HideInInspector] public MoveableObject pickedUpObject;

    void Update()
    {
        Camera cam = player.cam;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedUpObject == null)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, distance, pickupMask);
                for (int i = 0; i < colliders.Length; i++)
                {

                    if (colliders[i].gameObject.GetComponent<MoveableObject>() != null)
                    {
                        MoveableObject obj = colliders[i].gameObject.GetComponent<MoveableObject>();
                        if (obj.canPickUp && !obj.isPickedUp)
                        {
                            obj.gameObject.transform.SetParent(carry);

                            obj.gameObject.transform.localPosition = new Vector3(0, obj.yOffset, 0);

                            if (obj.rb != null)
                            {
                                if (obj.disableSimulation)
                                {
                                    obj.rb.simulated = false;
                                }
                                else
                                {
                                    obj.rb.bodyType = RigidbodyType2D.Kinematic;
                                }
                                obj.rb.velocity = Vector2.zero;
                                obj.rb.angularVelocity = 0;
                            }    

                            obj.isPickedUp = true;

                            pickedUpObject = obj;
                        }
                        break;
                    }
                }
            } else
            {
                // Drop infront of the player

                Drop();

            }
            
        }

        if (pickedUpObject == null || !pickedUpObject.canThrow)
            return;

        // Start throw
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;
            //Debug.Log(startPoint);
        }

        // Drag Throw
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;

            lt.RenderLine(startPoint, currentPoint, dragBack);
        }

        // Release Throw
        if (Input.GetMouseButtonUp(0))
        {
            pickedUpObject.transform.SetParent(null);
            if (pickedUpObject.rb != null)
            {
                if (pickedUpObject.disableSimulation)
                {
                    pickedUpObject.rb.simulated = true;
                } else
                {
                    pickedUpObject.rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
                

            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            if (dragBack)
            {
                force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y) + yAdd);
            } else
            {
                force = new Vector2(Mathf.Clamp(endPoint.x - startPoint.x, minPower.x, maxPower.x), Mathf.Clamp(endPoint.y - startPoint.y, minPower.y, maxPower.y) + yAdd);
            }

            
            pickedUpObject.rb.AddForce(force * power, ForceMode2D.Impulse);

            lt.EndLine();

            pickedUpObject.isPickedUp = false;
            pickedUpObject = null;
        }

    }

    public void Drop()
    {
        if (pickedUpObject == null)
            return;

        if (Physics2D.OverlapCircle(wallCheck.position, radius, ground))
            return;

        if (pickedUpObject.rb != null)
        {
            if (pickedUpObject.disableSimulation)
            {
                pickedUpObject.rb.simulated = true;
            }
            else
            {
                pickedUpObject.rb.bodyType = RigidbodyType2D.Dynamic;
            }
            pickedUpObject.rb.velocity = Vector2.zero;
        }

        pickedUpObject.transform.SetParent(null);

        Vector3 dropPos = drop.position;
        pickedUpObject.transform.position = dropPos;

        pickedUpObject.isPickedUp = false;
        pickedUpObject = null;
    }

    public bool HasPickedUpObject()
    {
        return pickedUpObject != null;
    }

}
