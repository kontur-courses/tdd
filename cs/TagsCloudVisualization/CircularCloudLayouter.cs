using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private enum AngleDirection
        {
            LeftBottom,
            RightBottom,
            LeftTop,
            RightTop
        }

        private class Angle
        {
            public Point Pos;
            public AngleDirection Direction;
        }

        private readonly Dictionary<AngleDirection, Func<Point, Size, Rectangle>> directionToRectangle;

        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        private readonly List<Angle> angles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
            angles = new List<Angle>();
            directionToRectangle = new Dictionary<AngleDirection, Func<Point, Size, Rectangle>>
            {
                {
                    AngleDirection.RightBottom, (point, size) =>
                        new Rectangle(point, size)
                },
                {
                    AngleDirection.LeftBottom, (point, size) =>
                        new Rectangle(new Point(point.X - size.Width, point.Y), size)
                },
                {
                    AngleDirection.RightTop, (point, size) =>
                        new Rectangle(new Point(point.X, point.Y - size.Height), size)
                },
                {
                    AngleDirection.LeftTop, (point, size) =>
                        new Rectangle(point - size, size)
                }
            };
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
            {
                var firstRectangle = new Rectangle(center.X - rectangleSize.Width / 2,
                    center.Y - rectangleSize.Height / 2,
                    rectangleSize.Width, rectangleSize.Height);
                rectangles.Add(firstRectangle);
                AddAngles(rectangles[0]);
                return firstRectangle;
            }
            return AddNewRectangle(rectangleSize);
        }

        private Rectangle AddNewRectangle(Size rectangleSize)
        {
            var resultTuple = angles
                .Select(angle => (rectangle: directionToRectangle[angle.Direction](angle.Pos, rectangleSize), angle))
                .Where(rectangle => !rectangles.Any(anotherRectangle => anotherRectangle.IntersectsWith(rectangle.Item1)))
                .OrderBy(rect => 
                    Math.Sqrt(Math.Pow(rect.rectangle.X + rect.rectangle.Width / 2 - center.X, 2) + Math.Pow(rect.rectangle.Y + rect.rectangle.Height / 2 - center.Y, 2)))
                .First();
            rectangles.Add(resultTuple.rectangle);
            angles.Remove(resultTuple.angle);
            AddAngles(resultTuple.rectangle);
            return resultTuple.rectangle;
        }

        private void AddAngles(Rectangle rect)
        {
            angles.Add(new Angle { Direction = AngleDirection.LeftBottom, Pos = rect.Location });
            angles.Add(new Angle { Direction = AngleDirection.RightTop, Pos = rect.Location });
            angles.Add(new Angle { Direction = AngleDirection.LeftBottom, Pos = rect.Location + rect.Size });
            angles.Add(new Angle { Direction = AngleDirection.RightTop, Pos = rect.Location + rect.Size });
            angles.Add(new Angle { Direction = AngleDirection.RightBottom, Pos = rect.Location + new Size(rect.Size.Width, 0) });
            angles.Add(new Angle { Direction = AngleDirection.LeftTop, Pos = rect.Location + new Size(rect.Size.Width, 0) });
            angles.Add(new Angle { Direction = AngleDirection.RightBottom, Pos = rect.Location + new Size(0, rect.Size.Height) });
            angles.Add(new Angle { Direction = AngleDirection.LeftTop, Pos = rect.Location + new Size(0, rect.Size.Height) });
        }
    }
}
