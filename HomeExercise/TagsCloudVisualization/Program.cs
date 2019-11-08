using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GetRectangles(out var rectangles);
            Console.WriteLine(rectangles
                .SelectMany((r, i) => rectangles
                    .Skip(i + 1)
                    .Select(r.IntersectsWith))
                .Any(x => x));
            CloudVisualizator.Visualize("img1.bmp");
        }

        private static void GetRectangles(out Rectangle[] rectangles)
        {
            const int size = 50;
            const int spacing = 5;
            var rectangleSize = new Size(size, size);
            var locations1 = new[]
            {
                new Point(-size/2, size/2),
                new Point(size/2+spacing, size/2),
                new Point(-(size/2+size+spacing), size/2),
                new Point(-size/2, -(size/2+spacing)),
                new Point(-size/2, size/2+size+spacing),
            };
            var locations2 = new[]
            {
                new Point(),
                new Point(size+spacing, size+spacing),
            };
            rectangles = locations1.Select(l => new Rectangle(l, rectangleSize)).ToArray();
        }
    }
}