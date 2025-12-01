namespace TagsCloudLib.Models;

public record LayoutBounds(int MinX, int MinY, int MaxX, int MaxY)
{
    public int Width => MaxX - MinX;
    public int Height => MaxY - MinY;
}