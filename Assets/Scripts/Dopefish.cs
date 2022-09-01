using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Dopefish : MonoBehaviour
{
    [SerializeField] private BootScreen bootScreen;

    private readonly List<KeyCode> enteredKeyCodes = new List<KeyCode>();

    private void Update()
    {
        TestKey(KeyCode.UpArrow);
        TestKey(KeyCode.DownArrow);
        TestKey(KeyCode.LeftArrow);
        TestKey(KeyCode.RightArrow);
        TestKey(KeyCode.B);
        TestKey(KeyCode.A);
        TestKey(KeyCode.C);

        if (IsKonamiCodeEntered())
        {
            bootScreen.ShowDopefish();
        }
    }

    private void TestKey(KeyCode key)
    {
        if (Input.GetKeyUp(key))
        {
            enteredKeyCodes.Add(key);
        }
    }

    private bool IsKonamiCodeEntered()
    {
        const string konamiCode = "↑↑↓↓←→←→BAC";
        var enteredCode = new StringBuilder();

        foreach (var keyCode in enteredKeyCodes)
        {
            switch (keyCode)
            {
                case KeyCode.UpArrow:
                    enteredCode.Append("↑");
                    break;
                case KeyCode.DownArrow:
                    enteredCode.Append("↓");
                    break;
                case KeyCode.LeftArrow:
                    enteredCode.Append("←");
                    break;
                case KeyCode.RightArrow:
                    enteredCode.Append("→");
                    break;
                case KeyCode.B:
                    enteredCode.Append("B");
                    break;
                case KeyCode.A:
                    enteredCode.Append("A");
                    break;
                case KeyCode.C:
                    enteredCode.Append("C");
                    break;
                default:
                    break;
            }
        }

        if (enteredCode.ToString() == konamiCode)
        {
            return true;
        }
        else if (!konamiCode.StartsWith(enteredCode.ToString()))
        {
            enteredKeyCodes.Clear();
        }

        return false;
    }
}
