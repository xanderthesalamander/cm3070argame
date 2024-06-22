using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oculus.Interaction;

public class TurretPartStats : MonoBehaviour
{
    public string partName;
    public int resourceCost;
    public bool currentlyGrabbed;
    public bool rbEdited;
    private bool leftGrab;
    private bool rightGrab;

    private void OnEnable()
    {
        rbEdited = false;
        // currentlyGrabbed = false;
    }

    void OnTriggerStay(Collider collider)
    {
        // Check if controllers are grabbing the object
        if (collider.gameObject.name == "ControllerGrabLocation")
        {
            // Left
            if (collider.gameObject.transform.parent.parent.parent.name == "LeftController")
            {
                leftGrab = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
            }
            // Right
            if (collider.gameObject.transform.parent.parent.parent.name == "RightController")
            {
                rightGrab = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
            }
            currentlyGrabbed = (leftGrab || rightGrab);
            Debug.Log("TurretPartStats - Left grab: " + leftGrab.ToString());
            Debug.Log("TurretPartStats - Right grab: " + rightGrab.ToString());
            Debug.Log("TurretPartStats - currentlyGrabbed: " + currentlyGrabbed.ToString());
        }
    }

    public bool isGrabbed()
    {
        return currentlyGrabbed;
    }
}
