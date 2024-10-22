using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [Tooltip("Array of enemy prefabs")]
    [SerializeField] float baseProbabilityThreshold = 0.999f;
    [SerializeField] AudioClip spawnSound;
    [SerializeField] AudioSource audioSource;
    private WaveManager waveManager;

    void Start()
    {
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
    }
    void Update()
    {
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
        // Disable NavMeshAgent (this is to avoid teleporting issue with navMesh)
        spawnedEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        // Place it in current position
        spawnedEnemy.transform.position = transform.position;
        // Enable NavMeshAgent (to allow enemy to move)
        spawnedEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
    }

}
