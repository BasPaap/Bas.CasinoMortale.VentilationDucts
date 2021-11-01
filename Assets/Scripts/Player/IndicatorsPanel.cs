using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorsPanel : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject vuIndicator;
    [SerializeField] private GameObject signalIndicator;
    [SerializeField] private GameObject batteryIndicator;

    private void Awake()
    {
        background.SetActive(Settings.Instance.IsIndicatorBackgroundVisible);
        vuIndicator.SetActive(Settings.Instance.IsVUIndicatorVisible);
        signalIndicator.SetActive(Settings.Instance.IsSignalIndicatorVisible);
        batteryIndicator.SetActive(Settings.Instance.IsBatteryIndicatorVisible);
    }
}
