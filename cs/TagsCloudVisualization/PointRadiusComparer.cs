using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class PointRadiusComparer : IComparer<Point>
    {
        private readonly Point centre;

        public PointRadiusComparer(Point centre) => this.centre = centre;

        public int Compare(Point firstPoint, Point secondPoint) =>
            GetRadius(firstPoint).CompareTo(GetRadius(secondPoint));


        private double GetRadius(Point point)
        {
            var pointDifference = point - (Size) centre;
            return pointDifference.X * pointDifference.X + pointDifference.Y * pointDifference.Y;
        }
    }
}