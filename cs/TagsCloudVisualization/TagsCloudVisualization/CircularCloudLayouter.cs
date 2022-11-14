using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : IRectangleLayouter
    {
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
        }

        public IReadOnlyList<Rectangle> Rectangles => rectangles;
        public Point Center { get; }
        private readonly List<Rectangle> rectangles;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle newRectangle;

            if (rectangles.Count == 0)
                newRectangle = new Rectangle(Center - rectangleSize / 2, rectangleSize);
            else
                newRectangle = FindPlaceForRectangle(rectangleSize);

            rectangles.Add(newRectangle);
            return newRectangle;
        }

        private Rectangle FindPlaceForRectangle(Size rectangleSize)
        {
            var rectangleScale = Math.Max(rectangleSize.Height, rectangleSize.Width);

            // So that angles never recur and are distributed evenly:
            var startAngleOfSpiral = rectangles.Count;
            
            foreach (var rectangleCenter in Center.ScatterPointsBySpiralAround(rectangleScale, startAngleOfSpiral))
            {
                var rectangle = RectangleExtensions.NewRectangle(rectangleCenter, rectangleSize);
                if (IntersectsWithExistingRectangles(rectangle)) 
                    continue;
               
                rectangles.Add(rectangle);
                return rectangle;
            }

            throw new InvalidOperationException(
                $"Could not find a place for a rectangle of width {rectangleSize.Width} and height {rectangleSize.Height}");
        }

        public double GetCircumcircleRadius()
        {
            return Rectangles.SelectMany(r => r.GetVertices()).Select(p => Center.GetDistanceTo(p))
                .Max();
        }


        private bool IntersectsWithExistingRectangles(Rectangle newRectangle)
        {
            return Rectangles.Any(r => r.IntersectsWith(newRectangle));
        }
    }
}