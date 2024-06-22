using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Grab;

public class TurretAttachmentPoint : MonoBehaviour
// This script should be attached to the attachment trigger collider
{
    [SerializeField] string attachableTag;
    [Tooltip("The tag of objects that can be attached")]
    [SerializeField] Transform transformPosition;
    [Tooltip("The location where to place the attached object")]
    [SerializeField] Rigidbody thisRigidbody;
    [Tooltip("The rigidbody of this object")]
    public GameObject attachedObject;
    private bool occupied;
    // private Collider occupyingObject;
    private List<Collider> colliders;

    private void Start()
    {
        occupied = false;
        colliders = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == attachableTag)
        {
            colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == attachableTag) && colliders.Contains(other))
        {
            colliders.Remove(other);
        }
    }

    private void FixedUpdate()
    {
        if (occupied)
        {
            GameObject occupier = attachedObject.GetComponent<Collider>().gameObject;
            bool isGrabbed = isObjGrabbed(occupier);
            if (isGrabbed)
            {
                // Re-add physics properties to rigid body
                Debug.Log("TurretAttachmentPoint - Object grabbed - Resetting rb");
                // Reset parent
                occupier.transform.SetParent(null);
                Rigidbody occupierRB = occupier.GetComponent<Rigidbody>();
                TurretPartStats occupierStats = occupier.GetComponent<TurretPartStats>();
                if (occupierRB != null && occupierStats.rbEdited)
                {
                    // Reset original rb
                    occupierRB.useGravity = true;
                    occupierRB.isKinematic = false;
                    occupierStats.rbEdited = false;
                    // Removing forces from current rigid body and attachment to avoid movements
                    occupierRB.velocity = Vector3.zero;
                    occupierRB.angularVelocity = Vector3.zero;
                    if (!thisRigidbody.isKinematic)
                    {
                        thisRigidbody.velocity = Vector3.zero;
                        thisRigidbody.angularVelocity = Vector3.zero;
                    }
                }
                Debug.Log("TurretAttachmentPoint - Not occupied");
                occupied = false;
                attachedObject = null;
            }
            
        }
        else if (colliders != null)
        {
            // Currently not occupied
            foreach (Collider collider in colliders)
            {
                GameObject other = collider.gameObject;
                TurretPartStats otherStats = other.GetComponent<TurretPartStats>();
                bool isGrabbed = isObjGrabbed(other);
                if (!isGrabbed)
                {
                    // Attach if not grabbed
                    Debug.Log("TurretAttachmentPoint - Attaching object");
                    // Update object position and make child
                    other.transform.position = transformPosition.position;
                    other.transform.rotation = transformPosition.rotation;
                    other.transform.parent = transformPosition.transform;
                    // Remove physics and forces to keep it in place
                    Rigidbody otherRB = other.GetComponent<Rigidbody>();
                    if (otherRB != null)
                    {
                        otherRB.useGravity = false;
                        otherRB.isKinematic = true;
                        otherStats.rbEdited = true;
                        // Removing forces from current rigid body to avoid movements
                        if (!thisRigidbody.isKinematic)
                        {
                            thisRigidbody.velocity = Vector3.zero;
                            thisRigidbody.angularVelocity = Vector3.zero;
                        }
                    }
                    // Save object reference
                    Debug.Log("TurretAttachmentPoint - Occupied");
                    occupied = true;
                    attachedObject = other.gameObject;
                    return;
                }
            }
        }
        else
        {

        }
    }

    private bool isObjGrabbed(GameObject go)
    {
        TurretPartStats turretpartStats = go.GetComponent<TurretPartStats>();
        if (turretpartStats == null)
        {
            Debug.Log("TurretAttachmentPoint - Object is not a turret part");
        }
        bool isGrabbed = turretpartStats.isGrabbed();
        return isGrabbed;
    }
}
