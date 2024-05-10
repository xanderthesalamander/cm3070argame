using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Excecute this before the other scripts, in order to capture the errors (default is 0)
[DefaultExecutionOrder(-100)]
public class DebugScreen : MonoBehaviour
{
    public TextMeshProUGUI TMPTextLogs;
    public int keepLogLines;
    public TextMeshProUGUI TMPTextWarnings;
    public int keepWarningLines;
    public TextMeshProUGUI TMPTextErrors;
    public int keepErrorLines;

    void Start()
    {
        // Remove text
        TMPTextLogs.text = "";
        TMPTextWarnings.text = "";
        TMPTextErrors.text = "";
        // Subscribe to the log event
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    // This is triggered whenever there is an error
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Errors and exceptions
        if (type == LogType.Error || type == LogType.Exception)
        {
            // Extract script and line number information from the stack trace
            string[] lines = stackTrace.Split('\n');
            if (lines.Length > 0)
            {
                // The first line usually contains the error message; the second line contains script and line number
                string errorInfo = lines[1].Trim();
                string errorLocation = lines[2].Trim();
                // Append the error information
                string errorsText = TMPTextErrors.text + errorInfo + ":\n" + logString + "\n(" + errorLocation + ")";
                // Keep only the last N lines
                TMPTextErrors.text = GetLastNLines(errorsText, keepErrorLines);
            }
        }
        // Warnings
        if (type == LogType.Warning)
        {
            // Extract script and line number information from the stack trace
            string[] lines = stackTrace.Split('\n');
            if (lines.Length > 0)
            {
                // Append the warning information
                string warningText = TMPTextWarnings.text + logString;
                // Keep only the last N lines
                TMPTextWarnings.text = GetLastNLines(warningText, keepWarningLines);
            }
        }
        // Other logs
        else
        {
            // Extract script and line number information from the stack trace
            string[] lines = stackTrace.Split('\n');
            if (lines.Length > 0)
            {
                // Append the log information
                string logsText = TMPTextLogs.text + "\n" + logString;
                // Keep only the last N lines
                TMPTextLogs.text = GetLastNLines(logsText, keepLogLines);
            }
        }
    }

    private string GetLastNLines(string text, int n)
    {
        // Split text in lines and keep last n lines
        string[] lines = text.Split('\n');
        int startLineIndex = Mathf.Max(0, lines.Length - n);
        string[] lastNLines = new string[Mathf.Min(n, lines.Length - startLineIndex)];
        Array.Copy(lines, startLineIndex, lastNLines, 0, lastNLines.Length);
        return string.Join("\n", lastNLines);
    }
}