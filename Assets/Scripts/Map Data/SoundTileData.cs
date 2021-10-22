using System.Collections.ObjectModel;

public class SoundTileData : TileData
{
    public Collection<string> FileNames { get; set; } = new Collection<string>();
}