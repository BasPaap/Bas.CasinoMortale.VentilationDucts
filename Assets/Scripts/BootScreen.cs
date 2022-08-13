using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BootScreen : MonoBehaviour
{
    [SerializeField] private Animator signalAnimator;
    [SerializeField] private Animator batteryAnimator;
    [SerializeField] private GameObject player;
    [SerializeField] private Terminal terminal;

    private PlayerInput playerInput;
    private AudioListener playerAudioListener;
    private readonly List<VUIndicator> vuIndicators = new List<VUIndicator>();

    private void Awake()
    {
        playerInput = player.GetComponent<PlayerInput>();
        playerInput.enabled = false;

        playerAudioListener = player.GetComponent<AudioListener>();
        playerAudioListener.enabled = false;

        vuIndicators.AddRange(player.GetComponentsInChildren<VUIndicator>());
        foreach (var vuIndicator in vuIndicators)
        {
            vuIndicator.enabled = false;
        }

        signalAnimator.enabled = false;
        batteryAnimator.enabled = false;
    }

    private void Start()
    {
        terminal.Clear();
        this.Wait(1, () => terminal.Append("> Checking battery status... "));
        this.Wait(2, () =>
        {
            batteryAnimator.enabled = true;
            terminal.Append("OK.");
        });
    }
}
