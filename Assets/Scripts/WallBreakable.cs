using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreakable : MonoBehaviour
{
    
    [SerializeField] Material material;
    [Tooltip("The material to be changed on collision")]

    [SerializeField] string[] tags;
    [Tooltip("The tags that trigger the collision")]
    

    private void OnCollisionEnter(Collision collision)
    {
        // When hit by a something in the tags
        foreach (string tag in tags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                Destroy(gameObject);
                // gameObject.GetComponent<MeshRenderer>().materials[0] = material;
                // gameObject.GetComponent<MeshCollider>().enabled = false;
                // break;
            }
        }
    }
}
