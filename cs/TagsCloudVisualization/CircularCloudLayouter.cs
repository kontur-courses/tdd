using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point _center;
        private readonly List<Rectangle> _rectangles = new();
        private readonly IVectorsGenerator _vectorsGenerator = new CircularVectorsGenerator(0.005, 360);

        public CircularCloudLayouter(Point center)
        {
            _center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0) throw new ArgumentException("Width should be positive");
            if (rectangleSize.Height <= 0) throw new ArgumentException("Height should be positive");

            var rectangle = CreateCorrectRectangle(rectangleSize);
            _rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle CreateCorrectRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(_center, rectangleSize);
            while (true)
            {
                var vector = _vectorsGenerator.GetNextVector();
                rectangle.X = _center.X + vector.X - rectangleSize.Width / 2;
                rectangle.Y = _center.Y + vector.Y - rectangleSize.Height / 2;
                if (!_rectangles.Any(x => x.IntersectsWith(rectangle))) return GetShiftedToCenterRectangle(rectangle);
            }
        }

        private Rectangle GetShiftedToCenterRectangle(Rectangle rectangle)
        {
            var center = rectangle.GetCenter();
            var shift =  new Point(Math.Sign(_center.X - center.X), Math.Sign(_center.Y - center.Y));

            while (Math.Abs(_center.X - rectangle.GetCenter().X) > 0)
            {
                rectangle.X += shift.X;
                if (_rectangles.Any(rect => rect.IntersectsWith(rectangle)))
                {
                    rectangle.X -= shift.X;
                    break;
                }
            }

            while (Math.Abs(_center.Y - rectangle.GetCenter().Y) > 0)
            {
                rectangle.Y += shift.Y;
                if (_rectangles.Any(rect => rect.IntersectsWith(rectangle)))
                {
                    rectangle.Y -= shift.Y;
                    break;
                }
            }

            return rectangle;
        }
    }
}