using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class ArchemedianSpiral : IPointGenerator
    {
        public IEnumerable<Point> GeneratePoints(Point start)
        {
            var zoom = 1;
            var spiralStep = 0.0;
            yield return start;
            while (true)
            {
                spiralStep += Math.PI / 180;
                var x = start.X + (int)(zoom * spiralStep * Math.Cos(spiralStep));
                var y = start.Y + (int)(zoom * spiralStep * Math.Sin(spiralStep));
                var next = new Point(x, y);
                yield return next;
            }
        }
    }
}
