using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private const int sampleRate = 44100;

    [SerializeField] private float playbackDelay = 0.5f;

    private void Awake()
    {
        enabled = Settings.Instance.IsMicrophoneActiveOnStartPosition;
        playbackDelay = Settings.Instance.StartPositionMicrophonePlaybackDelay;
    }


    private void OnEnable()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.Log("No microphone devices found.");
            return;
        }

        Debug.Log("Starting recording on default microphone.");
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 10, sampleRate);
        audioSource.loop = true;
    }

    private void OnDisable()
    {
        if (Microphone.IsRecording(null))
        {
            Debug.Log("Stopping recording on default microphone.");
            Microphone.End(null);
        }
    }

    private void Update()
    {
        if (audioSource != null &&
            audioSource.isActiveAndEnabled && 
            !audioSource.isPlaying && 
            Microphone.IsRecording(null) && 
            Microphone.GetPosition(null) > sampleRate * playbackDelay)
        {
            Debug.Log("Starting start position playback");
            audioSource.Play();            
        }        
    }

    private void OnApplicationQuit()
    {
        if (Microphone.IsRecording(null))
        {
            Debug.Log("Stopping recording on default microphone.");
            Microphone.End(null);
        }
    }
}
