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
    public bool IsScannerPulseVisible { get; set; }
    public bool IsFogOfWarEnabled { get; set; }

    private static void Load()
    {
        Debug.Log("Loading settings data.");
        var fullPath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(fullPath))
        {
            Debug.Log("Settings file not found, creating default settings file.");

            Instance.StartPositionMicrophoneMaxLoudness = 0.02f;
            Instance.StartPositionMicrophonePlaybackDelay = 0.5f;
            Instance.IsRetroEffectEnabled = true;
            Instance.IsMicrophoneActiveOnStartPosition = true;
            Instance.IsVUIndicatorVisible = true;
            Instance.IsSignalIndicatorVisible = true;
            Instance.IsBatteryIndicatorVisible = true;
            Instance.IsScannerPulseVisible = true;
            Instance.IsFogOfWarEnabled = true;

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
