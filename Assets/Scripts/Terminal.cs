using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void Clear()
    {
        text.text = string.Empty;
    }

    public void AppendLine() => AppendLine(string.Empty);
    public void AppendLine(string textToAppend) => Append(System.Environment.NewLine + textToAppend);
    public void Append(string textToAppend)
    {
        text.text += textToAppend;
    }
}
