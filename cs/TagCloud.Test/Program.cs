using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Test
{
    class Program
    {
        static void Main()
        {
            TestPath();
            TestOnDescendingRectangles();
            TestOnRandomRectangles();
            TestOnSimillarRectangles();
        }

        private static void TestOnDescendingRectangles()
        {
            var spiral = new Spiral();
            var cloud = new CircularCloudLayouter(spiral);
            var rectangles = new List<Rectangle>();
            var visualizer = new CloudVisualizer
            {
                Settings = DrawSettings.RectanglesWithNumeration
            };

            for (var i = 30; i >= 0; i--)
                rectangles.Add(cloud.PutNextRectangle(new Size(100 + i * 5, 50 + i * 5)));

            var picture = visualizer.CreatePictureWithRectangles(rectangles.ToArray());
            picture.Save("descendingRectanglesTest.png");
        }

        private static void TestOnRandomRectangles()
        {
            var spiral = new Spiral();
            var cloud = new CircularCloudLayouter(spiral);
            var rectangles = new List<Rectangle>();
            var visualizer = new CloudVisualizer
            {
                Settings = DrawSettings.RectanglesWithNumeration
            };
            var rnd = new Random();

            for (var i = 22; i >= 0; i--)
                rectangles.Add(cloud.PutNextRectangle(new Size(100 + i * rnd.Next(1, 10), 50 + i * rnd.Next(1, 10))));

            var picture = visualizer.CreatePictureWithRectangles(rectangles.ToArray());
            picture.Save("randomCloudTest.png");
        }

        private static void TestOnSimillarRectangles()
        {
            var spiral = new Spiral();
            var cloud = new CircularCloudLayouter(spiral);
            var rectangles = new List<Rectangle>();
            var visualizer = new CloudVisualizer
            {
                Settings = DrawSettings.RectanglesWithNumeration
            };

            for (var i = 22; i >= 0; i--)
                rectangles.Add(cloud.PutNextRectangle(new Size(100, 100)));

            var picture = visualizer.CreatePictureWithRectangles(rectangles.ToArray());
            picture.Save("similarCloudTest.png");
        }

        private static void TestPath()
        {
            Spiral spiral = Spiral.Create().WithStepLength(100);
            var points = new Point[100];

            for (var i = 0; i < 100; i++)
                points[i] = spiral.GetNextPoint();

            var size = points.GetBounds();
            var picture = new Bitmap(size.Width, size.Height);
            using (var graphics = Graphics.FromImage(picture))
            {
                graphics.TranslateTransform(-size.X, -size.Y);
                graphics.DrawLines(Pens.Black, points);
            }
            picture.Save("pathTest.png");
        }
    }
}