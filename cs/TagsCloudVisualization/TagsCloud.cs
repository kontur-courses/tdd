using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagsCloud
    {
        public readonly Point Center;
        public readonly List<Rectangle> AddedRectangles = new List<Rectangle>();

        public TagsCloud(Point center)
        {
            var x = center.X;
            var y = center.Y;
            if (x < 0 || y < 0)
                throw new ArgumentException("Center coordinates should not be negative");
            Center = center;
        }

        public TagsCloud(Point center, List<Rectangle> rectangles)
        {
            var x = center.X;
            var y = center.Y;
            if (x < 0 || y < 0)
                throw new ArgumentException("Center coordinates should not be negative");
            AddedRectangles = rectangles;
        }

        public void AddRectangle(Rectangle rectangle)
        {
            AddedRectangles.Add(rectangle);
        }
    }
}