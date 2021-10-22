using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class FileOption : MonoBehaviour
{
    [SerializeField] Text label;
    private Toggle toggle;
    private string fileName;

    public bool IsSelected => toggle.isOn;
    
    public string FileName
    {
        get => fileName;
        set
        {
            fileName = value;
            label.text = fileName;
        }
    }

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
}