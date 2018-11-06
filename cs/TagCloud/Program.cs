using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
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
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            var visualizer = new CloudVisualizer();
            visualizer.Settings = DrawSettings.RectanglesWithNumeration;

            for (var i = 30; i >= 0; i--)
                rectangles.Add(cloud.PutNextRectangle(new Size(100 + i * 5, 50 + i * 5)));

            var picture = visualizer.CreatePictureWithRectangles(rectangles.ToArray());
            picture.Save("descendingRectanglesTest.png");
        }

        private static void TestOnRandomRectangles()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            var visualizer = new CloudVisualizer();
            visualizer.Settings = DrawSettings.RectanglesWithNumeration;
            var rnd = new Random();

            for (var i = 22; i >= 0; i--)
                rectangles.Add(cloud.PutNextRectangle(new Size(100 + i * rnd.Next(1, 10), 50 + i * rnd.Next(1, 10))));

            var picture = visualizer.CreatePictureWithRectangles(rectangles.ToArray());
            picture.Save("randomCloudTest.png");
        }

        private static void TestOnSimillarRectangles()
        {
            var cloud = new CircularCloudLayouter(new Point(0, 0));
            var rectangles = new List<Rectangle>();
            var visualizer = new CloudVisualizer();
            visualizer.Settings = DrawSettings.RectanglesWithNumeration;

            for (var i = 22; i >= 0; i--)
                rectangles.Add(cloud.PutNextRectangle(new Size(100, 100)));

            var picture = visualizer.CreatePictureWithRectangles(rectangles.ToArray());
            picture.Save("similarCloudTest.png");
        }

        private static void TestPath()
        {
            var picture = new Bitmap(250, 250);
            var path = new SpiralPointsSequence(10);
            var points = new Point[100];

            for (var i = 0; i < 100; i++)
                points[i] = path.GetNextPoint();

            var size = points.GetBounds();
            picture = new Bitmap(size.Width, size.Height);
            using (var graphics = Graphics.FromImage(picture))
            {
                graphics.TranslateTransform(-size.X, -size.Y);
                graphics.DrawLines(Pens.Black, points);
            }
            picture.Save("pathTest.png");
        }
    }
}