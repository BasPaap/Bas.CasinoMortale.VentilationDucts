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
        callbacks.Clear();
        textBuffer.Clear();
        enteredText = string.Empty;
        lastEnteredCharacterTime = Time.time;
        lastCursorVisibilityChangeTime = Time.time;
    }

    public void AppendLine() => AppendLine(string.Empty);
    public void AppendLine(string textToAppend, Action callback = null) => Append(System.Environment.NewLine + textToAppend, callback);
    public void Append(string textToAppend, Action callback = null)
    {
        if (isActiveAndEnabled)
        {
            if (callback != null)
            {
                callbacks.Add(textToAppend, callback);
            }

            textBuffer.Append(textToAppend + pauseCharacter);
        }
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

    public void ShowDopefish()
    {
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW..;;;;;.WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW.;;;;;;;WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW;;;;;;;;;WWWWWWWWWWWWWWWWWWWWWW..WWWWWWWWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWW.;;;;;;;;.WWWWWWWWWWWWWWWWWWWWWWW..;;.WWWWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWW...;;;.......;;..WWWWWWWWWWWWWWWWWWWWWWWW.;;..WWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWW...;;;;;;;;;;;;;;;;;;;;;;..WWWWWWWWWWWWWWWWWWWW.;;;.WWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWW.;;;;;;;;;;;;;;;;;;;;;;;;;;;;;..WWWWWWWWWWWWWWWWW.;;;;..WWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWW;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;.WWWWWWWWWWWWWW.;;;;;..WWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWW.;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;.WWWWWWWWWWWW.;;;;;;..WWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWW....;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;.WWWWWWWWWW.;;;;;;;;..WWW" + Environment.NewLine;
        enteredText += "WWWWWWWW.WWW..WWWWV..;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+..WWWWWWW.;;;;;;;;;..WWW" + Environment.NewLine;
        enteredText += "WWWWWW.WWWWWWWWWWWWWWWW;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;..WWWW.;;;;;;;;;;;..WWW" + Environment.NewLine;
        enteredText += "WWWWW.VW......WWWWWWWWW.;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;....;;;;;;;;;;;..WWWW" + Environment.NewLine;
        enteredText += "WWWWW.WW......WWWWWWWWWW.;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;..WWWWW" + Environment.NewLine;
        enteredText += "WWWWW..WWW.....WWWWWWWWV.;;;;;;...;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;..WWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWW.;.W.WWWWWWWWWWWWW.;;;;;;;;..;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;.WWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWW...;.VW..WWWWWWWW.;..;;;;;;..;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;..WWWWWW" + Environment.NewLine;
        enteredText += "WWWWWW......;;;......;;;;;;..;;;..;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;..WWWWW" + Environment.NewLine;
        enteredText += "WWWWWW.WW.VV...............;;;;;..;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;..WWWW" + Environment.NewLine;
        enteredText += "WWWWWW.WW.WWWWV.;;;;;;.;....;;;...;;;;;;;;;;;;;;;;;;;;;;;;....;;;;;;;;;;;..WWWW" + Environment.NewLine;
        enteredText += "WWWWWW.WW.WWWWV....;;;;;;;..;;;..;;;;;;;;;;;;;;;;;;;;;;;..WWWW..;;;;;;;;;..WWWW" + Environment.NewLine;
        enteredText += "WWWWWW.WW.WWWWV.;;;;;;.;;;;.;;;;;;;;;;;;;;;;;;;;;..;;....WWWWWW..;;;;;;;;..WWWW" + Environment.NewLine;
        enteredText += "WWWWWW.WW.WWWWV.....;;;;;;;;;;;;;;;;;;;;;;;;;.;;;;....WWWWWWWWWWW.;;;;;;..WWWWW" + Environment.NewLine;
        enteredText += "WWWWWW.WW.WWWWV.;;;;;;;;;;;;;;;;;;;;;;;;;;;;;.;;.....WWWWWWWWWWWW.;;;;;;..WWWWW" + Environment.NewLine;
        enteredText += "WWWWWW....WWWWV..;;;;;;;.;;;;;;;;;;;;;;;;;;;;......WWWWWWWWWWWWWW.;;;;;..WWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWW...WV.WW......+;;;;;;;;;;;;;;;;;;;;;...WWWWWWWWWWWWWWW.;;;;;;.WWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWW..;;;;;;;;;;;;;;;;;...WWWWWWWWWWWWWWWWWW.;;;..WWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWWWWW..;;;;;;;;;...WWWWWWWWWWWWWWWWWWWWWW.;;.WWWWWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWWWWWW -= DOPEFISH LIVES! =- WWWWWWWWWWWWWWWWWWWWWWWWWWWW" + Environment.NewLine;
        enteredText += "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW" + Environment.NewLine;
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
