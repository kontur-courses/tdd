using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Utils
    {
        public static List<Rectangle> GenerateRandomRectangles(CircularCloudLayouter layouter, int count, int minSize,
            int maxSize, Random random)
        {
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < count; i++)
            {
                var size = new Size(random.Next(minSize, maxSize), random.Next(minSize, maxSize));
                var rectangle = layouter.PutNextRectangle(size);
                rectangles.Add(rectangle);
            }

            return rectangles;
        }

        public static double GetDistanceBetweenRectangleAndPoint(Rectangle rectangle, Point point)
        {
            var rectangleCentre = new Point(rectangle.Location.X + rectangle.Width / 2,
                rectangle.Location.Y + rectangle.Height / 2);

            return Math.Sqrt(Math.Pow(rectangleCentre.X - point.X, 2) + Math.Pow(rectangleCentre.Y - point.Y, 2));
        }
    }
}