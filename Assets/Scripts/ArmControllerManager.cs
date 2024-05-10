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
    private GameObject currentScreen;
    // Functionalities
    [SerializeField] private GameObject placePrinter;
    [SerializeField] private GameObject debugRay;
    // Debugging output
    [SerializeField] private TextMeshProUGUI debugScreenText;
    
    void Awake()
    {
        Troubleshooting();
        // Subscribe to the game manager
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        // Assign current screen
        currentScreen = startScreen;
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
        DeactivateAllFunctionalities();
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
            debuggingText += "\ndebugRay: " + debugRay?.activeSelf.ToString();
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

    void DeactivateAllFunctionalities()
    {
        // Set inactive
        placePrinter?.SetActive(false);
        debugRay?.SetActive(false);
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
        placePrinter?.GetComponent<PlaceObject>().togglePreview();
    }

    public void toggleDebugRay()
    {
        // Activate or deactivate the debug ray
        Debug.Log("ArmControllerManager - Toggle debug ray");
        debugRay?.SetActive(!debugRay.activeSelf);
        debugRay?.GetComponent<DebugRay>().toggleRayHitPreview();
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
        if (debugRay == null) {Debug.LogError(errorText + "debugRay");}
        if (debugScreenText == null) {Debug.LogError(errorText + "debugScreenText");}
    }
}
