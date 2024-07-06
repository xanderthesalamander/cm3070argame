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
    [SerializeField] Material destroyableMaterial;
    [SerializeField] Material protectedMaterial;
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

    private void BreakXZ(GameObject originalCube, int numSections)
    {
        
    }
    
    private void BreakY(GameObject originalCube, int numSections)
    {
        // Original position, size and scale
        Vector3 originalPosition = originalCube.transform.position;
        Vector3 originalSize = originalCube.GetComponent<Renderer>().bounds.size;
        Vector3 originalScale = originalCube.transform.localScale;
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
            Vector3 partPosition = new Vector3(originalPosition.x, partPosY, originalPosition.z);
            // Instantiate a smaller cube for each part
            GameObject partCube = Instantiate(originalCube, partPosition, Quaternion.identity);
            partCube.transform.rotation = originalCube.transform.rotation;
            partCube.transform.localScale = partSize;
            // Assign random material from list
            partCube.GetComponent<MeshRenderer>().material = destroyableMaterial;
            // Set parent to original parent
            partCube.transform.SetParent(originalCube.transform.parent);
            // Remove breaking on impact for bottom layers
            if (i < protectedBottomLayers)
            {
                partCube.GetComponent<MeshRenderer>().material = protectedMaterial;
                DestroyOnBulletImpact partCubeDOBI = partCube.GetComponent<DestroyOnBulletImpact>();
                if (partCubeDOBI != null)
                {
                    partCubeDOBI.allowDestroy = false;
                }
                Fracture partCubeFracture = partCube.GetComponent<Fracture>();
                if (partCubeFracture != null)
                {
                    Destroy(partCubeFracture);
                }
            }
        }
        // Destroy the original cube
        Destroy(originalCube);
    }
}
