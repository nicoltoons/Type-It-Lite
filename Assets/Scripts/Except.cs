using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Except : MonoBehaviour
{
    public TextMeshProUGUI outputText;
    void Awake()
    {
        Application.logMessageReceived += HandleException;
    }

    void HandleException(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            //handle here
            outputText.text = "Error: Incorrect Format!";
        }
    }
}

