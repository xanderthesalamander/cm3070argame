using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Excecute this before the other scripts, in order to capture the errors (default is 0)
[DefaultExecutionOrder(-100)]
public class TMPDebugScreen : MonoBehaviour
{
    public TextMeshProUGUI TMPTextStatus;
    public TextMeshProUGUI TMPTextErrors;

    void Start()
    {
        // Subscribe to the log event
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void Update()
    {
        string statusText = "Debugging: ";
        TMPTextStatus.text = statusText;
    }

    // This is triggered whenever there is an error
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            // Extract script and line number information from the stack trace
            string[] lines = stackTrace.Split('\n');
            if (lines.Length > 0)
            {
                // The first line usually contains the error message; the second line contains script and line number
                string errorInfo = lines[1].Trim();
                // Append the error information
                string errorsText = TMPTextErrors.text + errorInfo + ": " + logString;
                // Keep only the last 500 characters
                if (errorsText.Length > 500)
                {
                    errorsText = errorInfo.Substring(errorInfo.Length - 500);
                }
                TMPTextErrors.text = errorsText;
            }
        }
    }
}