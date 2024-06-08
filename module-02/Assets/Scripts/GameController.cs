using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Player player;
    public TMP_InputField textEntry;
    public TMP_Text logText;
    public TMP_Text currentText;
    [TextArea] public string introText;
    public Action[] actions;

    private List<string> commandHistory;
    private int currentCommandHistory;
    void Start()
    {
        logText.text = introText;
        DisplayLocation();
        textEntry.ActivateInputField();
        commandHistory = new List<string>();
        currentCommandHistory = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            currentCommandHistory++;
            if(currentCommandHistory >= commandHistory.Count)
            {
                currentCommandHistory--;
                return;
            }
            textEntry.text = commandHistory[currentCommandHistory];
            textEntry.caretPosition = textEntry.text.Length;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentCommandHistory--;
            if (currentCommandHistory < -1)
            {
                currentCommandHistory++;
                return;
            }
            textEntry.text = currentCommandHistory != -1 ? commandHistory[currentCommandHistory] : string.Empty;
            textEntry.caretPosition = textEntry.text.Length;
        }
    }

    public void DisplayLocation(bool add = false)
    {
        currentText.text = (add ? currentText.text : "") + player.currentLocation.description + '\n' + player.currentLocation.GetConnections() + player.currentLocation.GetItems();
    }

    public void OnTextEntered()
    {
        if (textEntry.text == string.Empty) {
            textEntry.ActivateInputField();
            return;
        }
        LogCurrentText();
        ProcessInput(textEntry.text);
        textEntry.text = string.Empty;
        textEntry.ActivateInputField();
    }

    private void LogCurrentText() {
        logText.text += "\n<color=#f0f>>" + textEntry.text + "</color>\n";
    }

    public void ProcessInput(string input) {
        currentCommandHistory = -1;
        if(commandHistory.Count == 0 || input != commandHistory[0])
            commandHistory.Insert(0, input);
        input = input.ToLower();
        char[] delimiter = { ' ' };
        string[] seperated = input.Split(delimiter);

        foreach (Action a in actions) {
            if(a.keyword.ToLower() == seperated[0]) {
                    a.Respond(this, seperated.Length > 1 ? seperated[1] : string.Empty);
                return;
            }
        }

        logText.text += "Command not found. Type help for more information\n";
    }
}
