using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    [SerializeField, Range(0,100)] private float speed = 1.0f;
    [SerializeField, Range(0,100)] private float cursorBlinkSpeed = 10.0f;
    private Text text;
    private StringBuilder textBuffer = new StringBuilder();
    private string enteredText;
    private float lastEnteredCharacterTime;
    private const char pauseCharacter = (char)3;
    private bool isCursorVisible = false;
    private float lastCursorVisibilityChangeTime;
    private readonly Dictionary<string, Action> callbacks = new Dictionary<string, Action>();

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void Clear()
    {
        enteredText = string.Empty;
        lastEnteredCharacterTime = Time.time;
        lastCursorVisibilityChangeTime = Time.time;
    }

    public void AppendLine() => AppendLine(string.Empty);
    public void AppendLine(string textToAppend, Action callback = null) => Append(System.Environment.NewLine + textToAppend, callback);
    public void Append(string textToAppend, Action callback = null)
    {
        if (callback != null)
        {
            callbacks.Add(textToAppend, callback);
        }

        textBuffer.Append(textToAppend + pauseCharacter);
    }

    private void Update()
    {
        UpdateText();
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (isCursorVisible)
        {
            text.text += "_";
        }

        if (Time.time - lastCursorVisibilityChangeTime > 1 / cursorBlinkSpeed)
        {
            isCursorVisible = !isCursorVisible;
            lastCursorVisibilityChangeTime = Time.time;
        }
    }

    private void UpdateText()
    {
        if (textBuffer.Length > 0 && Time.time - lastEnteredCharacterTime > 1 / speed)
        {
            var newCharacter = textBuffer[0];
            textBuffer.Remove(0, 1);

            if (newCharacter == pauseCharacter)
            {
                lastEnteredCharacterTime = Time.time + 1f;
            }
            else
            {
                enteredText += newCharacter;
                lastEnteredCharacterTime = Time.time;
            }

            HandleCallbacks();
        }

        text.text = enteredText;
    }

    private void HandleCallbacks()
    {
        if (textBuffer.Length == 0)
        {
            string callbackText = null;
            foreach (var key in callbacks.Keys)
            {
                if (enteredText.EndsWith(key))
                {
                    callbackText = key;
                    callbacks[key]();
                }
                break;
            }

            if (!string.IsNullOrWhiteSpace(callbackText))
            {
                callbacks.Remove(callbackText);
            }
        }
    }
}
