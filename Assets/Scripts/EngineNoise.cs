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

    private Vector3 lastPosition;

    private void Awake()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {   
        engineAudioSource.volume = Mathf.Clamp(playerMovementController.Speed, 0, maxVolume);
        engineAudioSource.pitch = Mathf.Clamp(playerMovementController.Speed, minPitch, maxPitch);
        
        lastPosition = transform.position;
    }
}
