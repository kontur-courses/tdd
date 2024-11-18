using System.Drawing;

namespace TagsCloudVisualization.TagCloud;

public interface IPositionCalculator
{
    public IEnumerable<Rectangle> CalculateNextPosition(Size nextRectangleSize);
    public bool IsRectanglePositionValid(List<Rectangle> rectangles, Rectangle currentRectangle);
}