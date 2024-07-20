using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArmControllerManager : MonoBehaviour
{
    // Screens
    [Header("Screens")]
    [SerializeField] private GameObject setupScreen;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject planningScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private GameObject debugScreen;
    private GameObject currentScreen;
    // Functionalities
    [Header("Functionalities")]
    [SerializeField] private GameObject placePrinter;
    [SerializeField] private GameObject placeEnemy;
    [SerializeField] private GameObject debugRay;
    [SerializeField] private GameObject tutorials;
    // Buttons
    [Header("Buttons")]
    [SerializeField] private GameObject toggleScreenButton;
    [SerializeField] private GameObject tutorialButton;
    [SerializeField] private GameObject fightButton;
    // Debugging output
    [Header("Tutorials")]
    [SerializeField] private GameObject tutorialPressButton;
    [SerializeField] private GameObject tutorialPlacePrinter;
    // Debugging output
    [Header("Screens Text Output")]
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI victoryScoreText;
    [SerializeField] private TextMeshProUGUI debugScreenText;
    
    void Awake()
    {
        Troubleshooting();
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
        if (state == GameState.SetupState)
        {
            // Game setup - User to press start
            DeactivateAllScreens();
            DeactivateAllFunctionalities();
            DeactivateAllButtons();
            updateCurrentScreen(setupScreen);
            currentScreen?.SetActive(true);
            DeactivateAllTutorials();
            tutorialPressButton?.SetActive(true);
        }
        if (state == GameState.GameStart)
        {
            // Game has started - user to place 3D printer
            DeactivateAllScreens();
            DeactivateAllFunctionalities();
            updateCurrentScreen(startScreen);
            placePrinter?.SetActive(true);
            placePrinter?.GetComponent<PlaceObject>().PreviewOn();
            currentScreen?.SetActive(true);
            DeactivateAllButtons();
            toggleScreenButton?.SetActive(true);
            DeactivateAllTutorials();
            tutorialPlacePrinter?.SetActive(true);
        }
        if (state == GameState.PlayerPlanningState)
        {
            // Planning phase
            DeactivateAllScreens();
            DeactivateAllFunctionalities();
            updateCurrentScreen(planningScreen);
            currentScreen?.SetActive(true);
            // Deactivate function and button
            placePrinter?.GetComponent<PlaceObject>().PreviewOff();
            placePrinter?.SetActive(false);
            DeactivateAllButtons();
            toggleScreenButton?.SetActive(true);
            tutorialButton?.SetActive(true);
            fightButton?.SetActive(true);
            // placePrinterButton?.SetActive(false);
            DeactivateAllTutorials();
        }
        if (state == GameState.EnemyWaveState)
        {
            // Enemy wave
            DeactivateAllScreens();
            DeactivateAllFunctionalities();
            DeactivateAllButtons();
            tutorialButton?.SetActive(true);
        }
        if (state == GameState.VictoryState)
        {
            // Victory
            UpdateScoreTexts();
            DeactivateAllScreens();
            DeactivateAllFunctionalities();
            DeactivateAllButtons();
            updateCurrentScreen(victoryScreen);
            currentScreen?.SetActive(true);
        }
        if (state == GameState.LoseState)
        {
            // Game Over
            UpdateScoreTexts();
            DeactivateAllScreens();
            DeactivateAllFunctionalities();
            DeactivateAllButtons();
            updateCurrentScreen(gameOverScreen);
            currentScreen?.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DeactivateAllScreens();
        DeactivateAllFunctionalities();
        DeactivateAllButtons();
        // Assign current screen
        currentScreen = setupScreen;
        currentScreen?.SetActive(true);
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
            debuggingText += "\nPosition: " + transform.position.ToString();
            debuggingText += "\n";
            debuggingText += "\nScreen statuses:";
            debuggingText += "\ncurrentScreen: " + currentScreen?.activeSelf.ToString();
            debuggingText += "\nstartScreen: " + startScreen?.activeSelf.ToString();
            debuggingText += "\nplanningScreen: " + planningScreen?.activeSelf.ToString();
            debuggingText += "\ngameOverScreen: " + gameOverScreen?.activeSelf.ToString();
            debuggingText += "\nvictoryScreen: " + victoryScreen?.activeSelf.ToString();
            debuggingText += "\nsettingsScreen: " + settingsScreen?.activeSelf.ToString();
            debuggingText += "\ndebugScreen: " + debugScreen?.activeSelf.ToString();
            debuggingText += "\n";
            debuggingText += "\nFeature statuses:";
            debuggingText += "\nplacePrinter: " + placePrinter?.activeSelf.ToString();
            debuggingText += "\nplaceEnemy: " + placeEnemy?.activeSelf.ToString();
            debuggingText += "\ndebugRay: " + debugRay?.activeSelf.ToString();
            debuggingText += "\ntutorials: " + tutorials?.activeSelf.ToString();
            debugScreenText.text = debuggingText;
        }
    }

    void DeactivateAllScreens()
    {
        // Set inactive
        setupScreen?.SetActive(false);
        startScreen?.SetActive(false);
        planningScreen?.SetActive(false);
        gameOverScreen?.SetActive(false);
        victoryScreen?.SetActive(false);
        settingsScreen?.SetActive(false);
        // debugScreen?.SetActive(false);
    }

    void DeactivateAllFunctionalities()
    {
        // Set inactive
        placePrinter?.SetActive(false);
        placeEnemy?.SetActive(false);
        debugRay?.SetActive(false);
        tutorials?.SetActive(false);
    }

    void DeactivateAllTutorials()
    {
        
        tutorialPressButton?.SetActive(false);
        tutorialPlacePrinter?.SetActive(false);
    }

    void DeactivateAllButtons()
    {
        
        toggleScreenButton?.SetActive(false);
        tutorialButton?.SetActive(false);
        fightButton?.SetActive(false);
    }

    private void updateCurrentScreen(GameObject screen)
    {
        currentScreen = screen;
    }

    public void toggleScreen()
    {
        // Activate or deactivate current screen
        Debug.Log("ArmControllerManager - Toggle screen");
        currentScreen?.SetActive(!currentScreen.activeSelf);
    }

    public void togglePlacePrinter()
    {
        // Activate or deactivate the printer placement (and its preview object)
        Debug.Log("ArmControllerManager - Toggle printer placement");
        placePrinter?.SetActive(!placePrinter.activeSelf);
        placePrinter?.GetComponent<PlaceObject>().TogglePreview();
    }

    public void togglePlaceEnemy()
    {
        // Activate or deactivate the enemy placement (and its preview object)
        Debug.Log("ArmControllerManager - Toggle enemy placement");
        placeEnemy?.SetActive(!placeEnemy.activeSelf);
        placeEnemy?.GetComponent<PlaceObject>().TogglePreview();
    }

    public void toggleDebugRay()
    {
        // Activate or deactivate the debug ray
        Debug.Log("ArmControllerManager - Toggle debug ray");
        debugRay?.SetActive(!debugRay.activeSelf);
        debugRay?.GetComponent<DebugRay>().toggleDebugRay();
    }

    public void toggleTutorial()
    {
        // Activate or deactivate the tutorials
        Debug.Log("ArmControllerManager - Toggle tutorial");
        tutorials?.SetActive(!tutorials.activeSelf);
    }

    private void UpdateScoreTexts()
    {
        int score = GameManager.instance.getScore();
        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = score.ToString();
        }
        if (victoryScoreText != null)
        {
            victoryScoreText.text = score.ToString();
        }
    }

    private void Troubleshooting()
    {
        // Troubleshoot for missing references
        string errorText = "ArmControlManager - ERROR - Missing component reference: ";
        if (startScreen == null) {Debug.LogError(errorText + "startScreen");}
        if (planningScreen == null) {Debug.LogError(errorText + "planningScreen");}
        if (gameOverScreen == null) {Debug.LogError(errorText + "gameOverScreen");}
        if (victoryScreen == null) {Debug.LogError(errorText + "victoryScreen");}
        if (settingsScreen == null) {Debug.LogError(errorText + "settingsScreen");}
        if (debugScreen == null) {Debug.LogError(errorText + "debugScreen");}
        if (placePrinter == null) {Debug.LogError(errorText + "placePrinter");}
        if (placeEnemy == null) {Debug.LogError(errorText + "placeEnemy");}
        if (debugRay == null) {Debug.LogError(errorText + "debugRay");}
        if (debugScreenText == null) {Debug.LogError(errorText + "debugScreenText");}
    }

    public void StartGame()
    {
        Debug.Log("ArmControlManager - StartGame");
        GameManager.instance.UpdateGameState(GameState.GameStart);
    }

    public void StartPlanning()
    {
        Debug.Log("ArmControlManager - StartPlanning");
        GameManager.instance.UpdateGameState(GameState.PlayerPlanningState);
    }

    public void StartWave()
    {
        Debug.Log("ArmControlManager - StartWave");
        GameManager.instance.UpdateGameState(GameState.EnemyWaveState);
    }
}
