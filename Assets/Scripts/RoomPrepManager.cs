using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using Parabox.CSG;

public class RoomPrepManager : MonoBehaviour
{
    [SerializeField] private GameObject wallBreakablePrefab;
    [SerializeField] private GameObject wallProtectionLayer;
    [SerializeField] float vertexRadius = 0.4f;
    [SerializeField] private GameObject placeholder;
    // [SerializeField] private GameObject cellPrefab;
    // private List<GameObject> cells = new List<GameObject>();
    // private float cellSize;
    private MRUKRoom mrukRoom;
    private GameObject mrukRoomGO;

    private void Start()
    {
        wallProtectionLayer.SetActive(false);
    }
    
    public void PrepareRoom()
    {
        Debug.Log("RoomPrepManager - Preparing room...");
        MRUK mruk = FindObjectOfType<MRUK>();
        // TODO: Not sure if this is enough (possibly loop through GetRooms())
        // https://developer.oculus.com/documentation/unity/unity-mr-utility-kit-features/
        mrukRoom = mruk.GetCurrentRoom();
        mrukRoomGO = mrukRoom.gameObject;
        FixRoomChildren();
        FixGlobalMesh();
        Debug.Log("RoomPrepManager - Room complete");
    }

    private void FixRoomChildren()
    {
        Debug.Log("RoomPrepManager - Room name: " + mrukRoomGO.name);
        Debug.Log("RoomPrepManager - Room children: " + mrukRoomGO.transform.childCount.ToString());
        wallProtectionLayer.SetActive(true);
        // List<MRUKAnchor> wallAnchores = mrukRoom.GetWallAnchors();
        // Debug.Log("RoomPrepManager - Walls: " + wallAnchores.Count.ToString());
        foreach (Transform child in mrukRoomGO.transform)
        {
            Debug.Log("RoomPrepManager - Working on " + child.name);
            string[] objNames = {
                "WALL_FACE",
                "CEILING",
                "FLOOR",
                // "GLOBAL_MESH"
            };
            foreach (string objName in objNames)
            {
                if (child.name.Contains(objName))
                {
                    StealDefaultMeshAndDestroyOriginal(child, objName);
                    break;
                }
            }
        }
        wallProtectionLayer.SetActive(false);
    }

    private void StealDefaultMeshAndDestroyOriginal(Transform objTransform, string objName)
    {
        Debug.Log("RoomPrepManager - Replacing " + objName);
        GameObject defaultObj = null;
        GameObject newObj = null;
        // GameObject childGO = child.gameObject;
        foreach (Transform child in objTransform)
        {
            Debug.Log("RoomPrepManager - " + child.name.ToString());
            if (child.gameObject.name.Contains(objName) && child.gameObject.name.Contains("_EffectMesh"))
            {
                defaultObj = child.gameObject;
            }
            else
            {
                newObj = child.gameObject;
            }
        }
        if (defaultObj != null && newObj != null)
        {
            // Copy mesh into newWall
            Mesh wallMesh = defaultObj.GetComponent<MeshFilter>().sharedMesh;
            newObj.GetComponent<MeshFilter>().sharedMesh = wallMesh;
            newObj.GetComponent<MeshCollider>().sharedMesh = wallMesh;
            // Get rid of old wall
            Destroy(defaultObj);
            if (objName.Contains("WALL"))
            {
                // Apply changes to wall
                SetBreakableWall(newObj, wallProtectionLayer);
                // Destroy newWall
                Destroy(newObj);
            }
        }
        else
        {
            Debug.LogError("RoomPrepManager - Could not find both children for " + objName);
        }
    }

    private void FixGlobalMesh()
    {
        MRUKAnchor globalMeshParent = mrukRoom.GetGlobalMeshAnchor();
        // MRUKAnchor globalMeshParent = mrukRoom.GetFloorAnchor();
        
        if (globalMeshParent != null)
        {
            GameObject globalMeshObj = globalMeshParent.gameObject.transform.GetChild(0).gameObject;
            Mesh mesh = globalMeshObj.GetComponent<MeshFilter>().sharedMesh;
            Mesh newMesh = RemoveCollidingTrinagles(mesh, globalMeshObj);
            globalMeshObj.GetComponent<MeshFilter>().sharedMesh = newMesh;
            globalMeshObj.GetComponent<MeshCollider>().sharedMesh = newMesh;
            Debug.LogError("RoomPrepManager - FixGlobalMesh - GLOBAL MESH updated");
        }
        else
        {
            Debug.LogError("RoomPrepManager - FixGlobalMesh - GLOBAL MESH not found");
        }
    }

