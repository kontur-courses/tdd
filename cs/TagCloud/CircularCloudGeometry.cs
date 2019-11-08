using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudGeometry
    {
        private readonly Point center;
        private readonly HashSet<Rectangle> rectangles;

        public CircularCloudGeometry(Point center, HashSet<Rectangle> rectangles)
        {
            this.center = center;
            this.rectangles = rectangles;
        }

        public double GetCloudFullnessPercent()
        {
            var radius = GetFurthestDistance();
            var cloudCircleSquare = Math.PI * radius * radius;
            var allRectanglesSquare = GetAllRectanglesSquare();
            return allRectanglesSquare / cloudCircleSquare;
        }

        public double GetFurthestDistance()
        {
            return rectangles.Select(rect => GetDistanceToRectangle(center, rect)).Concat(new[] {0.0}).Max();
        }


        public double GetDistanceToRectangle(Point point, Rectangle rect)
        {
            return new List<double>
            {
                GetDistanceBetweenPoints(point, new Point(rect.Left, rect.Top)),
                GetDistanceBetweenPoints(point, new Point(rect.Left, rect.Bottom)),
                GetDistanceBetweenPoints(point, new Point(rect.Right, rect.Top)),
                GetDistanceBetweenPoints(point, new Point(rect.Right, rect.Bottom))
            }.Max();
        }

        public double GetDistanceBetweenPoints(Point from, Point to)
        {
            return Math.Sqrt((to.X - from.X) * (to.X - from.X) + (to.Y - from.Y) * (to.Y - from.Y));
        }

        private double GetAllRectanglesSquare()
        {
            return rectangles.Sum(rect => rect.Width * rect.Height);
        }
    }
}