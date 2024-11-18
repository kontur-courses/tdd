using System.Drawing;

namespace TagsCloudVisualizationTests.Extensions;

public static class RectangleExtensions
{
    public static double Area(this Rectangle rectangle)
    {
        return rectangle.Width * rectangle.Height;
    }
    
}