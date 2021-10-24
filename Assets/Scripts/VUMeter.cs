using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VUMeter : MonoBehaviour
{
    public AudioSource audioSource;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;
    public float currentMaxLoudness;
    private float currentUpdateTime = 0f;
    private float clipLoudness;
    private float[] clipSampleData;

    [SerializeField] private VUIndicator[] vuIndicators;

    void Awake()
    {
        clipSampleData = new float[sampleDataLength];
    }

    void Update()
    {
        if (!audioSource)
        {
            return;
        }

        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.GetOutputData(clipSampleData, 0); //audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for

            
            var loudness = Map(clipLoudness, 0, currentMaxLoudness, 0, 1, true, true, true, true);

            foreach (var vuIndicator in vuIndicators)
            {
                vuIndicator.Level = loudness;
            }
        }
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
