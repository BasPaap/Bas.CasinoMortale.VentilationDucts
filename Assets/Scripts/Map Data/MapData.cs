using System.Collections.ObjectModel;

public class MapData
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Collection<Tile> Tiles { get; set; } = new Collection<Tile>();
}