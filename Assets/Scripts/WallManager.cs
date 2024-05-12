using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] Material highlightMaterial;
    
    private OVRSceneManager OVRsceneManager;
    private OVRSceneRoom sceneRoom;
    private OVRScenePlane[] wallPlanes;
    private GameObject newWalls;

    private bool wallsUpdated = false;

    private void Awake()
    {
        // Get OVR scene manager
        OVRsceneManager = FindObjectOfType<OVRSceneManager>();
        // Subscribe to scene loaded event
        OVRsceneManager.SceneModelLoadedSuccessfully += SceneLoaded;
    }
    
    private void SceneLoaded()
    {
        // Once scene is loaded, get the room walls
        sceneRoom = FindObjectOfType<OVRSceneRoom>();
        Debug.Log("WallManager - Finding walls");
        wallPlanes = sceneRoom.Walls;
        Debug.Log("WallManager - " + wallPlanes.Length.ToString() + " walls found");
    }

    private void Update()
    {
        if (sceneRoom != null)
        {
            if (wallsUpdated == false)
            {
                // Update walls
                Debug.Log("WallManager - Updating walls");
                UpdateWall(wallPlanes[3]);
                Debug.Log("WallManager - Walls updated");
                wallsUpdated = true;
            }
        }
    }

    void UpdateWall(OVRScenePlane wallPlane)
    {
        // Get wall game object
        Debug.Log("WallManager - Getting parent object");
        GameObject wall = wallPlane.transform.parent.gameObject;

        // Get components
        Debug.Log("WallManager - Getting components");
        Component[] wallComponents = wall.GetComponents(typeof(Component));
        Debug.Log("WallManager - " + wallComponents.Length.ToString() + " components found");
        foreach (Component component in wallComponents)
        {
            Debug.Log("WallManager - " + component.GetType().Name.ToString());
        }

        Debug.Log("WallManager - Copy wall");
        GameObject newWall = Instantiate(wall);
        newWall.transform.position = new Vector3(0.0f, 10.0f, 0.0f);
        Debug.Log("WallManager - New wall placed");
        // Check for renderer
        MeshRenderer meshRenderer = newWall.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Debug.Log("WallManager - Updating material");
            meshRenderer.material = highlightMaterial;
        }
        // Add to newWalls array

    }

}
