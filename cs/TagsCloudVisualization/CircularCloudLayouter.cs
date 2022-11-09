using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using TagsCloudVisualization.Curves;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> _rectangles = new List<Rectangle>();
        private readonly ICurve _curve;
        private readonly double _curveStep;
        private double _lastCurveParameter = 0;
        public IReadOnlyList<Rectangle> Rectangles => _rectangles;
        public Point Center { get; }
        
        public CircularCloudLayouter(ICurve curve, Point center, double curveStep = 0.01)
        {
            Center = center;
            _curve = curve;
            _curveStep = curveStep;
        }

        public CircularCloudLayouter(ICurve curve) : this(curve, Point.Empty) { }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Rectangles' width and height should be positive.");
            Rectangle rectangle = new Rectangle(Point.Empty, rectangleSize);
            rectangle = PlaceRectangle(rectangle);
            rectangle = ShiftRectangleToCenter(rectangle);
            _rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle PlaceRectangle(Rectangle rectangle)
        {
            do {
                rectangle.Location = _curve.GetPoint(_lastCurveParameter) + (Size)Center;
                _lastCurveParameter += _curveStep;
            } while (rectangle.IntersectsWith(_rectangles));

            return rectangle;
        }
        
        private Rectangle ShiftRectangleToCenter(Rectangle rectangle)
        {
            int dx = (rectangle.GetCenter().X < Center.X) ? 1 : -1;
            rectangle = ShiftRectangle(rectangle, dx, 0);
            int dy = (rectangle.GetCenter().Y < Center.Y) ? 1 : -1;
            rectangle = ShiftRectangle(rectangle, 0, dy);
            return rectangle;
        }
        
        private Rectangle ShiftRectangle(Rectangle rectangle, int dx, int dy)
        {
            Size offset = new Size(dx, dy);
            while (rectangle.IntersectsWith(_rectangles) == false && 
                   rectangle.GetCenter().X != Center.X &&
                   rectangle.GetCenter().Y != Center.Y)
            {
                rectangle.Location += offset;
            }

            if (rectangle.IntersectsWith(_rectangles))
                rectangle.Location -= offset;

            return rectangle;
        }
    }
}