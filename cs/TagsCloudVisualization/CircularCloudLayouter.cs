using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
            rectangles.Add(new Rectangle(GetNextRectanglePosition(rectangleSize), rectangleSize));
        }

        public void GenerateRandomCloud(int amountRectangles)
        {
            var rnd = new Random();
            for (var i = 0; i < amountRectangles; i++)
                PutNextRectangle(new Size(rnd.Next(25, 80), rnd.Next(25, 80)));
        }
    }
}