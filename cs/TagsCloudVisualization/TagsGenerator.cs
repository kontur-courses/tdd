using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagsGenerator
    {
        public static IEnumerable<Size> GenerateRectanglesSizes(int count)
        {
            var random = new Random();
            var rectangles = new List<Size>();

            for (var i = 0; i < count; i++)
                rectangles.Add(new Size(random.Next(30, 50), random.Next(30, 50)));

            return rectangles;
        }
    }
}
