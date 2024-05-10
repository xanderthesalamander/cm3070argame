using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRay : MonoBehaviour
{
    [SerializeField] GameObject rayHitPreview;
    [SerializeField] Material highlightMaterial;
    
    private GameObject currentRayHitPreview;
    private GameObject currentHit;
    private GameObject previousHit;
    private Material currentHitOriginalMaterial;
    private Material previousHitOriginalMaterial;
    
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
            // Show where the ray is hitting
            currentRayHitPreview.transform.position = hit.point;
            currentRayHitPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            // Get object information
            currentHit = hit.collider.gameObject;
            // When object is new
            if (currentHit != previousHit)
            {
                // Apply original material to previous hit
                MeshRenderer previousMeshRenderer = previousHit?.GetComponent<MeshRenderer>();
                if (previousMeshRenderer != null)
                {
                    previousMeshRenderer.material = previousHitOriginalMaterial;
                }
                // Get material of current hit
                MeshRenderer currentMeshRenderer = currentHit.GetComponent<MeshRenderer>();
                if (currentMeshRenderer != null)
                {
                    currentHitOriginalMaterial = currentMeshRenderer?.material;
                    // Apply highlighter
                    currentMeshRenderer.material = highlightMaterial;
                    previousHitOriginalMaterial = currentHitOriginalMaterial;
                }
                // Store as previous hit
                previousHit = currentHit;
            }

            OVRSemanticClassification anchor = hit.collider.gameObject.GetComponentInParent<OVRSemanticClassification>();
            if (anchor != null)
            {
                Debug.Log("DebugRay - Labels: " + string.Join(", ", anchor.Labels));
            }
            else
            {
                Debug.Log("DebugRay - No labels");
            }
        }
    }

    public void toggleRayHitPreview()
    {
        // Turn preview object on and off
        currentRayHitPreview?.SetActive(!currentRayHitPreview.activeSelf);
    }
}
