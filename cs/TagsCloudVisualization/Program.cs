using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;


namespace TagsCloudVisualization
{
    class Program
    {
        private const int WIDTH_MIN = 5;
        private const int WIDTH_MAX = 50;

        private const int HEIGHT_MIN = 5;
        private const int HEIGHT_MAX = 50;

        public static void Main()
        {
            var layouter = new CircularCloudLayouter(new Point(500, 500));
            var rectangles = new List<Rectangle>();

            var rand = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < 250; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(new Size(rand.Next(WIDTH_MIN, WIDTH_MAX), rand.Next(HEIGHT_MIN, HEIGHT_MAX))));
            }
            Visualizer.GetImageFromRectangles(rectangles, new Size(1000, 1000)).Save("out.png");
        }
    }
}


