using System.Drawing;

namespace TagsCloudVisualization;

public interface ICircularCloudBuilder
{
    public Rectangle GetNextPosition(Size rectangleSize, List<Rectangle> placedRectangles);
}