    private Mesh RemoveCollidingTrinagles(Mesh mesh, GameObject go)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Debug.LogError("RoomPrepManager - RemoveCollidingTrinagles - Initial triangles: " + triangles.Length.ToString());
        List<int> newTriangles = new List<int>();
        // Looping trough each triangle
        int i = 0;
        while (i < triangles.Length)
        {
            bool keep = true;
            // Get triangle points (world position)
            Vector3 p1 = go.transform.TransformPoint(vertices[triangles[i]]);
            Vector3 p2 = go.transform.TransformPoint(vertices[triangles[i+1]]);
            Vector3 p3 = go.transform.TransformPoint(vertices[triangles[i+2]]);
            // Get triangle center
            Vector3 c = (p1 + p2 + p3) / 3;
            // Calculate
            Collider[] hitColliders = Physics.OverlapSphere(c, vertexRadius);
            // GameObject ph = Instantiate(placeholder);
            // ph.transform.position = c;
            foreach (Collider other in hitColliders)
            {
                if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Ceiling")
                {
                    keep = false;
                    break;
                }
            }
            if (keep)
            {
                newTriangles.AddRange(new int[] {triangles[i], triangles[i+1], triangles[i+2]});
            }
            // Next triangle
            i = i + 3;
        }
        Debug.LogError("RoomPrepManager - RemoveCollidingTrinagles - Final triangles: " + newTriangles.Count.ToString());
        mesh.vertices = vertices;
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateBounds();
        Debug.LogError("RoomPrepManager - RemoveCollidingTrinagles - Completed");
        return mesh;
    }

    private void SetBreakableWall(GameObject wall, GameObject protectedLayer)
    {
        Debug.Log("RoomPrepManager - Wall update");
        // Get wall protected layer
        Model result = CSG.Intersect(wall, protectedLayer);
        GameObject wallProtected = Instantiate(wall, wall.transform.parent.transform);
        wallProtected.name = "Wall Protected";
        MeshFilter wpMeshFilter = wallProtected.GetComponent<MeshFilter>();
        MeshCollider wpMeshCollider = wallProtected.GetComponent<MeshCollider>();
        // Need to set the position, scale and rotation of object 1 to default,
        // as those are already taken into account in the resulting mesh
        // If this step is not done, the mesh will be adding the position, scale and rotation on top
        wallProtected.transform.position = new Vector3(0, 0, 0);
        wallProtected.transform.rotation = Quaternion.identity;
        wallProtected.transform.localScale = new Vector3(1, 1, 1);
        if (wpMeshFilter != null)
        {
            wpMeshFilter.sharedMesh = result.mesh;
        }
        if (wpMeshCollider != null)
        {
            wpMeshCollider.sharedMesh = result.mesh;
        }
        // Get wall destructible section
        result = CSG.Subtract(wall, protectedLayer);
        GameObject wallBreakable = Instantiate(wallBreakablePrefab, wall.transform.parent.transform);
        wallBreakable.name = "Wall Breakable";
        MeshFilter wbMeshFilter = wallBreakable.GetComponent<MeshFilter>();
        MeshCollider wbMeshCollider = wallBreakable.GetComponent<MeshCollider>();
        if (wbMeshFilter != null)
        {
            wbMeshFilter.sharedMesh = result.mesh;
        }
        if (wbMeshCollider != null)
        {
            wbMeshCollider.sharedMesh = result.mesh;
        }
        // Need to set the position, scale and rotation of object 1 to default,
        // as those are already taken into account in the resulting mesh
        // If this step is not done, the mesh will be adding the position, scale and rotation on top
        wallBreakable.transform.position = new Vector3(0, 0, 0);
        wallBreakable.transform.rotation = Quaternion.identity;
        wallBreakable.transform.localScale = new Vector3(1, 1, 1);
        // // Remove original wall
        Destroy(wall);
        // wall.SetActive(false);
        Debug.Log("RoomPrepManager - Updated");
    }
    
}
