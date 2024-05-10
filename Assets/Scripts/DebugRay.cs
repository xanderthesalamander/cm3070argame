using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRay : MonoBehaviour
{
    [SerializeField] GameObject rayHitPreview;
    
    private GameObject currentRayHitPreview;
    private GameObject currentHit;
    private GameObject previousHit;
    
    void Start()
    {
        currentRayHitPreview = Instantiate(rayHitPreview);
    }

    void Update()
    {
        // Ray from left controller
        Ray ray = new Ray(
            OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch),
            OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Vector3.forward
        );
        // Get what the ray is hitting
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Show a dot where the ray is hitting
            currentRayHitPreview.transform.position = hit.point;
            currentRayHitPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            // Get object information
            OVRSemanticClassification anchor = hit.collider.gameObject.GetComponentInParent<OVRSemanticClassification>();
            if (anchor != null)
            {
                Debug.Log("Hit anchor with labels: " + string.Join(", ", anchor.Labels));
            }
        }
    }

    public void toggleRayHitPreview()
    {
        // Turn preview object on and off
        currentRayHitPreview?.SetActive(!currentRayHitPreview.activeSelf);
    }
}
