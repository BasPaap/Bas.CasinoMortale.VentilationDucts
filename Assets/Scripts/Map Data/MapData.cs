using System.Collections.ObjectModel;

public class MapData
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Collection<TileData> Tiles { get; set; } = new Collection<TileData>();
}