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

    public Vector3 CellSize => Vector3.one;

    private void Start()
    {
        Load();
    }

    internal void Load()
    {
        LoadMapData();
        PopulateMap();
        PlacePlayerAtStartPosition();
    }

    #region Map Editing
    public void AddColumn(ColumnSide side)
    {
        mapData.AddColumn(side);
        Save();
    }

    public void RemoveColumn(ColumnSide side)
    {
        mapData.RemoveColumn(side);
        Save();
    }

    public void AddRow(RowSide side)
    {
        mapData.AddRow(side);
        Save();
    }

    public void RemoveRow(RowSide side)
    {
        mapData.RemoveRow(side);
        Save();
    }

    internal void ClearCell(int column, int row, int level)
    {
        Debug.Log($"Removing all tiles from cell {column}, {row} at level {level}.");
        mapData.RemoveTiles(t => t.Column == column && t.Row == row && t.Level == level);
        Save();
    }

    internal void AddTile(TileData newTileData)
    {
        Debug.Log($"Adding new tile to map at {newTileData.Column}, {newTileData.Row}, rotation {newTileData.Rotation} degrees.");
        if (newTileData is DuctTileData newDuctTileData)
        {
            mapData.RemoveTilesOfType<DuctTileData>(newTileData.Column, newTileData.Row);
            mapData.Tiles.Add(newDuctTileData);
        }
        else if (newTileData is SoundTileData newSoundTileData)
        {
            mapData.RemoveTilesOfType<SoundTileData>(newTileData.Column, newTileData.Row);
            mapData.Tiles.Add(newSoundTileData);
        }
        else if (newTileData is StartPositionTileData newStartPositionTileData)
        {
            mapData.RemoveTilesOfType<StartPositionTileData>(); // Remove all start position tiles from the map. There can be only one.
            mapData.Tiles.Add(newStartPositionTileData);
        }

        Save();
    } 
    #endregion
    
    #region Visuals
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
                Debug.Log($"Instantiating {prefab.name} at {tile.Column}, {tile.Row} at level {tile.Level} at {tile.Rotation} degrees rotation.");
                var instantiatedTile = Instantiate(prefab, prefab.transform.position + GetPosition(tile.Column, tile.Row, tile.Level), Quaternion.AngleAxis(tile.Rotation, Vector3.up) * prefab.transform.localRotation, transform);

                var fogOfWar = instantiatedTile.GetComponent<FogOfWar>();
                if (fogOfWar != null)
                {
                    fogOfWar.PlayerTransform = playerTransform;
                }

                if (tile is SoundTileData soundTile)
                {
                    var sound = instantiatedTile.GetComponent<SoundTile>();
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

    private void PlacePlayerAtStartPosition()
    {
        var startPositionTileData = mapData.Tiles.FirstOrDefault(t => t.GetType() == typeof(StartPositionTileData));

        if (startPositionTileData == null)
        {
            playerTransform.position = GetPosition(0, 0, 0);
        }
        else
        {
            playerTransform.position = GetPosition(startPositionTileData.Column, startPositionTileData.Row, startPositionTileData.Level);
        }
    }

    #endregion

    #region File I/O
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
    /// Creates a backup of the current map file.
    /// </summary>
    internal void CreateBackup()
    {
        Debug.Log("Creating backup of map file.");
        if (File.Exists(FullPath))
        {
            File.Copy(FullPath, Path.Combine(Application.persistentDataPath, $"{DateTime.Now:yyyyMMddhhmmss} {fileName}"));
        }

        // Remove old backups.
        RemoveOldBackups();
    }

    private static void RemoveOldBackups()
    {
        string searchPattern = $"* {fileName}";
        var backupFilePaths = Directory.GetFiles(Application.persistentDataPath, searchPattern).OrderByDescending(f => File.GetLastWriteTime(f));
        var filePathsToExclude = backupFilePaths.Take(10).ToList();
        var filePathFromPreviousDate = backupFilePaths.Where(f => File.GetLastWriteTime(f) < DateTime.Today).FirstOrDefault();
        if (filePathFromPreviousDate != null && !filePathsToExclude.Contains(filePathFromPreviousDate))
        {
            filePathsToExclude.Add(filePathFromPreviousDate);
        }

        foreach (var filePath in backupFilePaths.Except(filePathsToExclude))
        {
            Debug.Log($"Deleting backup file {Path.GetFileName(filePath)}");
            File.Delete(filePath);
        }
    }
    #endregion

    #region Utilities
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

    /// <summary>
    /// Returns the position in world space for a cell in the grid.
    /// </summary>
    /// <param name="column">The X-coordinate of the cell.</param>
    /// <param name="row">The Y-coordinate of the cell.</param>
    /// <returns>The cell's position in world space.</returns>
    internal Vector3 GetPosition(int column, int row, int level)
    {
        var xPos = CellSize.x * column;
        var yPos = CellSize.y * level;
        var zPos = CellSize.z * row;

        return new Vector3(xPos, yPos, zPos);
    } 
    #endregion
}
