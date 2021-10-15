using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Map : MonoBehaviour
{
    private const string fileName = "map.xml";
    private string FullPath => Path.Combine(Application.persistentDataPath, fileName);

    private MapData mapData;

    private void Start()
    {
        Load();
    }

    internal void Load()
    {
        if (!File.Exists(FullPath))
        {
            MapData defaultMap = CreateDefaultMap();
            Save(defaultMap);
        }

        var serializer = new XmlSerializer(typeof(MapData));
        using (var streamReader = new StreamReader(FullPath))
        {
            mapData = serializer.Deserialize(streamReader) as MapData;            
        }
    }

    private void Save(MapData mapData)
    {
        var serializer = new XmlSerializer(typeof(MapData));
        using (var streamWriter = new StreamWriter(FullPath))
        {
            serializer.Serialize(streamWriter, mapData);
            streamWriter.Close();
        }
    }

    private MapData CreateDefaultMap()
    {
        // Create an empty 10x10 map
        var defaultMapData = new MapData()
        {
            Height = 10,
            Width = 10
        };

        return defaultMapData;
    }

    internal void CreateBackup()
    {
        if (File.Exists(FullPath))
        {
            File.Copy(FullPath, Path.Combine(Application.persistentDataPath, $"{DateTime.Now:yyyyMMddhhmmss} {fileName}"));
        }
    }
}
