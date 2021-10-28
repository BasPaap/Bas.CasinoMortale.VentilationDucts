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
        Debug.Log($"Populating map with {mapData.Tiles.Count} tiles.");
        foreach (var tile in mapData.Tiles)
        {
            var prefab = TileFactory.Instance.GetTilePrefab(tile);

            if (prefab != null)
            {
                Debug.Log($"Instantiating {prefab.name} at {tile.Column}, {tile.Row} at {tile.Rotation} degrees rotation.");
                var instantiatedTile = Instantiate(prefab, prefab.transform.position + GetPosition(tile.Column, tile.Row), Quaternion.AngleAxis(tile.Rotation, Vector3.up) * prefab.transform.localRotation, transform);

                var fogOfWar = instantiatedTile.GetComponent<FogOfWar>();
                if (fogOfWar != null)
                {
                    fogOfWar.PlayerTransform = playerTransform;
                }

                if (tile is SoundTileData soundTile)
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
        Debug.Log($"Clearing {transform.childCount} map children.");
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

        Debug.Log("Loading map data.");
        var serializer = new XmlSerializer(typeof(MapData));
        using var streamReader = new StreamReader(FullPath);
        mapData = serializer.Deserialize(streamReader) as MapData;
    }

    internal void ResetMap()
    {
        Debug.Log($"Resetting the current map.");
        CreateBackup();
        File.Delete(FullPath);
        Load();
    }

    /// <summary>
    /// Saves the map to a file.
    /// </summary>    
    private void Save()
    {
        Debug.Log("Saving map data.");
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
        Debug.Log($"Creating a default map.");

        // Create an empty 10x10 map
        var defaultMapData = new MapData()
        {
            Height = 10,
            Width = 10
        };
                
        defaultMapData.Tiles.Add(new DuctTileData(0, 3, -270, DuctType.Corner));
        defaultMapData.Tiles.Add(new DuctTileData(0, 4, 0, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(0, 5, 180, DuctType.Grill));
        defaultMapData.Tiles.Add(new SoundTileData(0, 5, 0, "casino.mp3"));
        defaultMapData.Tiles.Add(new DuctTileData(1, 3, -90, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(2, 3, -90, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(3, 1, 0, DuctType.Grill));
        defaultMapData.Tiles.Add(new SoundTileData(3, 1, 0, "do-you-expect-me-to-talk.mp3"));
        defaultMapData.Tiles.Add(new DuctTileData(3, 2, -270, DuctType.ThreeWayCrossing));
        defaultMapData.Tiles.Add(new DuctTileData(3, 3, -90, DuctType.ThreeWayCrossing));
        defaultMapData.Tiles.Add(new DuctTileData(3, 4, 0, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(3, 5, 0, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(3, 6, 180, DuctType.Corner));
        defaultMapData.Tiles.Add(new DuctTileData(4, 2, -90, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(4, 6, -90, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(4, 8, -270, DuctType.Grill));        
        defaultMapData.Tiles.Add(new SoundTileData(4, 8, 0, "gunnar-gunnarson.mp3"));
        defaultMapData.Tiles.Add(new DuctTileData(5, 0, 0, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(5, 1, 0, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(5, 2, -90, DuctType.Corner));
        defaultMapData.Tiles.Add(new DuctTileData(5, 6, 90, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(5, 8, -90, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(6, 6, 0, DuctType.Corner));
        defaultMapData.Tiles.Add(new DuctTileData(6, 7, 0, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(6, 8, 180, DuctType.ThreeWayCrossing));
        defaultMapData.Tiles.Add(new DuctTileData(7, 8, -90, DuctType.Straight));
        defaultMapData.Tiles.Add(new DuctTileData(8, 8, 90, DuctType.Straight));
        defaultMapData.Tiles.Add(new SoundTileData(9, 8, 0, "trololololo.mp3"));
        defaultMapData.Tiles.Add(new DuctTileData(9, 8, -90, DuctType.Grill));
        defaultMapData.Tiles.Add(new StartPositionTileData(5, 0, 0));

        return defaultMapData;
    }

    internal void ClearCell(int column, int row)
    {
        Debug.Log($"Removing all tiles from cell {column}, {row}.");
        RemoveTiles(t => t.Column == column && t.Row == row);
        Save();
    }

    private void RemoveTiles(Func<TileData, bool> predicate)
    {
        var tilesToRemove = mapData.Tiles.Where(predicate).ToList();
        foreach (var tileToRemove in tilesToRemove)
        {
            mapData.Tiles.Remove(tileToRemove);
        }
    }

    private void RemoveTilesOfType<T>(int? column = null, int? row = null)
    {
        if ((column.HasValue && !row.HasValue) ||
            (row.HasValue && !column.HasValue))
        {
            Debug.LogError($"The parameters column and row should either both be null or both have a value. column has value: {column.HasValue}. row has value: {row.HasValue}");
            return;
        }

        if (column.HasValue && row.HasValue)
        {
            Debug.Log($"Removing all tiles of type {nameof(T)} from {column.Value}, {row.Value}.");
            RemoveTiles(t => t.Column == column && t.Row == row && t.GetType() == typeof(T));
        }
        else
        {
            Debug.Log($"Removing all tiles of type {nameof(T)} from the entire map.");
            RemoveTiles(t => t.GetType() == typeof(T));
        }
    }

    internal void AddColumn(ColumnSide side)
    {
        Debug.Log($"Adding column to: {side}");

        mapData.Width++;

        if (side == ColumnSide.Left)
        {
            foreach (var tile in mapData.Tiles)
            {
                tile.Column++;
            }
        }
    }

    internal void DeleteColumn(ColumnSide side)
    {
        Debug.Log($"Deleting column from: {side}");

        if (mapData.Width <= 1)
        {
            Debug.LogWarning("Cannot delete column because the map is already at its minimum width.");
            return;
        }

        mapData.Width--;

        if (side == ColumnSide.Left)
        {
            RemoveTiles(t => t.Column == 0);
        }
        else
        {
            RemoveTiles(t => t.Column == mapData.Width);
        }
    }

    internal void AddRow(RowSide side)
    {
        Debug.Log($"Adding row to: {side}");

        mapData.Height++;

        if (side == RowSide.Bottom)
        {
            foreach (var tile in mapData.Tiles)
            {
                tile.Row++;
            }
        }
    }


    internal void DeleteRow(RowSide side)
    {
        Debug.Log($"Deleting row from: {side}");

        if (mapData.Height <= 1)
        {
            Debug.LogWarning("Cannot delete row because the map is already at its minimum height.");
            return;
        }
        
        mapData.Height--;

        if (side == RowSide.Bottom)
        {
            RemoveTiles(t => t.Row == 0);
        }
        else
        {
            RemoveTiles(t => t.Row == mapData.Height);
        }
    }


    internal void Load()
    {
        LoadMapData();
        PopulateMap();
        PlacePlayerAtStartPosition();
    }

    private void PlacePlayerAtStartPosition()
    {
        var startPositionTileData = mapData.Tiles.FirstOrDefault(t => t.GetType() == typeof(StartPositionTileData));

        if (startPositionTileData == null)
        {
            playerTransform.position = GetPosition(0, 0);
        }
        else
        {
            playerTransform.position = GetPosition(startPositionTileData.Column, startPositionTileData.Row);
        }
    }

    internal void AddTile(TileData newTileData)
    {
        Debug.Log($"Adding new tile to map at {newTileData.Column}, {newTileData.Row}, rotation {newTileData.Rotation} degrees.");
        if (newTileData is DuctTileData newDuctTileData)
        {
            RemoveTilesOfType<DuctTileData>(newTileData.Column, newTileData.Row);
            mapData.Tiles.Add(newDuctTileData);
        }
        else if (newTileData is SoundTileData newSoundTileData)
        {
            RemoveTilesOfType<SoundTileData>(newTileData.Column, newTileData.Row);
            mapData.Tiles.Add(newSoundTileData);
        }
        else if (newTileData is StartPositionTileData newStartPositionTileData)
        {
            RemoveTilesOfType<StartPositionTileData>(); // Remove all start position tiles from the map. There can be only one.
            mapData.Tiles.Add(newStartPositionTileData);
        }
        Save();
    }

    /// <summary>
    /// Creates a backup of the current map file.
    /// </summary>
    internal void CreateBackup()
    {
        Debug.Log("Creating backup of map file.");
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
