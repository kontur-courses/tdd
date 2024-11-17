using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class DeltaSHaped : IPointGenerator
    {
        public IEnumerable<Point> GeneratePoints(Point start)
        {
            var zoom = 5;
            yield return start;
            while (true)
            {
                foreach (var pair in Heart())
                {
                    var x = start.X + (int)(zoom * pair.Item1);
                    var y = start.Y + (int)(zoom * pair.Item2);
                    var next = new Point(x, y);
                    yield return next;
                }
                zoom += 2;
            }
        }

        public IEnumerable<(double, double)> Heart()
        {
            for (var t = 0.0; t < 2 * Math.PI; t += Math.PI / 180)
            {
                var x = 2 * Math.Cos(t) + Math.Cos(2*t);
                var y = 2 * Math.Sin(t) - Math.Sin(2 * t);
                yield return (x, y);
            }
        }
    }
}
