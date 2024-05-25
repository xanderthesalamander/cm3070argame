using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWallManager : MonoBehaviour
{
    [SerializeField] GameObject wallPrefab;
    [Tooltip("The prefab to be used to generate walls")]
    
    private OVRSceneManager OVRsceneManager;
    private OVRSceneRoom sceneRoom;
    private OVRScenePlane[] wallPlanes;
    private List<GameObject> newWalls;

    private bool wallsUpdated = false;

    private void Awake()
    {
        WallBreakable wb = wallPrefab.GetComponent<WallBreakable>();
        if (wb == null)
        {
            Debug.LogError("RoomWallManager - Cannot get WallBreakable component in prefab");
        }
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
        // Get the room walls
        sceneRoom = FindObjectOfType<OVRSceneRoom>();
        Debug.Log("RoomWallManager - Finding walls");
        wallPlanes = sceneRoom.Walls;
        Debug.Log("RoomWallManager - " + wallPlanes.Length.ToString() + " walls found");
    }

    private void Update()
    {
        if (sceneRoom != null)
        {
            if (wallsUpdated == false)
            {
                // Update walls
                Debug.Log("RoomWallManager - Updating walls");
                foreach (OVRScenePlane wallPlane in wallPlanes)
                {
                    UpdateWall(wallPlane);
                }
                Debug.Log("RoomWallManager - Walls updated");
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
        Debug.Log("RoomWallManager - Wall dimensions: " + wallDimensions.ToString());
        // Place wall prefab
        Debug.Log("RoomWallManager - Create new wall");
        GameObject newWall = Instantiate(wallPrefab);
        newWall.transform.position = wall.transform.position;
        newWall.transform.rotation = wall.transform.rotation;
        // The transform is done in the child object to avoid issues when rotating
        GameObject cube = newWall.transform.Find("Cube").gameObject;
        cube.transform.localScale = new Vector3(wallDimensions.x, wallDimensions.y, cube.transform.localScale.z);
        // Debug.Log("RoomWallManager - New wall location " + newWall.transform.position.ToString());
        // Debug.Log("RoomWallManager - New wall rotation " + newWall.transform.rotation.ToString());
        // Break wall into sections that can be destroyed
        WallBreakable wb = newWall.GetComponent<WallBreakable>();
        if (wb != null)
        {
            wb.MakeBreakable();
        }
        // Deactivate original wall
        wall.SetActive(false);
        Debug.Log("RoomWallManager - Original wall deactivated");

    }

    private void PrintDebugComponents(Component[] components)
    {
        Debug.Log("RoomWallManager - " + components.Length.ToString() + " components found");
        foreach (Component component in components)
        {
            // This has 3 children: Transform, OVRSceneAnchor and OVRSceneRoom
            Debug.Log("RoomWallManager - " + component.GetType().Name.ToString());
        }
    }

}
