using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineNoise : MonoBehaviour
{
    [SerializeField] PlayerMovementController playerMovementController;
    [SerializeField] AudioSource engineAudioSource;
    
    [Range(0,1)]
    [SerializeField] float maxVolume;

    [Range(0, 1)]
    [SerializeField] float minPitch;

    [Range(0, 1)]
    [SerializeField] float maxPitch;

    private void Update()
    {
        if (playerMovementController.enabled)
        {
            // Use the lateral speed (ignore the y axis) to set the engine noise to prevent constant engine sounds while unity adjusts the grounding of the player in small increments.
            engineAudioSource.volume = Mathf.Clamp(playerMovementController.LateralSpeed, 0, maxVolume);
            engineAudioSource.pitch = Mathf.Clamp(playerMovementController.LateralSpeed, minPitch, maxPitch);
        }
        else
        {
            engineAudioSource.volume = 0;
            engineAudioSource.pitch = minPitch;
        }
    }
}
