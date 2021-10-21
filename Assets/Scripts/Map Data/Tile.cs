using System.Xml.Serialization;

[XmlInclude(typeof(DuctTile))]
[XmlInclude(typeof(SoundTile))]
public abstract class Tile
{
    public Tile()
    {
    }

    public Tile(int column, int row, float rotation) 
    {
        Column = column;
        Row = row;
        Rotation = rotation;
    }

    public int Column { get; set; }
    public int Row { get; set; }
    public float Rotation { get; set; }
}