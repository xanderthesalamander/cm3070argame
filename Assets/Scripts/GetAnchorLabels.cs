using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAnchorLabels : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    
    private void Update()
    {
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Vector3 rayDirection = controllerRotation * Vector3.forward;

        if (Physics.Raycast(controllerPosition, rayDirection, out RaycastHit hit))
        {
            // Identify object hit by raycast
            lineRenderer.SetPosition(0, controllerPosition);
            OVRSemanticClassification anchor = hit.collider.gameObject.GetComponentInParent<OVRSemanticClassification>();
            if (anchor != null)
            {
                // Debug.LogError("Hit an anchor with label: " + string.Join(", ", anchor.Labels));
                Vector3 endPoint = anchor.transform.position;
                lineRenderer.SetPosition(1, endPoint);
            }
            else
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        }
    }
}
