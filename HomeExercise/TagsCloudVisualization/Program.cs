using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using FluentAssertions;

namespace TagsCloudVisualization
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var maxSize = new Size(20, 20);
            var minSize = new Size(20, 20);
            var rectangles = RectanglesGenerator.GenerateRectangles(100, maxSize, minSize);
            var image = CloudVisualizer.Visualize(rectangles);
            image.Save("layout1.png");
        }
    }
}