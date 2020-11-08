using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualization.TagCloud
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point Center { get; }
        
        private readonly IPointGenerator pointGenerator;
        private readonly List<Rectangle> cloudRectangles;

        public CircularCloudLayouter(IPointGenerator pointGenerator)
        {
            if (pointGenerator == null)
                throw new ArgumentException("Point generator cannot be null");
            
            Center = pointGenerator.Center;
            this.pointGenerator = pointGenerator;
            cloudRectangles = new List<Rectangle>();
        }

        public IEnumerable<Rectangle> GetAddedRectangles() => cloudRectangles;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Rectangle size must be more than zero");

            var point = pointGenerator.GetNextPoint();
            
            while (!CheckRectanglePosition(point!.Value, rectangleSize))
            {
                point = pointGenerator.GetNextPoint();
                if (!point.HasValue)
                    throw new ArgumentException($"Location for rectangle with size {rectangleSize} not found");
            }

            var rectangle = GetLocatedRectangle(point.Value, rectangleSize);
            cloudRectangles.Add(rectangle);
            pointGenerator.StartOver();
            
            return rectangle;
        }

        private Rectangle GetLocatedRectangle(Point position, Size rectangleSize)
        {
            return new Rectangle(new Point(
                    position.X - rectangleSize.Width / 2,
                    position.Y - rectangleSize.Height / 2),
                rectangleSize);
        }

        private bool CheckRectanglePosition(Point position, Size rectangleSize)
        {
            return !cloudRectangles.Any(
                rectangle => rectangle.IntersectsWith(GetLocatedRectangle(position, rectangleSize)));
        }
    }
}