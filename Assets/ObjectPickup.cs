using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] CharacterController2D controller;

    [SerializeField] Transform carry;
    [SerializeField] float distance;
    [SerializeField] Transform check;

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
                pickedUpObject.transform.SetParent(null);
                pickedUpObject.rb.simulated = true;
                pickedUpObject.rb.velocity = controller.GetVelocity();
                pickedUpObject.isPickedUp = false;

                pickedUpObject = null;
            }
            
        }
    }

    public bool HasPickedUpObject()
    {
        return pickedUpObject != null;
    }

}
