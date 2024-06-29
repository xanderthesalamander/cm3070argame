using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float forceMax = 1.0f;
    [SerializeField] float moveProbabilityThreshold = 0.99f;
    [SerializeField] GameObject[] enemyPrefabs;
    [Tooltip("Array of enemy prefabs")]
    [SerializeField] float baseProbabilityThreshold = 0.999f;
    [SerializeField] AudioClip spawnSound;
    [SerializeField] AudioSource audioSource;
    private WaveManager waveManager;
    // The maximum allowed velocity. The velocity will be clamped to keep 
    // it from exceeding this value.
    private float maxVelocity = 2.0f;
    // A cached copy of the squared max velocity. Used in FixedUpdate.
    private float sqrMaxVelocity = 4.0f;

    void Start()
    {
        // resourceManager = GameObject.Find("Resource Manager").GetComponent<ResourceManager>();
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
    }
    void Update()
    {        
        // Move
        MoveRandomly();
        // Spawn enemy randomly
        float probabilityThreshold = baseProbabilityThreshold;
        if (Random.Range(0.0f, 1.0f) > probabilityThreshold)
        {
            // Spawn random enemy from array
            int i = Random.Range(0, enemyPrefabs.Length);
            Debug.Log("EnemySpawner - Spawn enemy in array index " + i.ToString());
            spawnEnemy(enemyPrefabs[i]);
            waveManager.AddSpawnedEnemy();
        }
    }

    public void spawnEnemy(GameObject enemy) {
        // Play sound
        audioSource.PlayOneShot(spawnSound);
        // Create enemy
        GameObject spawnedEnemy = Instantiate(enemy);
        // Place it in current position
        spawnedEnemy.transform.position = transform.position;
    }

    private void MoveRandomly()
    {
        if (Random.Range(0.0f, 1.0f) > moveProbabilityThreshold)
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


    // // FixedUpdate is a built-in unity function that is called every fixed framerate frame.
    // // We use FixedUpdate instead of Update here because the docs recommend doing so when
    // // dealing with rigidbodies.
    // // For more info, see:
    // // http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.FixedUpdate.html 
    // private void FixedUpdate() {
    //     var v = rb.velocity;
    //     // Clamp the velocity, if necessary
    //     // Use sqrMagnitude instead of magnitude for performance reasons.
    //     if(v.sqrMagnitude > sqrMaxVelocity) // Equivalent to: rigidbody.velocity.magnitude > maxVelocity, but faster.
    //     {
    //         // Vector3.normalized returns this vector with a magnitude 
    //         // of 1. This ensures that we're not messing with the 
    //         // direction of the vector, only its magnitude.
    //         rb.velocity = v.normalized * maxVelocity;
    //     }   
    // }
}
