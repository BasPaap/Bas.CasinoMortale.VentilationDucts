using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private const string fileName = "map.xml";
    private MapData mapData;

    private string FullPath => Path.Combine(Application.persistentDataPath, fileName);
    public Vector2 Size => mapData != null ? new Vector2(mapData.Width, mapData.Height) : Vector2.zero;

    private void Start()
    {
        Load();
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
        PopulateTiles();
    }
        
    private void PopulateTiles()
    {
        foreach (var tile in mapData.Tiles)
        {
            var prefab = TileFactory.Instance.GetTilePrefab(tile);

            if (prefab != null)
            {
                var instantiatedTile = Instantiate(prefab, prefab.transform.position + GetPosition(tile.Column, tile.Row), Quaternion.AngleAxis(tile.Rotation, Vector3.up) * prefab.transform.localRotation, transform);

                var duct = instantiatedTile.GetComponent<Duct>();
                if (duct != null)
                {
                    duct.PlayerTransform = playerTransform;
                }

                if (tile is SoundTile soundTile)
                {
                    var sound = instantiatedTile.GetComponent<Sound>();
                    sound.SetAudioFileNames(soundTile.FileNames);
                }
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
    private void LoadMapData()
    {
        if (!File.Exists(FullPath))
        {
            mapData = CreateDefaultMap();
            Save();
        }

        var serializer = new XmlSerializer(typeof(MapData));
        using var streamReader = new StreamReader(FullPath);
        mapData = serializer.Deserialize(streamReader) as MapData;
    }

    /// <summary>
    /// Saves the map to a file.
    /// </summary>    
    private void Save()
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

    internal void RemoveTiles(int column, int row)
    {
        var tilesToRemove = mapData.Tiles.Where(t => t.Column == column && t.Row == row).ToList();
        foreach (var tileToRemove in tilesToRemove)
        {
            mapData.Tiles.Remove(tileToRemove);
        }

        Save();
    }

    internal void Load()
    {
        LoadMapData();
        PopulateMap();
    }

    internal void AddTile(Tile newTile)
    {
        if (newTile is DuctTile newDuctTile)
        {
            var tilesToRemove = mapData.Tiles.Where(t => t.Column == newTile.Column && t.Row == newTile.Row && t.GetType() == typeof(DuctTile)).ToList();
            foreach (var tileToRemove in tilesToRemove)
            {
                mapData.Tiles.Remove(tileToRemove);
            }

            mapData.Tiles.Add(newDuctTile);
        }
        else if (newTile is SoundTile soundTile)
        {
            var tilesToRemove = mapData.Tiles.Where(t => t.Column == newTile.Column && t.Row == newTile.Row && t.GetType() == typeof(SoundTile)).ToList();
            foreach (var tileToRemove in tilesToRemove)
            {
                mapData.Tiles.Remove(tileToRemove);
            }

            mapData.Tiles.Add(soundTile);
        }

        Save();
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
    /// <param name="column">The X-coordinate of the cell.</param>
    /// <param name="row">The Y-coordinate of the cell.</param>
    /// <returns>The cell's position in world space.</returns>
    internal Vector3 GetPosition(int column, int row)
    {
        var halfWidth = (Size.x - 1) / 2.0f;
        var halfHeight = (Size.y - 1) / 2.0f;

        var cellSize = Vector3.one;
        var xPos = 0 - halfWidth + (cellSize.x * column);
        var zPos = 0 - halfHeight + (cellSize.z * row);
        
        return new Vector3(xPos, 0, zPos);
    }
}
