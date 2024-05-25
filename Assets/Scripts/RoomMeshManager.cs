using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class RoomMeshManager : MonoBehaviour
{
    
    [SerializeField] GameObject go1;
    [SerializeField] GameObject go2;

    private OVRSceneManager OVRsceneManager;
    private OVRSceneRoom sceneRoom;
    private OVRSceneVolumeMeshFilter[] sceneVolumeMeshFilters;
    private List<GameObject> roomMeshes = new List<GameObject>();
    private GameObject floor;
    private GameObject ceiling;

    void Awake()
    {
        // Get OVR scene manager
        OVRsceneManager = FindObjectOfType<OVRSceneManager>();
        // Subscribe to scene loaded event
        OVRsceneManager.SceneModelLoadedSuccessfully += SceneLoaded;
        CSGSubtractDirectly(go1, go2);
    }

    private void SceneLoaded()
    {
        // Find all OVRSceneVolumeMeshFilters (this should find the GLOBAL_MESH)
        sceneVolumeMeshFilters = Object.FindObjectsOfType<OVRSceneVolumeMeshFilter>();
        Debug.LogError("RoomMeshManager - " + sceneVolumeMeshFilters.Length.ToString() + " room meshes found");
        // Add to room meshes
        foreach (OVRSceneVolumeMeshFilter sceneVolumeMeshFilter in sceneVolumeMeshFilters)
        {
            roomMeshes.Add(sceneVolumeMeshFilter.transform.gameObject);
        }
        // Get floor and ceiling
        sceneRoom = FindObjectOfType<OVRSceneRoom>();
        floor = sceneRoom.Floor.gameObject;
        ceiling = sceneRoom.Ceiling.gameObject;
        updadeRoomMeshes();
    }

    // public void updadeRoomMeshes(GameObject[] subtractObjects)
    public void updadeRoomMeshes()
    {
        // Update mesh
        // foreach (GameObject roomMesh in roomMeshes)
        // {
        //     // removeWallFromGlobalMesh(roomMesh, subtractObjects);
        //     removeWallFromGlobalMesh(roomMesh);
        // }
        // CSGSubtractDirectly(go1, go2);
    }

    // private void removeWallFromGlobalMesh(GameObject roomGlobalMesh, GameObject[] subtractObjects)
    private void removeWallFromGlobalMesh(GameObject roomGlobalMesh)
    {
        Component[] components = roomGlobalMesh.GetComponents(typeof(Component));
        PrintDebugComponents(components);
        // Get mesh filter and mesh renderer for room 
        MeshFilter gmMeshFilter = roomGlobalMesh.GetComponent<MeshFilter>();
        MeshRenderer gmMeshRenderer = roomGlobalMesh.GetComponent<MeshRenderer>();
        if (gmMeshFilter == null)
        {
            Debug.LogError("RoomMeshManager - missing MeshFilter from room mesh");
        }
        if (gmMeshRenderer == null)
        {
            Debug.LogError("RoomMeshManager - missing gmMeshRenderer from room mesh");
        }
        // Get mesh filter and mesh renderer for walls
        List<MeshFilter> subtractObjectMeshFilters = new List<MeshFilter>();
        List<MeshRenderer> subtractObjectMeshRenderers = new List<MeshRenderer>();
        // foreach (GameObject subtractObject in subtractObjects)
        // {
        //     subtractObjectMeshFilters.Add(subtractObject.GetComponent<MeshFilter>());
        //     subtractObjectMeshRenderers.Add(subtractObject.GetComponent<MeshRenderer>());
        // }

        // Subtract ceiling
        Model result = CSG.Subtract(roomGlobalMesh, ceiling);
        // var composite = new GameObject();
        // composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        // composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        gmMeshFilter.sharedMesh = result.mesh;
        gmMeshRenderer.sharedMaterials = result.materials.ToArray();

        // Subtract floor
        result = CSG.Subtract(roomGlobalMesh, floor);
        // var composite = new GameObject();
        // composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        // composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        gmMeshFilter.sharedMesh = result.mesh;
        gmMeshRenderer.sharedMaterials = result.materials.ToArray();

        // // Initialize two new meshes in the scene
        // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // sphere.transform.localScale = Vector3.one * 1.3f;

        // // Perform boolean operation
        // Model result = CSG.Subtract(cube, sphere);

        // // Create a gameObject to render the result
        // var composite = new GameObject();
        // composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        // composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
    }

    public void CSGSubtractDirectly(GameObject gameObj1, GameObject gameObj2)
    {
        Debug.Log("RoomMeshManager - Subtracting meshes");
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
        // Need to set the position, scale and rotation of object 1 to default,
        // as those are already taken into account in the resulting mesh
        // If this step is not done, the mesh will be adding the position, scale and rotation on top
        gameObj1.transform.position = new Vector3(0, 0, 0);
        gameObj1.transform.rotation = Quaternion.identity;
        gameObj1.transform.localScale = new Vector3(1, 1, 1);
        Debug.Log("RoomMeshManager - Updated");
    }

    private void PrintDebugComponents(Component[] components)
    {
        Debug.Log("RoomMeshManager - " + components.Length.ToString() + " components found");
        foreach (Component component in components)
        {
            // This has 3 children: Transform, OVRSceneAnchor and OVRSceneRoom
            Debug.Log("RoomMeshManager - " + component.GetType().Name.ToString());
        }
    }
}
