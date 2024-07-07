using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class RoomPrepManager : MonoBehaviour
{
    
    private MRUKRoom mrukComponent;
    private GameObject mrukRoom;

    public void PrepareRoom()
    {
        Debug.Log("RoomPrepManager - Preparing room...");
        MRUK mruk = FindObjectOfType<MRUK>();
        // TODO: Not sure if this is enough (possibly loop through GetRooms())
        // https://developer.oculus.com/documentation/unity/unity-mr-utility-kit-features/
        mrukComponent = mruk.GetCurrentRoom();
        mrukRoom = mrukComponent.gameObject;
        FixRoomChildren();
        Debug.Log("RoomPrepManager - Room complete");
    }

    private void FixRoomChildren()
    {
        Debug.Log("RoomPrepManager - Room name: " + mrukRoom.name);
        Debug.Log("RoomPrepManager - Room children: " + mrukRoom.transform.childCount.ToString());
        List<MRUKAnchor> wallAnchores = mrukComponent.GetWallAnchors();
        Debug.Log("RoomPrepManager - Walls: " + wallAnchores.Count.ToString());
        foreach (Transform child in mrukRoom.transform)
        {
            Debug.Log("RoomPrepManager - Working on " + child.name);
            string[] objNames = {
                "WALL_FACE",
                "INVISIBLE_WALL_FACE",
                "CEILING",
                "FLOOR",
                "GLOBAL_MESH"
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
        }
        else
        {
            Debug.LogError("RoomPrepManager - Could not find both children for " + objName);
        }
    }
}
