using System.Drawing;

namespace TagsCloudVisualization;

public interface ICloudLayouter
{
    List<Rectangle> PlacedRectangles { get; }
    Rectangle PutNextRectangle(Size rectangleSize);
    
    Rectangle GetCloudBorders()
    {
        if (PlacedRectangles is null || PlacedRectangles.Count == 0)
            throw new InvalidOperationException("The list of placed rectangles cannot be null or empty");
        
        var left = PlacedRectangles.Min(r => r.Left);
        var right = PlacedRectangles.Max(r => r.Right);
        var top = PlacedRectangles.Min(r => r.Top);
        var bottom = PlacedRectangles.Max(r => r.Bottom);

        var width = right - left;
        var height = bottom - top;
        return new Rectangle(left, top, width, height);
    }
}