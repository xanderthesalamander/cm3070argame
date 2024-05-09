using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArmControllerManager : MonoBehaviour
{
    // Screens
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject planningScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject debugScreen;
    // Debugging output
    [SerializeField] private TextMeshProUGUI debugScreenText;
    
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
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DeactivateAllScreens();
    }

    // Update is called once per frame
    void Update()
    {
        DebugScreen();
    }

    void DebugScreen()
    {
        if (debugScreenText != null)
        {
            string debuggingText = "Debug:";
            debuggingText += "\n";
            debuggingText += "\nScreen statuses:";
            debuggingText += "\nstartScreen: " + startScreen?.activeSelf.ToString();
            debuggingText += "\nplanningScreen: " + planningScreen?.activeSelf.ToString();
            debuggingText += "\ngameOverScreen: " + gameOverScreen?.activeSelf.ToString();
            debuggingText += "\nvictoryScreen: " + victoryScreen?.activeSelf.ToString();
            debuggingText += "\nsettingsScreen: " + settingsScreen?.activeSelf.ToString();
            debuggingText += "\ndebugScreen: " + debugScreen?.activeSelf.ToString();
            debugScreenText.text = debuggingText;
        }
    }

    void DeactivateAllScreens()
    {
        // Set inactive
        startScreen?.SetActive(false);
        planningScreen?.SetActive(false);
        gameOverScreen?.SetActive(false);
        victoryScreen?.SetActive(false);
        settingsScreen?.SetActive(false);
        // debugScreen?.SetActive(false);
    }

}
