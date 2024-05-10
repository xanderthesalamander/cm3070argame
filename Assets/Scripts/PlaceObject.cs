using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    public GameObject objectPrefab;
    public GameObject objectPreviewPrefab;
    public bool multiplePlacementsAllowed = false;
    
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
        // 
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Show the prefab preview so that it matches the hit object's normal vector
            currentPreview.transform.position = hit.point;
            currentPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            // Place the actual prefab on button press
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                // Place object
                currentObject.transform.position = hit.point;
                currentObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                currentObject.SetActive(true);
                // Create a new object when multiple placements are allowed
                if (multiplePlacementsAllowed)
                {
                    currentObject = Instantiate(objectPrefab);
                    currentObject.SetActive(false);
                }
            }
        }
    }

    public void togglePreview()
    {
        // Turn preview object on and off
        currentPreview?.SetActive(!currentPreview.activeSelf);
    }
}
