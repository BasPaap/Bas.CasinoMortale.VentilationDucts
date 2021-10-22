using System.Collections.ObjectModel;

public class SoundTileData : TileData
{
    public Collection<string> FileNames { get; set; } = new Collection<string>();

    public SoundTileData()
    {

    }

    public SoundTileData(int column, int row, float rotation, string fileName)
        : base(column, row, rotation)
    {
        FileNames.Add(fileName);
    }
}