using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    [Tooltip("A prefab of the object to be placed")]
    [SerializeField] GameObject objectPreviewPrefab;
    [Tooltip("A prefab of the preview object (this is shown before placement)")]
    [SerializeField] bool multiplePlacementsAllowed = false;
    [Tooltip("Whether multiple copies of the object can be placed. ")]
    [SerializeField] bool faceController = false;
    [Tooltip("Whether the object should face the player. ")]
    public UnityEvent whenPlaced;
    private GameObject currentPreview;
    private GameObject currentObject;

    private void Start()
    {
        // Instantiate preview and object (hidden)
        currentPreview = Instantiate(objectPreviewPrefab);
        currentObject = Instantiate(objectPrefab);
        currentObject.SetActive(false);
    }

    public void Update()
    {
        // Ray to position object from left controller
        Ray ray = new Ray(
            OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch),
            OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Vector3.forward
        );
        // Get what the ray is hitting
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Update location when hitting the room
            OVRSemanticClassification anchor = hit.collider.gameObject.GetComponentInParent<OVRSemanticClassification>();
            if (anchor != null)
            {
                // Show the prefab preview so that it matches the hit object's normal vector
                currentPreview.transform.position = hit.point;
                currentPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                if (faceController)
                {
                    Vector3 target = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                    currentPreview.transform.LookAt(new Vector3(
                        target.x,
                        currentPreview.transform.position.y,
                        target.z
                    ));
                    // currentPreview.transform.LookAt(OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch));
                }
            }
            // Place the actual prefab on button press
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                // Place object
                currentObject.transform.position = currentPreview.transform.position;
                currentObject.transform.rotation = currentPreview.transform.rotation;
                currentObject.SetActive(true);
                // Create a new object when multiple placements are allowed
                if (multiplePlacementsAllowed)
                {
                    currentObject = Instantiate(objectPrefab);
                    currentObject.SetActive(false);
                }
                // Trigger event if specified
                if (whenPlaced != null)
                {
                    whenPlaced.Invoke();
                }
            }
        }
    }

    public void TogglePreview()
    {
        // Turn preview object on and off
        currentPreview?.SetActive(!currentPreview.activeSelf);
    }

    public void PreviewOff()
    {
        currentPreview?.SetActive(false);
    }
}
