using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Sequences;

namespace TagsCloudVisualization.CloudLayouts
{
    public class CircularCloudLayout : ICloudLayout
    {
        private readonly Spiral spiral;
        private readonly Point center;
        private readonly HashSet<Rectangle> rectangles;
        
        public CircularCloudLayout(Point center)
        {
            this.center = center;
            this.spiral = new Spiral();
            this.rectangles = new HashSet<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if(rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException($"{nameof(PutNextRectangle)} : {rectangleSize}");
            foreach (var point in spiral.GetPoints())
            {
                var offsetPoint = new Point(center.X + point.X, center.Y + point.Y);
                var rect = new Rectangle(offsetPoint, rectangleSize);
                if (rectangles.Count != 0 && rectangles.Where(rect.IntersectsWith).Any()) continue;
                rectangles.Add(rect);
                return rect;
            }
            throw new InvalidOperationException("There is no place for rectangle!");
        }

        public List<Rectangle> GetListOfRectangles()
        {
            return new List<Rectangle>(rectangles);
        }
    }
}