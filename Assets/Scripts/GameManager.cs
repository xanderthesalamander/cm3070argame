using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;
    [Tooltip("Current game state")]
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] private TextMeshProUGUI debugScreenText;
    [Tooltip("Debugging output")]
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Initial state
        UpdateGameState(GameState.SetupState);
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.SetupState:
                break;
            case GameState.TutorialState:
                break;
            case GameState.GameStart:
                ResetGame();
                break;
            case GameState.PlayerPlanningState:
                break;
            case GameState.EnemyWaveState:
                break;
            case GameState.VictoryState:
                break;
            case GameState.LoseState:
                break;
        }
        // Update debugger
        DebugManager();
        // Notify that the state has been updated to any subscriber
        OnGameStateChanged?.Invoke(newState);
    }

    public void startGame()
    {
        // Make this function available in the unity interface
        UpdateGameState(GameState.GameStart);
    }

    public void startPlayerPlanningState()
    {
        // Make this function available in the unity interface
        UpdateGameState(GameState.PlayerPlanningState);
    }

    public void startEnemyWaveState()
    {
        // Make this funciton available in the unity interface
        UpdateGameState(GameState.EnemyWaveState);
    }

    private void DestroyAllEnemies()
    {

    }

    private void ResetGame()
    {
        
    }

    private void DebugManager()
    {
        if (debugScreenText != null)
        {
            string debuggingText = "";
            debuggingText += "Game state: " + state.ToString();
            debuggingText += "\n";
            debugScreenText.text = debuggingText;
        }
    }
}

public enum GameState {
    SetupState,
    TutorialState,
    GameStart,
    PlayerPlanningState,
    EnemyWaveState,
    VictoryState,
    LoseState,
}