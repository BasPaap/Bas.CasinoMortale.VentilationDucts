public class DuctTileData : TileData
{
    public DuctTileData()
    {
    }

    public DuctTileData(int column, int row, float rotation, DuctType type)
        : base(column, row, rotation)
    {
        Type = type;
    }

    public DuctType Type { get; set; }
}