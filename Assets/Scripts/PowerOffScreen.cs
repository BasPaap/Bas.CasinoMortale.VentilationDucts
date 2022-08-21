using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PowerOffScreen : MonoBehaviour
{
    [SerializeField] private BootScreen bootScreen;
    [SerializeField] private Terminal terminal;

    private bool isPoweredOn;
    private Image backgroundImage;

    private void Awake()
    {
        backgroundImage = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        if (!Settings.Instance.IsPowerButtonRequired)
        {
            PowerOn();
        }
    }

    public void OnPower(InputAction.CallbackContext context) // Called by PlayerInput component on Player
    {   
        if (context.started && !isPoweredOn)
        {
            Debug.Log("Power button pressed.");
            terminal.gameObject.SetActive(true);
            PowerOn();
        }
        else if (context.canceled && isPoweredOn)
        {
            Debug.Log("Power button released.");
            PowerOff();
        }
    }

    private void PowerOff()
    {
        backgroundImage.DOFade(1, 0.25f).OnComplete(() =>
        {
            bootScreen.Shutdown();
            terminal.gameObject.SetActive(false);
            isPoweredOn = false;
        });        
    }

    private void PowerOn()
    {
        backgroundImage.DOFade(0, 1).OnComplete(() =>
        {
            this.Wait(1, () =>
            {
                bootScreen.StartBootSequence();
                isPoweredOn = true;
            });
        });
        
    }
}
