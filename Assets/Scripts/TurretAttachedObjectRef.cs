using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttachedObjectRef : MonoBehaviour
{
    public string tagName;
    // Attached object reference
    public GameObject attachedObject;

    private void OnTriggerEnter(Collider other)
    {
        // Check for tag
        if (other.CompareTag(tagName))
        {
            attachedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset when object leaves trigger
        if (other.gameObject == attachedObject)
        {
            attachedObject = null;
        }
    }
}
