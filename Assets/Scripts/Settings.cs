using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Settings
{
    const string fileName = "settings.xml";

    private static Settings instance;
    private Settings()
    {        
    }

    public static Settings Instance 
    { 
        get
        {
            if (instance == null)
            {
                Load();
            }

            return instance;
        }
    }

    public float StartPositionMicrophoneMaxLoudness { get; set; }
    public float StartPositionMicrophonePlaybackDelay { get; set; }
    public bool IsRetroEffectEnabled { get; set; }
    public bool IsMicrophoneActiveOnStartPosition { get; set; }
    public bool IsVUIndicatorVisible { get; set; }
    public bool IsSignalIndicatorVisible { get; set; }
    public bool IsBatteryIndicatorVisible { get; set; }
    public bool IsIndicatorBackgroundVisible { get; set; }
    public bool IsScannerPulseVisible { get; set; }
    public float ScannerSpeed { get; set; }
    public float ScannerInterval { get; set; }
    public bool IsFogOfWarEnabled { get; set; }
    public bool IsPowerButtonRequired { get; set; }

    private static void Load()
    {
        Debug.Log("Loading settings data.");
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(fullPath))
        {
            Debug.Log("Settings file not found, creating default settings file.");

            instance = new Settings();
            instance.StartPositionMicrophoneMaxLoudness = 0.02f;
            instance.StartPositionMicrophonePlaybackDelay = 0.5f;
            instance.IsRetroEffectEnabled = true;
            instance.IsMicrophoneActiveOnStartPosition = true;
            instance.IsVUIndicatorVisible = true;
            instance.IsSignalIndicatorVisible = true;
            instance.IsBatteryIndicatorVisible = true;
            instance.IsIndicatorBackgroundVisible = true;
            instance.IsScannerPulseVisible = true;
            instance.ScannerSpeed = 1.0f;
            instance.ScannerInterval = 1.0f;
            instance.IsFogOfWarEnabled = true;
            instance.IsPowerButtonRequired = false;

            Save();
        }

        var serializer = new XmlSerializer(typeof(Settings));
        using var streamReader = new StreamReader(fullPath);
        instance = serializer.Deserialize(streamReader) as Settings;        
    }

    public static void Save()
    {
        Debug.Log("Saving settings data.");
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);
        var serializer = new XmlSerializer(typeof(Settings));
        using var streamWriter = new StreamWriter(fullPath);
        serializer.Serialize(streamWriter, Instance);
        streamWriter.Close();
    }
}
