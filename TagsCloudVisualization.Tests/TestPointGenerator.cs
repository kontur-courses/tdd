using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Tests
{
    internal class TestPointGenerator : IPointGenerator
    {
        public IEnumerable<Point> GeneratePoints(Point start)
        {
            var points = new[] { new Point(0, 0), new Point(1, 1), new Point(2, 2) };
            foreach (var point in points)
                yield return point;
        }
    }
}
