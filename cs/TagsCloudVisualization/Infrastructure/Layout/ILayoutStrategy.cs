using System;
using System.Drawing;

namespace TagsCloudVisualization.Infrastructure.Layout
{
    public interface ILayoutStrategy
    {
        Point GetPlace(Func<Point, bool> isValidPlace);
    }
}