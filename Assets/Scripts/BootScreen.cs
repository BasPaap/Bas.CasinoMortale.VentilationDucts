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

    private void Awake()
    {
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

        signalAnimator.enabled = false;
        batteryAnimator.enabled = false;
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

    public void Connect_started(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Connecting!");
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
        terminal.AppendLine("> Waiting for connection...");
    }
}
