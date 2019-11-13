using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.tools
{
    internal static class RectangleExtensions
    {
        public static IEnumerable<Point> Vertexes(this Rectangle rectangle)
        {
            yield return rectangle.Location;
            yield return rectangle.Location + new Size(rectangle.Width, 0);
            yield return rectangle.Location + rectangle.Size;
            yield return rectangle.Location + new Size(0, rectangle.Height);
        }

        public static double Distance(this Rectangle r1, Rectangle r2)
        {
            var points1 = r1.Vertexes().ToList();
            var points2 = r2.Vertexes().ToList();
            var min = points1[0].Distance(points2[0]);

            foreach (var p1 in points1)
            {
                foreach (var p2 in points2)
                {
                    min = Math.Min(p1.Distance(p2), min);
                }
            }

            return min;
        }
    }
}