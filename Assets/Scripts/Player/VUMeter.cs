using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VUMeter : MonoBehaviour
{
    private const int sampleDataLength = 1024; // 1024 samples is about 80 ms on a 44khz stereo clip.
    private const float updateStep = 0.1f;
    private const float startPositionMaxLoudness = 0.02f;

    private float currentUpdateTime = 0f;
    private float[] clipSampleData = new float[sampleDataLength];
    private AudioSource nearestSoundTileAudioSource;
    private float distanceToNearestSound = float.MaxValue;
    private float nearestSoundMaxLoudness;

    [SerializeField] private VUIndicator[] vuIndicators;
    
    void Update()
    {
        if (nearestSoundTileAudioSource == null)
        {
            return;
        }

        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            var clipLoudness = CalculateClipLoudness();
            var normalizedLoudness = Map(clipLoudness, 0, nearestSoundMaxLoudness, 0, 1, true, true, true, true);

            foreach (var vuIndicator in vuIndicators)
            {
                vuIndicator.Level = normalizedLoudness;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var sound = other.gameObject.GetComponent<SoundTile>();
        var audioSource = other.gameObject.GetComponent<AudioSource>();

        if (audioSource != null)
        {
            var distanceToSound = Vector3.Distance(transform.position, audioSource.transform.position);
                        
            if (audioSource == nearestSoundTileAudioSource)
            {
                distanceToNearestSound = distanceToSound;
                nearestSoundMaxLoudness = sound != null ? sound.CurrentMaxLoudness : startPositionMaxLoudness;
            }
            else if(distanceToSound < distanceToNearestSound)
            {     
                nearestSoundTileAudioSource = audioSource;
                distanceToNearestSound = distanceToSound;
                nearestSoundMaxLoudness = sound != null ? sound.CurrentMaxLoudness : startPositionMaxLoudness;
            }
        }        
    }


    private float CalculateClipLoudness()
    {
        if (nearestSoundTileAudioSource == null)
        {
            return 0;
        }

        currentUpdateTime = 0f;
        
        nearestSoundTileAudioSource.GetOutputData(clipSampleData, 0);

        float clipLoudness = 0f;
        foreach (var sample in clipSampleData)
        {
            clipLoudness += Mathf.Abs(sample);
        }

        clipLoudness /= sampleDataLength;

        return clipLoudness;
    }

    private static float Map(float val, float in1, float in2, float out1, float out2,
        bool in1Clamped, bool in2Clamped, bool out1Clamped, bool out2Clamped)
    {
        if (in1Clamped == true && val < in1) val = in1;
        if (in2Clamped == true && val > in2) val = in2;

        float result = out1 + (val - in1) * (out2 - out1) / (in2 - in1);

        if (out1Clamped == true && result < out1) result = out1;
        if (out2Clamped == true && result > out2) result = out2;

        return result;
    }
}
