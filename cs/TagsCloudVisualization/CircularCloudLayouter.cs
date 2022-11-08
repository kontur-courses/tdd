using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly ArchimedeanSpiral _spiral;
        private readonly List<Rectangle> _rectangles = new List<Rectangle>();
        private readonly double _angleStep = 0.01;
        private double _angle = 0;
        public Point Center { get; }
        public IReadOnlyList<Rectangle> Rectangles => _rectangles;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            _spiral = new ArchimedeanSpiral(center, 0, 0.25);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle = new Rectangle(Point.Empty, rectangleSize);
            rectangle = PlaceRectangle(rectangle);
            rectangle = ShiftRectangleToCenter(rectangle);
            _rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle PlaceRectangle(Rectangle rectangle)
        {
            do {
                rectangle.Location = _spiral.GetPoint(_angle);
                _angle += _angleStep;
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
            {
                rectangle.Location -= offset;
            }

            return rectangle;
        }
    }
}