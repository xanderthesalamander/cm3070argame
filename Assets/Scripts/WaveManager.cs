using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    // public GameObject enemyA;
    public GameObject[] enemyPrefabs;
    [Tooltip("Array of enemy prefabs")]
    public List<GameObject> enemySpawnPoints;
    [Tooltip("Array of enemy prefabs")]
    [SerializeField] private int enemiesWave1 = 10;
    [Tooltip("Number of enemies to spawn on first wave")]
    [SerializeField] private int incrementalEnemies = 5;
    [Tooltip("Number incremental enemies per wave")]
    [SerializeField] private int maxWaveLevel = 2;
    [Tooltip("Number of waves before game ends")]
    [SerializeField] private TextMeshProUGUI debugScreenText;
    [Tooltip("Debugging output")]
    private int waveLevel = 0;
    private bool waveActive = false;
    private int currentNumberOfEnemies = 0;
    private int enemiesToBeSpawned = 0;
    private int enemiesSpawned = 0;
    // This should be a list
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
            return;
        }
        if (state == GameState.GameStart)
        {
            // Start game
            DestroyAllEnemies();
            waveLevel = 0;
            waveActive = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        DebugManager();
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
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
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
        enemiesToBeSpawned = waveLevel * incrementalEnemies + enemiesWave1;
        enemiesSpawned = 0;
        activateEnemySpawnPoints();
    }

    private void activateEnemySpawnPoints()
    {
        foreach (GameObject enemySpawnPoint in enemySpawnPoints)
        {
            if (enemySpawnPoint != null)
            {
                enemySpawnPoint.SetActive(true);
            }
            else
            {
                Debug.LogError("WaveManager - Enemy Spawn Point missing");
            }
        }
    }

    private void deactivateEnemySpawnPoint()
    {
        foreach (GameObject enemySpawnPoint in enemySpawnPoints)
        {
            enemySpawnPoint.SetActive(false);
        }
    }

    private void DebugManager()
    {
        if (debugScreenText != null)
        {
            string debuggingText = "";
            debuggingText += "Wave Manager:";
            debuggingText += "\nwaveActive: " + waveActive.ToString();
            debuggingText += "\nwaveLevel: " + waveLevel.ToString();
            debuggingText += "\ncurrentNumberOfEnemies: " + currentNumberOfEnemies.ToString();
            debuggingText += "\nenemiesToBeSpawned: " + enemiesToBeSpawned.ToString();
            debuggingText += "\nenemiesSpawned: " + enemiesSpawned.ToString();
            debuggingText += "\n";
            string enemiesN = "";
            if (enemies != null)
            {
                enemiesN = enemies.Length.ToString();
            }
            debuggingText += "\n# of enemies alive: " + enemiesN;
            debuggingText += "\n";
            debugScreenText.text = debuggingText;
        }
    }
}
