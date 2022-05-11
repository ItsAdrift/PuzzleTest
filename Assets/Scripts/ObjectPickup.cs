using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] CharacterController2D controller;

    [SerializeField] Transform carry;
    [SerializeField] Transform drop;

    [Header("Drop Check")]
    [SerializeField] LayerMask ground;
    [SerializeField] Transform wallCheck;
    [SerializeField] float radius = 1f;

    [SerializeField] float distance;
    [SerializeField] Transform check;

    [Header("Throw")]
    [SerializeField] Camera cam;
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

                            obj.gameObject.transform.localPosition = new Vector3(0, obj.yOffset, 0);

                            if (obj.rb != null)
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

                if (Physics2D.OverlapCircle(wallCheck.position, radius, ground))
                    return;

                pickedUpObject.transform.SetParent(null);

                Vector3 dropPos = drop.position;
                pickedUpObject.transform.position = dropPos;

                if (pickedUpObject.rb != null)
                {
                    pickedUpObject.rb.simulated = true;
                    pickedUpObject.rb.velocity = Vector2.zero;
                }

                pickedUpObject.isPickedUp = false;
                pickedUpObject = null;

            }
            
        }

        if (pickedUpObject == null || !pickedUpObject.canThrow)
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

            lt.RenderLine(startPoint, currentPoint);
        }

        if (Input.GetMouseButtonUp(0))
        {
            pickedUpObject.transform.SetParent(null);
            if (pickedUpObject.rb != null)
                pickedUpObject.rb.simulated = true;

            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y) + yAdd);
            pickedUpObject.rb.AddForce(force * power, ForceMode2D.Impulse);

            lt.EndLine();

            pickedUpObject.isPickedUp = false;
            pickedUpObject = null;
        }

    }

    public bool HasPickedUpObject()
    {
        return pickedUpObject != null;
    }

}
