using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public partial class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        private readonly List<Point> spiralPoints;
        private double angle = 0;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
            spiralPoints = new List<Point>();
        }

        public void PutNextRectangle(Size rectangleSize)
        {
            throw new NotImplementedException();
        }

        private static bool IsRectanglesIntersect(Rectangle firstRect, Rectangle secondRect)
        {
            return !(firstRect.X > secondRect.X + secondRect.Width
                     || firstRect.X + firstRect.Width < secondRect.X
                     || firstRect.Y > secondRect.Y + secondRect.Height
                     || firstRect.Y + firstRect.Height < secondRect.Y);
        }

        public void GenerateRandomCloud(int amountRectangles)
        {
            var rnd = new Random();
            for (var i = 0; i < amountRectangles; i++)
                PutNextRectangle(new Size(rnd.Next(25, 80), rnd.Next(25, 80)));
        }
    }
}