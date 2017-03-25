using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {
    //Reference to object being tracked (controller)
    private SteamVR_TrackedObject trackedObj;
    // Stores the game object that the trigger is currently colliding with
    private GameObject collidingObject;
    // Reference to object that is being currently grabbed
    private GameObject objectInHand;
    // Provides access to the controllers data
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    private void SetCollidingObject(Collider col)
    {
        // Checks to see if player is currently holding something, or the object does not have a rigid body
        if (collidingObject || !col.GetComponent<Rigidbody>())
            return;
        // Assigns the object as potential grab object
        collidingObject = col.gameObject;
    }
    // When trigger collider (controller) enters another, sets other as potential grab target
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }
    // Same as above but ensures target is set
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }
    // When the trigger collider exits an object, removes the current colliding object
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
            return;
        collidingObject = null;

    }
    private void GrabObject()
    {
        // Move the grabbed object inside the player's hand 
        objectInHand = collidingObject;
        collidingObject = null;
        //Add a new joint that connects the object to the controller
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }
    // Make a new fixed joint, add it to the controller, and set it so it doesn't break easily
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }
    private void ReleaseObject()
    {
        // Make sure there's a fixed joint attached to the controller
        if (GetComponent<FixedJoint>())
        {
            // Remove the connection to the object held and destory the joint
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            // Add speed and rotation of controller when the player releases the object (aim is realism)
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        // Remove the reference to the formerly attached object
        objectInHand = null;
    }

    // Update is called once per frame
    void Update () {
        //When the player squeezes the trigger, grab the object
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if (collidingObject)
                GrabObject();
        }
        // If the player releases the trigger and there's an object attached to the controller, release it
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if (objectInHand)
                ReleaseObject();
        }
	}
}
