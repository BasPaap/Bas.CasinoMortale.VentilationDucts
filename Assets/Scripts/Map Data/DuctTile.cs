public class DuctTile : Tile
{
    public DuctTile()
    {
    }

    public DuctTile(DuctType type, int column, int row, float rotation)
        : base(column, row, rotation)
    {
        Type = type;
    }

    public DuctType Type { get; set; }
}