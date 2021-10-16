using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Map : MonoBehaviour
{
    private const string fileName = "map.xml";
    private MapData mapData;

    private string FullPath => Path.Combine(Application.persistentDataPath, fileName);

    [SerializeField] private GameObject straightDuctPrefab;
    [SerializeField] private GameObject cornerDuctPrefab;
    [SerializeField] private GameObject threeWayDuctPrefab;
    [SerializeField] private GameObject fourWayDuctPrefab;

    public Vector2 Size => mapData != null ? new Vector2(mapData.Width, mapData.Height) : Vector2.zero;

    private void Start()
    {
        Load();
        PopulateMap();
    }

    /// <summary>
    /// Populates the map objects with tiles as specified in the loaded map data.
    /// </summary>
    private void PopulateMap()
    {
        if (mapData == null)
        {
            Debug.Log("No map data found. Load the map first.");
            return;
        }

        ClearChildren();
        PopulateDuctTiles();
        //PopulateGrateTiles();
        //PopulateSoundTiles();
    }

    private void PopulateSoundTiles()
    {
        throw new NotImplementedException();
    }

    private void PopulateGrateTiles()
    {
        throw new NotImplementedException();
    }

    private void PopulateDuctTiles()
    {
        foreach (DuctTile tile in mapData.Tiles)
        {
            var prefab = tile.Type switch
            {
                DuctType.Straight => straightDuctPrefab,
                DuctType.Corner => cornerDuctPrefab,
                DuctType.ThreeWayCrossing => threeWayDuctPrefab,
                DuctType.FourWayCrossing => fourWayDuctPrefab,
                DuctType.None => null,
                _ => null
            };

            if (prefab != null)
            {
                Instantiate(prefab, prefab.transform.position + GetPosition(tile.Column, tile.Row), Quaternion.AngleAxis(tile.Rotation, Vector3.up) * prefab.transform.localRotation, transform);
            }
        }
    }

    /// <summary>
    ///  Clears all children from the map object.
    /// </summary>
    private void ClearChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
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
        using var streamReader = new StreamReader(FullPath);
        mapData = serializer.Deserialize(streamReader) as MapData;
    }

    /// <summary>
    /// Saves the map to a file.
    /// </summary>
    /// <param name="mapData">The map data to save.</param>
    private void Save(MapData mapData)
    {
        var serializer = new XmlSerializer(typeof(MapData));
        using var streamWriter = new StreamWriter(FullPath);
        serializer.Serialize(streamWriter, mapData);
        streamWriter.Close();
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
        defaultMapData.Tiles.Add(new DuctTile(DuctType.Straight, 5, 0, 0));
        defaultMapData.Tiles.Add(new DuctTile(DuctType.Straight, 5, 1, 0));
        defaultMapData.Tiles.Add(new DuctTile(DuctType.Corner, 5, 2, 270));
        defaultMapData.Tiles.Add(new DuctTile(DuctType.Straight, 6, 2, 90));
        defaultMapData.Tiles.Add(new DuctTile(DuctType.ThreeWayCrossing, 7, 2, 0));
        defaultMapData.Tiles.Add(new DuctTile(DuctType.FourWayCrossing, 7, 1, 0));

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
