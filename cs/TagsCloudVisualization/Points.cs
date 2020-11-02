using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public abstract class Points
    {
        protected Points(Point center)
        {
        }

        public abstract IEnumerable<Point> GetPoints();
    }
}