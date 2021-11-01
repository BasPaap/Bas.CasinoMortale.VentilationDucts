//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;

//public class Settings
//{
//    public static float StartPositionMicrophoneMaxLoudness { get; set; }
//    public static float StartPositionMicrophonePlaybackDelay { get; set; }
//    public static bool IsRetroEffectEnabled { get; set; }
//    public static bool IsMicrophoneActiveOnStartPosition { get; set; }
//    public static bool IsVUIndicatorVisible { get; set; }
//    public static bool IsSignalIndicatorVisible { get; set; }
//    public static bool IsBatteryIndicatorVisible { get; set; }
//    public static bool IsScannerPulseVisible { get; set; }
//    public static bool IsFogOfWarEnabled { get; set; }

//    public static void Load()
//    {
//        const string fileName = "settings.xml";
//        var fullPath = Path.Combine(Application.persistentDataPath, fileName);

//        if (!File.Exists(fullPath))
//        {
//            StartPositionMicrophoneMaxLoudness = 
//            Save();
//        }

//        Debug.Log("Loading Settings data.");
//        var serializer = new XmlSerializer(typeof(MapData));
//        using var streamReader = new StreamReader(FullPath);
//        mapData = serializer.Deserialize(streamReader) as MapData;
//    }

//    public static void Save()
//    {

//    }
//}
