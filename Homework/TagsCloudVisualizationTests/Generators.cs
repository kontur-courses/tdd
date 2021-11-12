using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualizationTests
{
    public static class Generators
    {
        public static IEnumerable<Point> CenterGenerator()
        {
            yield return new Point(0, 0);
            yield return new Point(-10, -10);
            yield return new Point(10, 10);
            yield return new Point(-10, 10);
            yield return new Point(10, -10);
            yield return new Point(0, 10);
            yield return new Point(0, -10);
            yield return new Point(10, 0);
            yield return new Point(-10, 0);
        }
        
        public static IEnumerable<SizeF> RectanglesSizeGenerator()
        {
            var random = new Random();

            for (var i = 0; i < 100; ++i)
            {
                // +0.01f is added to avoid NextDouble returned zero
                yield return new SizeF(
                    (float) random.NextDouble() * random.Next(1, 100) + 0.01f,
                    (float) random.NextDouble() * random.Next(1, 100) + 0.01f);
            }
        }
    }
}