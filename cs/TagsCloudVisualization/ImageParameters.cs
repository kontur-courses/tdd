namespace TagsCloudVisualization;

public class ImageParameters
{
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }

    public ImageParameters(int offsetX, int offsetY, int height, int width)
    {
        OffsetX = offsetX;
        OffsetY = offsetY;
        Height = height;
        Width = width;
    }
}