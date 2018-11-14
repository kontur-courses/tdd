using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class TagsGenerator
    {
        public static IEnumerable<Size> GenerateRectanglesSizes(int count)
        {
            var random = new Random();

            return Enumerable.Range(0, count).Select(x => new Size(random.Next(30, 50), random.Next(30, 50)));
        }
    }
}
