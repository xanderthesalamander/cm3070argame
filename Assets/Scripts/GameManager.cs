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
    [SerializeField] RoomPrepManager roomPrepManager;
    [Tooltip("Room prep manager script")]
    
    // TODO: Remove
    // Only for testing in Unity
    public bool updateRoom = false;
    // _____

    private int score = 0;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Initial state
        UpdateGameState(GameState.SetupState);
    }

    // TODO: Remove
    // Only for testing in Unity
    void Update()
    {
        if (updateRoom)
        {
            Debug.Log("GameManager - Room Preparation");
            roomPrepManager.PrepareRoom();
            updateRoom = false;
        }
    }
    // _____

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.SetupState:
                break;
            case GameState.RoomPrepState:
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

    public void startRoomPrep()
    {
        // Make this function available in the unity interface
        UpdateGameState(GameState.RoomPrepState);
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

    public void IncreaseScore(int value)
    {
        score += value;
    }

    public void ReduceScore(int value)
    {
        score -= value;
    }

    public int getScore()
    {
        return score;
    }

    private void ResetGame()
    {
        score = 0;
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
    RoomPrepState,
    TutorialState,
    GameStart,
    PlayerPlanningState,
    EnemyWaveState,
    VictoryState,
    LoseState,
}