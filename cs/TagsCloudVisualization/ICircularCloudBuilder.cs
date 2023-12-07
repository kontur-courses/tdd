using System.Drawing;

namespace TagsCloudVisualization;

public interface ICircularCloudBuilder
{
    public Rectangle GetNextPosition(Point center, Size rectangleSize, List<Rectangle> placedRectangles);
}