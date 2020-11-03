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
        public int AddedRectanglesCount => cloudRectangles.Count;
        public IPointGenerator PointGenerator { get; }

        private readonly List<Rectangle> cloudRectangles;

        public CircularCloudLayouter(IPointGenerator pointGenerator)
        {
            if (pointGenerator == null)
                throw new ArgumentException("Point generator cannot be null");
            
            Center = pointGenerator.Center;
            PointGenerator = pointGenerator;
            cloudRectangles = new List<Rectangle>(5);
        }
        
        public Rectangle GetAddedRectangle(int index)
        {
            if (index < 0 || index >= cloudRectangles.Count)
                throw new ArgumentException("Index must be positive and less than collection elements count");
            
            return cloudRectangles[index];
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height >= PointGenerator.CanvasSize.Height ||
                rectangleSize.Width >= PointGenerator.CanvasSize.Width)
                throw new ArgumentException("Rectangle size must be less than canvas size");

            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Rectangle size must be more than zero");
            
            var point = PointGenerator.GetNextPoint();
            var countIterations = 0;

            while (!CheckRectanglePosition(point, rectangleSize))
            {
                point = PointGenerator.GetNextPoint();

                if (countIterations > 600)
                {
                    rectangleSize /= 2;
                    countIterations = 0;
                    PointGenerator.StartOver();
                }

                countIterations++;
            }
            
            
            var rectangle = GetLocatedRectangle(point, rectangleSize);
            cloudRectangles.Add(rectangle);
            PointGenerator.StartOver();
            
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
            if (cloudRectangles.Count == 0)
                return true;

            return cloudRectangles.All(
                rectangle => !rectangle.IntersectsWith(GetLocatedRectangle(position, rectangleSize)));
        }
    }
}