public class DuctTileData : TileData
{
    public DuctTileData()
    {
    }

    public DuctTileData(DuctType type, int column, int row, float rotation)
        : base(column, row, rotation)
    {
        Type = type;
    }

    public DuctType Type { get; set; }
}