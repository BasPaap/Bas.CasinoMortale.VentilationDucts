using System.Xml.Serialization;

[XmlInclude(typeof(DuctTileData))]
[XmlInclude(typeof(SoundTileData))]
public abstract class TileData
{
    public TileData()
    {
    }

    public TileData(int column, int row, float rotation) 
    {
        Column = column;
        Row = row;
        Rotation = rotation;
    }

    public int Column { get; set; }
    public int Row { get; set; }
    public float Rotation { get; set; }
}