using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class HeartShaped : IPointGenerator
    {
        public IEnumerable<Point> GeneratePoints(Point start)
        {
            var zoom = 1;
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
                zoom += 1;
            }
        }

        public IEnumerable<(double, double)> Heart()
        {
            for (var t = 0.0; t < 2 * Math.PI; t += Math.PI / 180)
            {
                var x = 16 * Math.Sin(t) * Math.Sin(t) * Math.Sin(t);
                var y = -13 * Math.Cos(t) + 5 * Math.Cos(2 * t) + 2 * Math.Cos(3 * t) + Math.Cos(4 * t);
                yield return (x, y);
            }
        }
    }
}
