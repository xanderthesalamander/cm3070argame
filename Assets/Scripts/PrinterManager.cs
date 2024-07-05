using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrinterManager : MonoBehaviour
{
    [Header("Printer setup")]
    [SerializeField] Transform placement;
    [Tooltip("The location where the object should be created")]
    [SerializeField] float rotationSpeed = 1.0f;
    [Tooltip("A prefab of the preview object (this is shown before placement)")]
    [SerializeField] private TextMeshProUGUI costScreenText;
    [Tooltip("Where to display the cost")]
    [SerializeField] private TextMeshProUGUI catNameScreenText;
    [Tooltip("Where to display the category name")]
    [SerializeField] private TextMeshProUGUI catDescriptionScreenText;
    [Tooltip("Where to display the category description")]
    [SerializeField] private TextMeshProUGUI itemNameScreenText;
    [Tooltip("Where to display the item name")]
    [SerializeField] private TextMeshProUGUI itemDescriptionScreenText;
    [Tooltip("Where to display the item description")]

    [Header("Categories")]
    [SerializeField] string[] catNames;
    [Tooltip("Category names")]
    [TextArea(4,8)]
    [SerializeField] string[] catDescriptions;
    [Tooltip("Category descriptions")]
    
    [Header("Guns")]
    [SerializeField] GameObject[] gunPrefabs;
    [Tooltip("A list of prefabs of the gun to be placed")]
    [SerializeField] GameObject[] gunPreviewPrefabs;
    [Tooltip("A list of prefabs of the preview gun (this is shown before placement)")]
    
    [Header("Turrets")]
    [SerializeField] GameObject[] turretPrefabs;
    [Tooltip("A list of prefabs of the turret to be placed")]
    [SerializeField] GameObject[] turretPreviewPrefabs;
    [Tooltip("A list of prefabs of the preview turret (this is shown before placement)")]
    private GameObject[] objectPrefabs;
    private GameObject[] objectPreviewPrefabs;
    private int currentIndex = 0;
    private GameObject currentPreview;
    private string currentItemName;
    private string currentItemDescription;
    private int currentItemCost;
    private GameObject[][] categoriesPrefabs;
    private GameObject[][] categoriesPrefabsPreviews;
    private int currentCatIndex = 0;
    private ResourceManager resourceManager;
    private bool enoughResources;
    

    public void OnDestroy()
    {
        Destroy(currentPreview);
    }
    
    private void Start()
    {
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        if (resourceManager == null)
        {
            Debug.LogError("PrinterManager - Resource Manager not found");
        }
        // Check that the objects and their previews have same length
        if (gunPrefabs.Length != gunPreviewPrefabs.Length)
        {
            Debug.LogError(
                "PrinterManager - Length of gunPrefabs and gunPreviewPrefabs are not the same." +
                "\nPlease ensure each object has a preview and vice-versa.");
        }
        if (turretPrefabs.Length != turretPreviewPrefabs.Length)
        {
            Debug.LogError(
                "PrinterManager - Length of turretPrefabs and turretPreviewPrefabs are not the same." +
                "\nPlease ensure each object has a preview and vice-versa.");
        }
        // Initialise the categories (array of arrays of game objects)
        categoriesPrefabs = new GameObject[][]{gunPrefabs, turretPrefabs};
        categoriesPrefabsPreviews = new GameObject[][]{gunPreviewPrefabs, turretPreviewPrefabs};
        // Start with first category
        objectPrefabs = categoriesPrefabs[currentCatIndex];
        objectPreviewPrefabs = categoriesPrefabsPreviews[currentCatIndex];
        // Instantiate preview and object (hidden)
        currentPreview = Instantiate(objectPreviewPrefabs[currentIndex]);
        // Show name and description
        updateItemDetails();
    }

    public void Update()
    {
        Preview();    
    }

    private void Preview()
    {
        // Show and rotate the preview prefab
        currentPreview.transform.position = placement.position;
        currentPreview.transform.Rotate(0, rotationSpeed, 0, Space.Self);
    }

    public void ChangeObjectNext()
    {
        currentIndex = (currentIndex + 1) % objectPrefabs.Length;
        Destroy(currentPreview);
        currentPreview = Instantiate(objectPreviewPrefabs[currentIndex]);
        updateItemDetails();
    }

    public void ChangeObjecPrevious()
    {
        currentIndex = (currentIndex + objectPrefabs.Length - 1) % objectPrefabs.Length;
        Destroy(currentPreview);
        currentPreview = Instantiate(objectPreviewPrefabs[currentIndex]);
        updateItemDetails();
    }

    public void ChangeCategoryNext()
    {
        currentCatIndex = (currentCatIndex + categoriesPrefabs.Length + 1) % categoriesPrefabs.Length;
        objectPrefabs = categoriesPrefabs[currentCatIndex];
        objectPreviewPrefabs = categoriesPrefabsPreviews[currentCatIndex];
        Destroy(currentPreview);
        currentIndex = 0;
        currentPreview = Instantiate(objectPreviewPrefabs[currentIndex]);
        updateCategoryDetails();
        updateItemDetails();
    }

    public void ChangeCategoryPrevious()
    {
        currentCatIndex = (currentCatIndex + categoriesPrefabs.Length - 1) % categoriesPrefabs.Length;
        objectPrefabs = categoriesPrefabs[currentCatIndex];
        objectPreviewPrefabs = categoriesPrefabsPreviews[currentCatIndex];
        Destroy(currentPreview);
        currentIndex = 0;
        currentPreview = Instantiate(objectPreviewPrefabs[currentIndex]);
        updateCategoryDetails();
        updateItemDetails();
    }

    public void PrintObject()
    {
        if (enoughResources)
        {
            // Pay resources
            resourceManager.RemoveResource(currentItemCost);
            // Place object
            GameObject newObject = Instantiate(objectPrefabs[currentIndex]);
            newObject.transform.position = placement.position;
            newObject.transform.rotation = currentPreview.transform.rotation;
            newObject.SetActive(true);
            // Re-calculate for multiple prints
            enoughResources = currentItemCost <= resourceManager.GetCurrentResources();
        }
        else
        {
            Debug.Log("Not enough resources");
        }
    }

    private void updateItemDetails()
    {
        currentItemName = currentPreview.GetComponent<PrinterItemDetails>().itemName;
        currentItemDescription = currentPreview.GetComponent<PrinterItemDetails>().itemDescription;
        currentItemCost = currentPreview.GetComponent<PrinterItemDetails>().itemCost;
        if (itemNameScreenText != null)
        {
            itemNameScreenText.text = currentItemName;
        }
        if (itemDescriptionScreenText != null)
        {
            itemDescriptionScreenText.text = currentItemDescription;
        }
        if (costScreenText != null)
        {
            costScreenText.text = currentItemCost.ToString();
        }
        enoughResources = currentItemCost <= resourceManager.GetCurrentResources();
    }

    private void updateCategoryDetails()
    {
        // Update the text on the category display
        if (catNameScreenText != null)
        {
            catNameScreenText.text = catNames[currentCatIndex];
        }
        if (catDescriptionScreenText != null)
        {
            catDescriptionScreenText.text = catDescriptions[currentCatIndex];
        }
    }
}
