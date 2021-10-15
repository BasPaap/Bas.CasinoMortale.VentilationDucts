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

    public Vector2 Size => mapData != null ? new Vector2(mapData.Width, mapData.Height) : Vector2.zero;

    private void Start()
    {
        Load();
    }

    /// <summary>
    /// Loads map data from a file.
    /// </summary>
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

    /// <summary>
    /// Saves the map to a file.
    /// </summary>
    /// <param name="mapData">The map data to save.</param>
    private void Save(MapData mapData)
    {
        var serializer = new XmlSerializer(typeof(MapData));
        using (var streamWriter = new StreamWriter(FullPath))
        {
            serializer.Serialize(streamWriter, mapData);
            streamWriter.Close();
        }
    }

    /// <summary>
    /// Returns a default 10x10 map.
    /// </summary>
    /// <returns>A default 10x10 map.</returns>
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

    /// <summary>
    /// Creates a backup of the current map file.
    /// </summary>
    internal void CreateBackup()
    {
        if (File.Exists(FullPath))
        {
            File.Copy(FullPath, Path.Combine(Application.persistentDataPath, $"{DateTime.Now:yyyyMMddhhmmss} {fileName}"));
        }
    }

    /// <summary>
    /// Returns the position in world space for a cell in the grid.
    /// </summary>
    /// <param name="gridX">The X-coordinate of the cell.</param>
    /// <param name="gridY">The Y-coordinate of the cell.</param>
    /// <returns>The cell's position in world space.</returns>
    internal Vector3 GetPosition(int gridX, int gridY)
    {
        var halfWidth = (Size.x - 1) / 2.0f;
        var halfHeight = (Size.y - 1) / 2.0f;

        var cellSize = Vector3.one;
        var xPos = 0 - halfWidth + (cellSize.x * gridX);
        var zPos = 0 - halfHeight + (cellSize.z * gridY);
        
        return new Vector3(xPos, 0, zPos);
    }
}
