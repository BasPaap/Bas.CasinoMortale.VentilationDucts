using System.Collections.ObjectModel;

public class SoundTile : Tile
{
    public Collection<string> FileNames { get; set; } = new Collection<string>();
}