using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VUMeter : MonoBehaviour
{
    private const int sampleDataLength = 1024; // 1024 samples is about 80 ms on a 44khz stereo clip.
    private const float updateStep = 0.1f;

    private float currentUpdateTime = 0f;
    private float[] clipSampleData = new float[sampleDataLength];
    
    [SerializeField] private VUIndicator[] vuIndicators;

    public AudioSource AudioSource { get; set; }
    public float CurrentMaxLoudness { get; set; }

    void Update()
    {
        if (!AudioSource)
        {
            return;
        }

        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            var clipLoudness = CalculateClipLoudness();
            var normalizedLoudness = Map(clipLoudness, 0, CurrentMaxLoudness, 0, 1, true, true, true, true);

            foreach (var vuIndicator in vuIndicators)
            {
                vuIndicator.Level = normalizedLoudness;
            }
        }
    }

    private float CalculateClipLoudness()
    {
        currentUpdateTime = 0f;
        AudioSource.GetOutputData(clipSampleData, 0);

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
