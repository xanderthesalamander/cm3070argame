using UnityEngine;
using UnityEngine.UI;

public class ObjectLookAtPlayer : MonoBehaviour
{
    void Update()
    {
        // Find Player (center eye camera) and point at it
        Transform target = GameObject.Find("Player").transform;
        transform.LookAt(target);
    }
}
