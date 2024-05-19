using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] GameObject wallPrefab;
    
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
        GetWalls();
    }

    private void GetWalls()
    {
        // // Get the room walls
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
                foreach (OVRScenePlane wallPlane in wallPlanes)
                {
                    UpdateWall(wallPlane);
                }
                Debug.Log("WallManager - Walls updated");
                wallsUpdated = true;
            }
        }
    }

    private void UpdateWall(OVRScenePlane wallPlane)
    {
        // Get wall game object
        GameObject wall = wallPlane.transform.gameObject;
        // Get position
        Vector3 wallPosition = wall.transform.position;
        // Get OVSSceneAnchor
        OVRSceneAnchor wallAnchor = wall.GetComponent<OVRSceneAnchor>();
        // Get dimensions
        Vector2 wallDimensions = wallPlane.Dimensions;
        Debug.Log("WallManager - Wall dimensions: " + wallDimensions.ToString());
        // Place wall prefab
        Debug.Log("WallManager - Create new wall");
        GameObject newWall = Instantiate(wallPrefab);
        newWall.transform.position = wall.transform.position;
        newWall.transform.rotation = wall.transform.rotation;
        newWall.transform.localScale = new Vector3(wallDimensions.x, wallDimensions.y, newWall.transform.localScale.z);
        Debug.Log("WallManager - New wall placed " + newWall.transform.position.ToString());
        // Deactivate original wall
        wall.SetActive(false);
        Debug.Log("WallManager - Original wall deactivated");

    }

    private void PrintDebugComponents(Component[] components)
    {
        Debug.Log("WallManager - " + components.Length.ToString() + " components found");
        foreach (Component component in components)
        {
            // This has 3 children: Transform, OVRSceneAnchor and OVRSceneRoom
            Debug.Log("WallManager - " + component.GetType().Name.ToString());
        }
    }

}
