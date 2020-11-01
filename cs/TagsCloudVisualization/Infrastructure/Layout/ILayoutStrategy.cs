using System.Drawing;

namespace TagsCloudVisualization.Infrastructure.Layout
{
    public interface ILayoutStrategy
    {
        Point GetPlace(Size rectangleSize);
    }
}