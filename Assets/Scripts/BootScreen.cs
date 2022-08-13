using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BootScreen : MonoBehaviour
{
    [SerializeField] private GameObject signalAnimator;
    [SerializeField] private GameObject batteryAnimator;
    [SerializeField] private GameObject player;

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

        signalAnimator.SetActive(false);
        batteryAnimator.SetActive(false);
    }
}
