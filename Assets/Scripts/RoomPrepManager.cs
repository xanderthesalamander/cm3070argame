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
    [SerializeField] GameObject cellPrefab;
    private List<GameObject> cells = new List<GameObject>();
    private float cellSize;

    // ==== TEST
    // [SerializeField] GameObject go1;
    // [SerializeField] GameObject go2;
    [SerializeField] Material highlightMaterial;
    // ==== TEST END
    
    private OVRSceneManager OVRsceneManager;
    private OVRSceneRoom sceneRoom;
    private OVRScenePlane[] wallPlanes;
    private OVRScenePlane floor;
    private OVRScenePlane ceiling;
    private OVRSceneVolumeMeshFilter[] sceneVolumeMeshFilters;
    private List<GameObject> roomMeshes = new List<GameObject>();
    // private List<GameObject> newWalls;

    private bool roomUpdated = false;
    private bool globalMeshUpdated = false;
    private bool globalMeshReplaced = false;

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
        // CSGSubtractDirectly(go1, go2);
        // ==== TEST END
    }

    private void SceneLoaded()
    {
        GetRoomMeshes();
        GetRoomPlanes();
    }

    private void GetRoomMeshes()
    {
        // Get all OVRSceneVolumeMeshFilters (this should find the GLOBAL_MESH)
        sceneVolumeMeshFilters = UnityEngine.Object.FindObjectsOfType<OVRSceneVolumeMeshFilter>();
        // Add to room meshes
        // roomMeshes = new List<GameObject>();
        bool allMeshesCompleted = true;
        foreach (OVRSceneVolumeMeshFilter sceneVolumeMeshFilter in sceneVolumeMeshFilters)
        {
            if (sceneVolumeMeshFilter.IsCompleted == true)
            {
                // roomMeshes.Add(sceneVolumeMeshFilter.transform.gameObject);
            }
            else
            {
                allMeshesCompleted = false;
            }
        }
        globalMeshUpdated = allMeshesCompleted;
    }

    private void GetRoomPlanes()
    {
        // Get the room walls
        sceneRoom = FindObjectOfType<OVRSceneRoom>();
        Debug.Log("RoomPrepManager - Finding walls");
        wallPlanes = sceneRoom.Walls;
        Debug.Log("RoomPrepManager - " + wallPlanes.Length.ToString() + " walls found");
        floor = sceneRoom.Floor;
        if (floor != null)
        {
            Debug.Log("RoomPrepManager - Floor found");
        }
        ceiling = sceneRoom.Ceiling;
        if (ceiling != null)
        {
            Debug.Log("RoomPrepManager - Ceiling found");
        }
    }

    private void Update()
    {
        if (sceneRoom != null)
        {
            if (globalMeshUpdated == true)
            {
                if (roomUpdated == false)
                {
                    // Update walls
                    Debug.Log("RoomPrepManager - Updating walls");
                    foreach (OVRScenePlane wallPlane in wallPlanes)
                    {
                        UpdateWall(wallPlane);
                    }
                    Debug.Log("RoomPrepManager - Walls updated");
                    roomUpdated = true;
                }
                
                if (globalMeshReplaced == false)
                {
                    roomMeshes = new List<GameObject>();
                    RoomCellsCreate();
                    RoomCellsFilter();
                    RemoveGlobalMesh();
                    globalMeshReplaced = true;
                }
                
                
            }
            else
            {
                GetRoomMeshes();
            }
        }
    }

    private void RoomCellsCreate()
    {
        // Create cells all around the room
        // Use floor and ceiling to get dimension
        float xmin = floor.transform.position.x - floor.transform.lossyScale.x / 2;
        float xmax = floor.transform.position.x + floor.transform.lossyScale.x / 2;
        float ymin = floor.transform.position.y;
        float ymax = ceiling.transform.position.y;
        float zmin = floor.transform.position.z - floor.transform.lossyScale.z / 2;
        float zmax = floor.transform.position.z + floor.transform.lossyScale.z / 2;
        float vol_width = xmax - xmin;
        float vol_height = ymax - ymin;
        float vol_length = zmax - zmin;
        vol_width = vol_height * 4;
        vol_length = vol_height * 4;
        // TODO: Not ideal
        cellSize = cellPrefab.transform.lossyScale.x;
        float cell_width = cellSize;
        float cell_height = cellSize;
        float cell_length = cellSize;
        //
        float xitems = vol_width / cellSize;
        float yitems = vol_height / cellSize;
        float zitems = vol_length / cellSize;
        int nx = (int)xitems;
        int ny = (int)yitems;
        int nz = (int)zitems;
        Vector3 startPos = new Vector3(xmin, ymin, zmin) - new Vector3(vol_width/2, 0, vol_length/2) + new Vector3(cell_width / 2, cell_height / 2, cell_length / 2);;
        // This is to resize the cell prefab
        Vector3 prefabSize = cellPrefab.GetComponent<Renderer>().bounds.size;
        for (int i = 0; i < nx; i++)
        {
            for (int j = 0; j < ny; j++)
            {
                for (int k = 0; k < nz; k++)
                {
                    Vector3 cellPosition = startPos + new Vector3(i * cell_width, j * cell_height, k * cell_length);
                    Collider[] hitColliders = Physics.OverlapSphere(cellPosition, (cellSize / 2) - 0.01f);
                    foreach (Collider other in hitColliders)
                    {
                        if (other.gameObject.tag == "RoomGlobalMesh")
                        {
                            GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity);
                            cells.Add(cell);
                            break;
                        }
                    }
                }
            }
        }
        Debug.Log("RoomPrepManager - " + cells.Count.ToString() + " cells created");
        Debug.LogError("RoomPrepManager - " + cells.Count.ToString() + " cells created");
    }

    private void RoomCellsFilter()
    {
        int cellDestroyed = 0;
        // Only keep the cells intersecting with the room mesh
        string[] TagList = {"Wall", "Ceiling"};
        foreach (GameObject cell in cells)
        {
            Collider[] hitColliders = Physics.OverlapSphere(cell.transform.position, (cellSize / 2) - 0.01f);
            bool breakTag = false;
            foreach (Collider other in hitColliders)
            {
                foreach (string tag in TagList)
                {
                    if (other.gameObject.tag == tag)
                    {
                        Destroy(cell);
                        breakTag = true;
                        cellDestroyed++;
                        break;
                    }
                }
                if (breakTag)
                {
                    break;
                }
            }
        }
        Debug.Log("RoomPrepManager - " + cellDestroyed.ToString() + " cells removed");
        Debug.LogError("RoomPrepManager - " + cellDestroyed.ToString() + " cells removed");
    }

    private void RemoveGlobalMesh()
    {
        GameObject[] globalMeshes = GameObject.FindGameObjectsWithTag("RoomGlobalMesh");
        foreach (GameObject go in globalMeshes)
        {
            go.SetActive(false);
            Debug.Log("RoomPrepManager - Global mesh deactivated");
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
        // Assign same OVR anchor
        OVRSceneAnchor newAnchor = newWall.GetComponent<OVRSceneAnchor>();
        newAnchor = wallAnchor;
        // The transform is done in the child object to avoid issues when rotating
        GameObject cube = newWall.transform.Find("Cube").gameObject;
        cube.transform.localScale = new Vector3(wallDimensions.x, wallDimensions.y, cube.transform.localScale.z);
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

    // private void CSGSubtractDirectly(GameObject gameObj1, GameObject gameObj2)
    // {
    //     Debug.Log("RoomPrepManager - Subtracting meshes");
    //     // Subtract game object 2 from geme object 1
    //     Model result = CSG.Subtract(gameObj1, gameObj2);
    //     // Update mesh and material on game object 1
    //     MeshFilter gameObj1MeshFilter = gameObj1.GetComponent<MeshFilter>();
    //     MeshCollider gameObj1MeshCollider = gameObj1.GetComponent<MeshCollider>();
    //     MeshRenderer gameObj1MeshRenderer = gameObj1.GetComponent<MeshRenderer>();
    //     gameObj1MeshFilter.sharedMesh = result.mesh;
    //     if (gameObj1MeshCollider != null)
    //     {
    //         gameObj1MeshCollider.sharedMesh = result.mesh;
    //     }
    //     gameObj1MeshRenderer.sharedMaterials = result.materials.ToArray();
    //     gameObj1MeshRenderer.material = highlightMaterial;

    //     // Need to set the position, scale and rotation of object 1 to default,
    //     // as those are already taken into account in the resulting mesh
    //     // If this step is not done, the mesh will be adding the position, scale and rotation on top
    //     gameObj1.transform.position = new Vector3(0, 0, 0);
    //     gameObj1.transform.rotation = Quaternion.identity;
    //     gameObj1.transform.localScale = new Vector3(1, 1, 1);
    //     Debug.Log("RoomPrepManager - Updated");
    // }

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
