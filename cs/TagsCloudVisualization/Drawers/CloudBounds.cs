namespace TagsCloudVisualization.Drawers;

public readonly record struct CloudBounds(int MinX, int MinY, int MaxX, int MaxY)
{
    public int Width => MaxX - MinX;
    public int Height => MaxY - MinY;
}