using System.Xml.Serialization;

[XmlInclude(typeof(DuctTileData))]
[XmlInclude(typeof(SoundTileData))]
[XmlInclude(typeof(StartPositionTileData))]
public abstract class TileData
{
    public TileData()
    {
    }

    public TileData(int column, int row, float rotation, int level = 0) 
    {
        Column = column;
        Row = row;
        Rotation = rotation;
        Level = level;
    }

    public int Column { get; set; }
    public int Row { get; set; }
    public float Rotation { get; set; }
    public int Level { get; set; }
}