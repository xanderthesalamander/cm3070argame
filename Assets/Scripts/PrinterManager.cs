using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterManager : MonoBehaviour
{
    [SerializeField] Transform placement;
    [Tooltip("The location where the object should be created")]
    [SerializeField] GameObject[] objectPrefabs;
    [Tooltip("A prefab of the object to be placed")]
    [SerializeField] GameObject[] objectPreviewPrefabs;
    [Tooltip("A prefab of the preview object (this is shown before placement)")]
    [SerializeField] float rotationSpeed = 1.0f;
    [Tooltip("A prefab of the preview object (this is shown before placement)")]

    private int currentIndex = 0;
    private int nObjects;
    
    private GameObject currentPreview;

    public void OnDestroy()
    {
        Destroy(currentPreview);
    }
    
    private void Start()
    {
        //
        if (objectPrefabs.Length != objectPreviewPrefabs.Length)
        {
            Debug.LogError(
                "PrinterManager - Length of objectPrefabs and objectPreviewPrefabs are not the same." +
                "\nPlease ensure each object has a preview and vice-versa.");
        }
        nObjects = objectPrefabs.Length;
        // Instantiate preview and object (hidden)
        currentPreview = Instantiate(objectPreviewPrefabs[currentIndex]);
    }

    public void Update()
    {
        // Show and rotate the preview prefab
        currentPreview.transform.position = placement.position;
        currentPreview.transform.Rotate(0, rotationSpeed, 0, Space.Self);
        
    }

    public void ChangeObjectNext()
    {
        currentIndex = (currentIndex + 1) % nObjects;
        Destroy(currentPreview);
        currentPreview = Instantiate(objectPreviewPrefabs[currentIndex]);
    }

    public void ChangeObjecPrevious()
    {
        currentIndex = (currentIndex + nObjects - 1) % nObjects;
        Destroy(currentPreview);
        currentPreview = Instantiate(objectPreviewPrefabs[currentIndex]);
    }

    public void PrintObject()
    {
        // Place object
        GameObject newObject = Instantiate(objectPrefabs[currentIndex]);
        newObject.transform.position = placement.position;
        newObject.transform.rotation = currentPreview.transform.rotation;
        newObject.SetActive(true);
    }
}
