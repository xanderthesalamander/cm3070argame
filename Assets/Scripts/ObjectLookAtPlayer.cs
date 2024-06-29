using UnityEngine;
using UnityEngine.UI;

public class ObjectLookAtPlayer : MonoBehaviour
{
    void Update()
    {
        // Find CenterEyeTransform (center eye camera) and point at it
        Transform target = GameObject.Find("CenterEyeTransform").transform;
        transform.LookAt(target);
    }
}
