using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMoveRandom : MonoBehaviour
{
    [SerializeField] float forceMax = 1.0f;
    [SerializeField] float probabilityThreshold = 0.99f;
    [SerializeField] Rigidbody rb;
    
    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("RBMoveRandom - No rigidbody found");
        }
    }

    void Update()
    {
        if (Random.Range(0.0f, 1.0f) > probabilityThreshold)
        {
            // Add random force
            rb.AddForce(
                Random.Range(-forceMax, forceMax),
                Random.Range(-forceMax, forceMax),
                Random.Range(-forceMax, forceMax),
                ForceMode.Impulse
            );
        }
    }
}
