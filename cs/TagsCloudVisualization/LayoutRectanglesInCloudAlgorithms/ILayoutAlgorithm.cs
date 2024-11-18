using System.Drawing;

namespace TagsCloudVisualization.LayoutRectanglesInCloudAlgorithms;

public interface ILayoutAlgorithm
{
    Point CalculateNextPoint();
}