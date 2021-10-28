using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MapData
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Collection<TileData> Tiles { get; set; } = new Collection<TileData>();

    internal void RemoveTiles(Func<TileData, bool> predicate)
    {
        var tilesToRemove = Tiles.Where(predicate).ToList();
        foreach (var tileToRemove in tilesToRemove)
        {
            Tiles.Remove(tileToRemove);
        }
    }

    internal void RemoveTilesOfType<T>(int? column = null, int? row = null)
    {
        if ((column.HasValue && !row.HasValue) ||
            (row.HasValue && !column.HasValue))
        {
            Debug.LogError($"The parameters {nameof(column)} and {nameof(row)} should either both be null or both have a value. {nameof(column)} has value: {column.HasValue}. {nameof(row)} has value: {row.HasValue}");
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

        Width++;

        if (side == ColumnSide.Left)
        {
            foreach (var tile in Tiles)
            {
                tile.Column++;
            }
        }
    }

    internal void RemoveColumn(ColumnSide side)
    {
        Debug.Log($"Removing column from: {side}");

        if (Width <= 1)
        {
            Debug.LogWarning("Cannot remove column because the map is already at its minimum width.");
            return;
        }

        Width--;

        if (side == ColumnSide.Left)
        {
            RemoveTiles(t => t.Column == 0);

            foreach (var tile in Tiles)
            {
                tile.Column--;
            }
        }
        else
        {
            RemoveTiles(t => t.Column == Width);
        }
    }

    internal void AddRow(RowSide side)
    {
        Debug.Log($"Adding row to: {side}");

        Height++;

        if (side == RowSide.Bottom)
        {
            foreach (var tile in Tiles)
            {
                tile.Row++;
            }
        }
    }

    internal void RemoveRow(RowSide side)
    {
        Debug.Log($"Removing row from: {side}");

        if (Height <= 1)
        {
            Debug.LogWarning("Cannot remove row because the map is already at its minimum height.");
            return;
        }

        Height--;

        if (side == RowSide.Bottom)
        {
            RemoveTiles(t => t.Row == 0);

            foreach (var tile in Tiles)
            {
                tile.Row--;
            }
        }
        else
        {
            RemoveTiles(t => t.Row == Height);
        }
    }
}