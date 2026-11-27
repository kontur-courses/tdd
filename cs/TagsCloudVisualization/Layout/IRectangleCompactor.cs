using System.Drawing;

namespace TagsCloudVisualization.Layout;

public interface IRectangleCompactor
{
    Rectangle MoveTowardsCenter(Rectangle rectangle, IReadOnlyList<Rectangle> obstacles, Point center);
}