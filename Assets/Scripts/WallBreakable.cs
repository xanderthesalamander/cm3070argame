using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreakable : MonoBehaviour
{
    [SerializeField] GameObject WallCube;
    [Tooltip("The main cube object to be broken into pieces")]
    [SerializeField] int horizontalLayers = 6;
    [Tooltip("The number of horizon")]
    [SerializeField] int protectedBottomLayers = 2;
    [Tooltip("The number of layers at the bottom that cannot be destroyed")]
    [SerializeField] Material[] materials;
    [SerializeField] bool testing = false;

    void Start()
    {
        if (testing)
        {
            BreakY(WallCube, horizontalLayers);
        }
    }

    public void MakeBreakable()
    {
        BreakY(WallCube, horizontalLayers);
    }

    private void BreakY(GameObject originalCube, int numSections)
    {
        // Original position, size and scale
        Vector3 originalPosition = originalCube.transform.position;
        Vector3 originalSize = originalCube.GetComponent<Renderer>().bounds.size;
        Vector3 originalScale = originalCube.transform.localScale;
        Debug.Log("X: scale " + originalScale.x.ToString() + ", size " + originalSize.x.ToString());
        Debug.Log("Y: scale " + originalScale.y.ToString() + ", size " + originalSize.y.ToString());
        Debug.Log("Z: scale " + originalScale.z.ToString() + ", size " + originalSize.z.ToString());
        // Section size
        float sectionSize = originalSize.y / numSections;
        // Bottom position
        float bottomY = originalPosition.y - originalSize.y * 0.5f;
        // Break the cube into parts
        for (int i = 0; i < numSections; i++)
        {
            // Calculate the size and position of each part
            float partPosY = bottomY + (i * sectionSize) + (sectionSize * 0.5f);
            Vector3 partSize = new Vector3(originalScale.x, sectionSize, originalScale.z);  // Rotation ok
            // Vector3 partSize = new Vector3(originalScale.x * originalSize.x, sectionSize, originalScale.z * originalSize.z);  // Rotation ok, wall depth different in each wall and width not enough in some walls
            // Vector3 partSize = new Vector3(originalScale.x, sectionSize, originalScale.z); // Rotation ok, but width too small
            // Vector3 partSize = new Vector3(originalScale.x * originalSize.x, sectionSize, originalScale.z);  // Rotation ok, but some walls width not enough
            // Vector3 partSize = new Vector3(originalSize.x, sectionSize, originalSize.z); 
            Vector3 partPosition = new Vector3(originalPosition.x, partPosY, originalPosition.z);
            // Instantiate a smaller cube for each part
            GameObject partCube = Instantiate(originalCube, partPosition, Quaternion.identity);
            partCube.transform.rotation = originalCube.transform.rotation;
            partCube.transform.localScale = partSize;
            // Assign random material from list
            partCube.GetComponent<MeshRenderer>().material = materials[Random.Range(0,materials.Length)];
            // Set parent to original parent
            partCube.transform.SetParent(originalCube.transform.parent);
            // Remove breaking on impact for bottom layers
            if (i < protectedBottomLayers)
            {
                partCube.GetComponent<DestroyOnBulletImpact>().enabled = false;
            }
        }
        // Destroy the original cube
        Destroy(originalCube);
    }

    private void BreakX(GameObject originalCube, int numSections)
    {
        
    }
}
