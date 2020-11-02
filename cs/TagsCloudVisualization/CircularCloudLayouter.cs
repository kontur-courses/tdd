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

        private Dictionary<AngleDirection, Func<Point, Size, Rectangle>> directionToRectangle;

        public readonly Point Center;
        private readonly List<Rectangle> rectangles;
        private readonly List<Angle> angles;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
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

        public List<Rectangle> Rectangles => rectangles.ToList();

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
            {

                rectangles.Add(new Rectangle(Center.X - rectangleSize.Width / 2,
                    Center.Y - rectangleSize.Height / 2,
                    rectangleSize.Width, rectangleSize.Height));
                AddAngles(rectangles[0]);
            }
            else AddNewRectangle(rectangleSize);
            return rectangles[rectangles.Count - 1];
        }

        private void AddNewRectangle(Size rectangleSize)
        {
            var neededRectangle = angles
                .Select(angle => directionToRectangle[angle.Direction](angle.Pos, rectangleSize))
                .Where(rectangle => !rectangles.Any(anotherRectangle => anotherRectangle.IntersectsWith(rectangle)))
                .OrderBy(rect => Math.Sqrt(Math.Pow(rect.X + rect.Width / 2 - Center.X, 2) + Math.Pow(rect.Y + rect.Height / 2 - Center.Y, 2)))
                .First();
            rectangles.Add(neededRectangle);
            AddAngles(neededRectangle);
            var anglesToDelete = new List<Angle>();
            foreach (var angle in angles)
            {
                if (rectangles
                    .Any(rect => rect.IntersectsWith(directionToRectangle[angle.Direction](angle.Pos, new Size(1, 1)))))
                    anglesToDelete.Add(angle);
            }
            foreach (var angle in anglesToDelete)
                angles.Remove(angle);
        }

        private void AddAngles(Rectangle rect)
        {
            angles.Add(new Angle { Direction = AngleDirection.LeftBottom, Pos = rect.Location });
            angles.Add(new Angle { Direction = AngleDirection.RightTop, Pos = rect.Location });
            angles.Add(new Angle { Direction = AngleDirection.LeftBottom, Pos = rect.Location + rect.Size });
            angles.Add(new Angle { Direction = AngleDirection.RightTop, Pos = rect.Location + rect.Size });
            angles.Add(new Angle
            { Direction = AngleDirection.RightBottom, Pos = rect.Location + new Size(rect.Size.Width, 0) });
            angles.Add(new Angle
            { Direction = AngleDirection.LeftTop, Pos = rect.Location + new Size(rect.Size.Width, 0) });
            angles.Add(new Angle
            { Direction = AngleDirection.RightBottom, Pos = rect.Location + new Size(0, rect.Size.Height) });
            angles.Add(new Angle
            { Direction = AngleDirection.LeftTop, Pos = rect.Location + new Size(0, rect.Size.Height) });
        }
    }
}
