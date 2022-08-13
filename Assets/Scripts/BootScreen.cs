using System;
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
    [SerializeField] private Map map;

    private PlayerMovementController playerMovementController;
    private readonly List<VUIndicator> vuIndicators = new List<VUIndicator>();
    private readonly List<AudioSource> audioSources = new List<AudioSource>();
    private readonly List<MicrophoneAudioPlayer> microphoneAudioPlayers = new List<MicrophoneAudioPlayer>();
    private bool isReadyForConnection = false;

    private void Awake()
    {
        Debug.Log("Starting boot sequence.");
        map.Loaded += Map_Loaded;
        playerMovementController = player.GetComponent<PlayerMovementController>();
        playerMovementController.enabled = false;

        foreach (var audioSource in player.GetComponents<AudioSource>())
        {
            audioSources.Add(audioSource);
            audioSource.enabled = false;
        }

        vuIndicators.AddRange(player.GetComponentsInChildren<VUIndicator>());
        foreach (var vuIndicator in vuIndicators)
        {
            vuIndicator.enabled = false;
        }

        //signalAnimator.enabled = false;
        signalAnimator.gameObject.SetActive(false);
        //batteryAnimator.enabled = false;
        batteryAnimator.gameObject.SetActive(false);
    }

    private void Map_Loaded(object sender, System.EventArgs e)
    {
        foreach (var audioSource in map.GetComponentsInChildren<AudioSource>())
        {
            audioSources.Add(audioSource);
            audioSource.enabled = false;
        }

        foreach (var microphoneAudioPlayer in map.GetComponentsInChildren<MicrophoneAudioPlayer>())
        {
            microphoneAudioPlayers.Add(microphoneAudioPlayer);
            microphoneAudioPlayer.enabled = false;
        }
    }

    public void OnConnect(InputAction.CallbackContext context) // Called by PlayerInput component on Player
    {
        if (isReadyForConnection && context.started)
        {
            isReadyForConnection = false;
            terminal.Append(" OK");
            terminal.AppendLine("> Connecting...", () =>
            {
                terminal.Append(" OK");
                this.Wait(1, () => signalAnimator.gameObject.SetActive(true));
                this.Wait(1, () => terminal.AppendLine("> Testing battery status..."));
                this.Wait(3, () => terminal.Append(" OK", () => batteryAnimator.gameObject.SetActive(true)));
                this.Wait(5, () => terminal.AppendLine("> Receiving audio..."));
                this.Wait(7, () => terminal.Append(" OK", () =>
                {
                    foreach (var vuIndicator in vuIndicators)
                    {
                        vuIndicator.enabled = true;
                    }

                    EnableAllAudio();
                }));
                this.Wait(9, () => terminal.AppendLine("> Switching to environmental imaging mode...", () => this.Wait(3, EndBootSequence)));
            });
        }
    }

    private void EndBootSequence()
    {
        Debug.Log("Ending boot sequence.");
        terminal.Clear();
        playerMovementController.enabled = true;
        gameObject.SetActive(false);
    }

    private void EnableAllAudio()
    {
        foreach (var audioSource in audioSources)
        {
            if (audioSource != null)
            {
                audioSource.enabled = true;
            }
        }

        foreach (var microphoneAudioPlayer in microphoneAudioPlayers)
        {
            if (microphoneAudioPlayer != null)
            {
                microphoneAudioPlayer.enabled = true;
            }
        }
    }

    private void Start()
    {
        terminal.Clear();
        
        terminal.Append("> Welcome to ROVER.");
        terminal.AppendLine("> Please place the remote operated ROVER unit in the nearest accessible air vent and press the CONNECT button.");
        terminal.AppendLine("> <WARNING> To prevent signal interference, accidental exposure to lethal doses of radiation, dismemberment or partial loss of fingers by accidental or unexpected activation, do not press the CONNECT button until the remote operated ROVER unit is in the air vent.");
        terminal.AppendLine();
        terminal.AppendLine("> Q branch is committed to providing a safe working environment for our field agents.");
        terminal.AppendLine("> Most recent Health and Safety Executive guidelines compliance certfification date: ");
        
        terminal.Append(" <ERROR>");
        terminal.AppendLine();
        terminal.AppendLine("> Waiting for connection...", () => isReadyForConnection = true);
    }
}
