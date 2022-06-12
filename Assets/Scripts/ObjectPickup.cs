using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] PlayerMovement player;

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
    [SerializeField] LineTrajectory lt;

    [SerializeField] float power;
    [SerializeField] float yAdd;
    [SerializeField] Vector2 minPower;
    [SerializeField] Vector2 maxPower;

    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    [HideInInspector] public MoveableObject pickedUpObject;

    private void Start()
    {
        MainMenuController.OnLevelLoad.AddListener(OnlevelLoad);
    }

    void OnlevelLoad()
    {
        dragBack = PlayerPrefs.GetInt("throwing") == 0;
    }

    void Update()
    {
        Camera cam = player.cam;
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Make sure that the user has not already picked up an object.
            if (pickedUpObject == null)
            {
                // Creates an invisible circle at 'check.position', with a radius of 'distance', and a mask of 'pickupMask'
                // Every object inside of this invisible circle is stored in the array 'colliders' of type Collider2D
                Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, distance, pickupMask);
                // I loop through each of these colliders
                for (int i = 0; i < colliders.Length; i++)
                {
                    // Make sure that the gameobject has a MoveableObject component
                    if (colliders[i].gameObject.GetComponent<MoveableObject>() == null)
                        break;
                    
                    MoveableObject obj = colliders[i].gameObject.GetComponent<MoveableObject>();
                    // Make sure that the object can be pickup up, and is not already picked up
                    if (!obj.canPickUp || obj.isPickedUp)
                        break;

                    // Parent the object to the position 'carry' (Make sure that the object moves with the player)
                    obj.gameObject.transform.SetParent(carry);
                    // Reset the object's localPosition (position relative to the parent's position) to center it
                    obj.gameObject.transform.localPosition = new Vector3(0, obj.yOffset, 0);

                    // Check if the object has a RigidBody (has Physics applied to it)
                    if (obj.rb != null)
                    {
                        // Determine whether the object's simulation or RigidbodyType should be modified.
                        if (obj.disableSimulation)
                        {
                            // Disable simulation - This is the method I use on most objects
                            obj.rb.simulated = false;
                        }
                        else
                        {
                            // Setting a Rigidbody's type to Kinematic means that the object will no longer move
                            // according to Physics, but will still interact with objects around it. I need this
                            // option for allowing object's to block lasers while being carried
                            obj.rb.bodyType = RigidbodyType2D.Kinematic; 
                        }
                        // Reset the Rigidbody's velocity. Not doing this lead to an issue where, if you pickup up the object
                        // while it was still falling, it's downward velocity would be re-applied when thrown, leading to 
                        // innacurate throws.
                        obj.rb.velocity = Vector2.zero;
                        obj.rb.angularVelocity = 0;
                    }
                    // Set the object's pickedUp boolean to 'true'
                    obj.isPickedUp = true;
                    // Store the object
                    pickedUpObject = obj;

                    return;
                }
            } else
            {
                // Drop infront of the player
                Drop();

            }
            
        }

        // Make sure that the player is carrying an object before attempted to throw it
        if (pickedUpObject == null || !pickedUpObject.canThrow)
            return;

        // Start throw
        if (Input.GetMouseButtonDown(0)) // First time you click the mouse button
        {
            // Set the private Vector3 variable 'startPoint' to the cursor's position in WorldSpace.
            // WorldSpace is a position inside of the game's world, rather than a pixel position
            // on the screen.

            // 'cam' is a reference to the active camera, whether it's a zoomed out camera or regular.
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            // Set it's z position to 15, to move it infront of all other objects inside of the scene.
            startPoint.z = 15;
        }

        // Drag Throw
        if (Input.GetMouseButton(0)) // Called every frame the mouse button is held down.
        {
            // Get the new position of the mouse in WorldSpace.
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;

            // Render the line throw the script 'LineTrajectory'
            lt.RenderLine(startPoint, currentPoint, dragBack);
        }

        // Release Throw
        if (Input.GetMouseButtonUp(0)) // Mouse button has been released
        {
            // Unparent the object
            pickedUpObject.transform.SetParent(null);
            // Re-enable it's Rigidbody- the same as the Drop() method
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
                
            // Get the endPoint (required for calculating power)
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            if (dragBack)
            {
                // Calculations the force getting the un-clamped power (by subtracting startPoint.x and endPoint.x).
                // I then clamp this power by minPower and maxPower. I then repeat this process but for the y variable.
                force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), 
                    Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y) + yAdd);
            } else
            {
                // The same as the above line, but with the point's swapped around to account for the dragforwards setting
                force = new Vector2(Mathf.Clamp(endPoint.x - startPoint.x, minPower.x, maxPower.x), 
                    Mathf.Clamp(endPoint.y - startPoint.y, minPower.y, maxPower.y) + yAdd);
            }

            // Apply the force
            pickedUpObject.rb.AddForce(force * power, ForceMode2D.Impulse);

            // End the LineRenderer's line
            lt.EndLine();

            // Set the object's 'isPickedUp' boolean to false
            pickedUpObject.isPickedUp = false;
            // Remove the reference to the object
            pickedUpObject = null;
        }

    }

    public void Drop()
    {
        // Make sure that there is actually an object to be dropped
        if (pickedUpObject == null)
            return;

        // Create an invisible circle at 'wallCheck.position' with a radius 'radius' and checks for objects
        // tagged with the tag 'ground'. This ensures that you cannot drop an object inside of a wall.
        if (Physics2D.OverlapCircle(wallCheck.position, radius, ground))
            return;

        // Check if the object has a RigidBody
        if (pickedUpObject.rb != null)
        {
            // If the object's simulation was disabled
            if (pickedUpObject.disableSimulation)
            {
                // re-enable it
                pickedUpObject.rb.simulated = true;
            }
            else
            {
                // If it's RigidbodyType was set to Kinematic, set it back to Dynamic
                pickedUpObject.rb.bodyType = RigidbodyType2D.Dynamic;
            }
            // Set it's velocity to 0. 
            pickedUpObject.rb.velocity = Vector2.zero;
        }

        // Unparent it from the player
        pickedUpObject.transform.SetParent(null);

        // Move the object to the position 'drop.position' (assigned in the inspector)
        Vector3 dropPos = drop.position;
        pickedUpObject.transform.position = dropPos;

        // Set the object's 'isPickedUp' boolean to false
        pickedUpObject.isPickedUp = false;
        // Remove the reference to the object
        pickedUpObject = null;
    }

    public bool HasPickedUpObject()
    {
        return pickedUpObject != null;
    }

}
