using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // public GameObject enemyA;
    public GameObject[] enemyPrefabs;
    [Tooltip("Array of enemy prefabs")]
    public List<GameObject> enemySpawnPoints;
    [Tooltip("Array of enemy prefabs")]
    [SerializeField] private int maxWaveLevel = 10;
    private int waveLevel = 0;
    private bool waveActive = false;
    private int currentNumberOfEnemies = 0;
    private int enemiesToBeSpawned = 0;
    private int enemiesSpawned = 0;
    private GameObject[] enemies;

    void Awake()
    {
        // Subscribe to the game manager
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }


    void OnDestroy()
    {
        // Unsubscribe to the game manager
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        if (state == GameState.EnemyWaveState)
        {
            // Start wave
            waveLevel++;
            startNewWave();
            waveActive = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // During the wave
        if (waveActive)
        {
            UpdateEnemyList();
            // Enough enemies have been spawned
            if (enemiesSpawned >= enemiesToBeSpawned)
            {
                deactivateEnemySpawnPoint();
                // All enemis in current wave have been killed
                if (currentNumberOfEnemies == 0)
                {
                    waveActive = false;
                    if (waveLevel == maxWaveLevel)
                    {
                        // Won the game
                        waveLevel = 0;
                        GameManager.instance.UpdateGameState(GameState.VictoryState);
                    }
                    else
                    {
                        // Update game state
                        GameManager.instance.UpdateGameState(GameState.PlayerPlanningState);
                    }
                }
            }
        }
    }

    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    public void stopAndResetWave()
    {
        deactivateEnemySpawnPoint();
        waveLevel = 0;
        waveActive = false;
    }

    public void AddSpawnedEnemy()
    {
        enemiesSpawned++;
    }

    public int getWaveLevel()
    {
        return waveLevel;
    }

    public bool isWaveActive()
    {
        return waveActive;
    }

    private void UpdateEnemyList()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        currentNumberOfEnemies = enemies.Length;
    }

    private void startNewWave()
    {
        enemiesToBeSpawned = waveLevel * 10 + 10;
        enemiesSpawned = 0;
        activateEnemySpawnPoints();
    }

    private void activateEnemySpawnPoints()
    {
        foreach (GameObject enemySpawnPoint in enemySpawnPoints)
        {
            enemySpawnPoint.SetActive(true);
        }
    }

    private void deactivateEnemySpawnPoint()
    {
        foreach (GameObject enemySpawnPoint in enemySpawnPoints)
        {
            enemySpawnPoint.SetActive(false);
        }
    }
}
