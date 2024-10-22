using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public int initialResources = 0;
    [SerializeField] private TextMeshProUGUI resourcesScreen;
    [Tooltip("Debugging output")]
    private int resources = 0;
    private int activeTurrets = 0;

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
        if (state == GameState.GameStart)
        {
            // Reset resources on game start
            resources = initialResources;
            DisplayResources();
        }
    }

    public int GetCurrentResources()
    {
        return resources;
    }

    public int GetCurrentActiveTurrets()
    {
        return activeTurrets;
    }

    public void AddResource(int value)
    {
        resources += value;
        DisplayResources();
    }

    public void RemoveResource(int value)
    {
        resources -= value;
        DisplayResources();
    }

    public void AddActiveTurret()
    {
        activeTurrets++;
    }

    public void RemoveActiveTurret()
    {
        activeTurrets--;
    }

    private void DisplayResources()
    {
        if (resourcesScreen != null)
        {
            resourcesScreen.text = resources.ToString();
        }
    }
}
