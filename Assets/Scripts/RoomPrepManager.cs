using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class RoomPrepManager : MonoBehaviour
{
    [SerializeField] GameObject roomMeshPrefab;
    [Tooltip("The prefab to be used to generate the room prefab")]
    [SerializeField] GameObject wallPrefab;
    [Tooltip("The prefab to be used to generate walls")]

    // ==== TEST
    [SerializeField] GameObject go1;
    [SerializeField] GameObject go2;
    [SerializeField] Material highlightMaterial;
    // ==== TEST END
    
    private OVRSceneManager OVRsceneManager;
    private OVRSceneRoom sceneRoom;
    private OVRScenePlane[] wallPlanes;
    private OVRSceneVolumeMeshFilter[] sceneVolumeMeshFilters;
    private List<GameObject> roomMeshes = new List<GameObject>();
    // private List<GameObject> newWalls;

    private bool roomUpdated = false;
    private bool globalMeshUpdated = false;

    private void Awake()
    {
        // Get WallBreakable component from prefab
        WallBreakable wb = wallPrefab.GetComponent<WallBreakable>();
        if (wb == null)
        {
            Debug.LogError("RoomPrepManager - Cannot get WallBreakable component in wallPrefab");
        }
        // Get OVR scene manager
        OVRsceneManager = FindObjectOfType<OVRSceneManager>();
        // Subscribe to scene loaded event
        OVRsceneManager.SceneModelLoadedSuccessfully += SceneLoaded;

        // ==== TEST
        CSGSubtractDirectly(go1, go2);
        // ==== TEST END
    }

    private void SceneLoaded()
    {
        GetRoomMeshes();
        GetWalls();
    }

    private void GetRoomMeshes()
    {
        // Get all OVRSceneVolumeMeshFilters (this should find the GLOBAL_MESH)
        sceneVolumeMeshFilters = UnityEngine.Object.FindObjectsOfType<OVRSceneVolumeMeshFilter>();
        // Add to room meshes
        roomMeshes = new List<GameObject>();
        bool allMeshesCompleted = true;
        foreach (OVRSceneVolumeMeshFilter sceneVolumeMeshFilter in sceneVolumeMeshFilters)
        {
            if (sceneVolumeMeshFilter.IsCompleted == true)
            {
                roomMeshes.Add(sceneVolumeMeshFilter.transform.gameObject);
            }
            else
            {
                allMeshesCompleted = false;
            }
        }
        globalMeshUpdated = allMeshesCompleted;
    }

    private void GetWalls()
    {
        // Get the room walls
        sceneRoom = FindObjectOfType<OVRSceneRoom>();
        Debug.Log("RoomPrepManager - Finding walls");
        wallPlanes = sceneRoom.Walls;
        Debug.Log("RoomPrepManager - " + wallPlanes.Length.ToString() + " walls found");
    }

    private void Update()
    {
        if (sceneRoom != null)
        {
            if (globalMeshUpdated == true)
            {
                if (roomUpdated == false)
                {
                    GetRoomMeshes();
                    // Update walls
                    Debug.Log("RoomPrepManager - Updating walls");
                    UpdateWall(wallPlanes[0]);
                    UpdateWall(wallPlanes[1]);
                    // UpdateWall(wallPlanes[2]);
                    // foreach (OVRScenePlane wallPlane in wallPlanes)
                    // {
                    //     UpdateWall(wallPlane);
                    // }
                    Debug.Log("RoomPrepManager - Walls updated");
                    roomUpdated = true;
                }
            }
            else
            {
                GetRoomMeshes();
            }
        }
    }

    private void UpdateRoomMesh()
    {
        // Update mesh
        foreach (GameObject roomMesh in roomMeshes)
        {
            // Debug components
            Component[] components = roomMesh.GetComponents(typeof(Component));
            PrintDebugComponents(components);
            // CSGSubtractDirectly(roomMesh, subtractObject);
            // removeWallFromGlobalMesh(roomMesh, subtractObject);
            // removeWallFromGlobalMesh(roomMesh);
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
        Debug.Log("RoomPrepManager - Wall dimensions: " + wallDimensions.ToString());
        // Place wall prefab
        Debug.Log("RoomPrepManager - Create new wall");
        GameObject newWall = Instantiate(wallPrefab);
        newWall.transform.position = wall.transform.position;
        newWall.transform.rotation = wall.transform.rotation;
        // The transform is done in the child object to avoid issues when rotating
        GameObject cube = newWall.transform.Find("Cube").gameObject;
        cube.transform.localScale = new Vector3(wallDimensions.x, wallDimensions.y, cube.transform.localScale.z);
        // Debug.Log("RoomPrepManager - New wall location " + newWall.transform.position.ToString());
        // Debug.Log("RoomPrepManager - New wall rotation " + newWall.transform.rotation.ToString());
        // Remove wall intersections from global mesh
        // ===================
        // foreach (GameObject roomMesh in roomMeshes)
        // {
        removeWallFromGlobalMesh(roomMeshes[0], cube);
        // }
        // ===================
        // Break wall into sections that can be destroyed
        WallBreakable wb = newWall.GetComponent<WallBreakable>();
        if (wb != null)
        {
            wb.MakeBreakable();
        }
        // Deactivate original wall
        wall.SetActive(false);
        Debug.Log("RoomPrepManager - Original wall deactivated");

    }

    private void removeWallFromGlobalMesh(GameObject roomGlobalMesh, GameObject subtractWall)
    
    // CURRENT ISSUES:
    // 1. The resulting mesh is not in place (both position, rotation and scale)
    // 2. The resulting mesh takes time to generate. With all the walls the lag is too much
    // 3. The resutling mesh seems to be too complex (and there are some triangles cutting through the room).
    //    Is there a way to simplify this?
    
    // private void removeWallFromGlobalMesh(GameObject roomGlobalMesh)
    {
        // Check all components are available
        MeshFilter gmMeshFilter = roomGlobalMesh.GetComponent<MeshFilter>();
        Mesh gmMeshFilterSharedMesh = roomGlobalMesh.GetComponent<MeshFilter>()?.sharedMesh;
        Mesh gmMeshFilterMesh = roomGlobalMesh.GetComponent<MeshFilter>()?.mesh;
        // if (gmMeshFilterSharedMesh == null && gmMeshFilterMesh != null)
        // {
        //     Debug.Log("RoomPrepManager - Assigning mesh to shared mesh");
        //     gmMeshFilterSharedMesh = gmMeshFilterMesh;
        // }
        MeshRenderer gmMeshRenderer = roomGlobalMesh.GetComponent<MeshRenderer>();
        MeshCollider gmMeshCollider = roomGlobalMesh.GetComponent<MeshCollider>();
        MeshFilter wallMeshFilter = subtractWall.GetComponent<MeshFilter>();
        Mesh wallMeshFilterSharedMesh = subtractWall.GetComponent<MeshFilter>()?.sharedMesh;
        Mesh wallMeshFilterMesh = subtractWall.GetComponent<MeshFilter>()?.mesh;
        MeshRenderer wallMeshRenderer = subtractWall.GetComponent<MeshRenderer>();
        if (
            gmMeshFilterSharedMesh != null && gmMeshFilter != null && gmMeshRenderer != null
            && wallMeshFilterSharedMesh != null && wallMeshFilter != null && wallMeshRenderer != null
        )
        {
            try
            {
                Debug.Log("RoomPrepManager - Subtracting wall");
                // Subtract wall
                Model result = CSG.Subtract(roomGlobalMesh, subtractWall);
                // Update mesh, collider and material
                gmMeshFilter.sharedMesh = result.mesh;
                if (gmMeshCollider != null)
                {
                    gmMeshCollider.sharedMesh = result.mesh;
                }
                gmMeshRenderer.sharedMaterials = result.materials.ToArray();
                gmMeshRenderer.material = highlightMaterial;
                // Need to set the position, scale and rotation of object 1 to default,
                // as those are already taken into account in the resulting mesh
                // If this step is not done, the mesh will be adding the position, scale and rotation on top
                roomGlobalMesh.transform.position = new Vector3(0, 0, 0);
                roomGlobalMesh.transform.rotation = Quaternion.identity;
                // roomGlobalMesh.transform.localScale = new Vector3(1, 1, 1);
                Debug.Log("RoomPrepManager - Global mesh updated");
                }
            catch (Exception e)
            {
                Debug.LogError("ERROR - RoomPrepManager: " + e.ToString());
            }
        }
        else
        {
            // Log error on missing component
            Debug.LogError("RoomPrepManager - missing component");
            if (gmMeshFilterSharedMesh == null)
            {
                Debug.LogError("RoomPrepManager - missing SharedMesh from room mesh");
            }
            if (gmMeshFilter == null)
            {
                Debug.LogError("RoomPrepManager - missing MeshFilter from room mesh");
            }
            if (gmMeshRenderer == null)
            {
                Debug.LogError("RoomPrepManager - missing MeshRenderer from room mesh");
            }
            if (wallMeshFilterSharedMesh == null)
            {
                Debug.LogError("RoomPrepManager - missing SharedMesh from wall mesh");
            }
            if (wallMeshFilter == null)
            {
                Debug.LogError("RoomPrepManager - missing MeshFilter from wall mesh");
            }
            if (wallMeshRenderer == null)
            {
                Debug.LogError("RoomPrepManager - missing MeshRenderer from wall mesh");
            }
        }
    }

    private void CSGSubtractDirectly(GameObject gameObj1, GameObject gameObj2)
    {
        Debug.Log("RoomPrepManager - Subtracting meshes");
        // Subtract game object 2 from geme object 1
        Model result = CSG.Subtract(gameObj1, gameObj2);
        // Update mesh and material on game object 1
        MeshFilter gameObj1MeshFilter = gameObj1.GetComponent<MeshFilter>();
        MeshCollider gameObj1MeshCollider = gameObj1.GetComponent<MeshCollider>();
        MeshRenderer gameObj1MeshRenderer = gameObj1.GetComponent<MeshRenderer>();
        gameObj1MeshFilter.sharedMesh = result.mesh;
        if (gameObj1MeshCollider != null)
        {
            gameObj1MeshCollider.sharedMesh = result.mesh;
        }
        gameObj1MeshRenderer.sharedMaterials = result.materials.ToArray();
        gameObj1MeshRenderer.material = highlightMaterial;

        // Need to set the position, scale and rotation of object 1 to default,
        // as those are already taken into account in the resulting mesh
        // If this step is not done, the mesh will be adding the position, scale and rotation on top
        gameObj1.transform.position = new Vector3(0, 0, 0);
        gameObj1.transform.rotation = Quaternion.identity;
        gameObj1.transform.localScale = new Vector3(1, 1, 1);
        Debug.Log("RoomPrepManager - Updated");
    }

    private void PrintDebugComponents(Component[] components)
    {
        Debug.Log("RoomPrepManager - " + components.Length.ToString() + " components found");
        foreach (Component component in components)
        {
            // This has 3 children: Transform, OVRSceneAnchor and OVRSceneRoom
            Debug.Log("RoomPrepManager - " + component.GetType().Name.ToString());
        }
    }

}
