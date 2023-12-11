﻿using System.Drawing;
using System.Numerics;

namespace TagCloud
{
    public class CircularCloudLayouter : ITagCloudLayouter
    {
        private int i;
        private List<RectangleF> rectangles = new List<RectangleF>();
        private readonly PointF center;
        private RectangleF currentRectangle;
        private int segmentsCount = 2;
        private List<Vector2> directions = new List<Vector2>();

        public Rectangle[] Rectangles => rectangles.Select(Rectangle.Truncate).ToArray();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Rectangle can't be negative size.");

            currentRectangle = new RectangleF(center, rectangleSize);

            MoveOutOfCenterInDirection(ref currentRectangle, GetDirection());

            MoveTowardsCenter(ref currentRectangle);

            rectangles.Add(currentRectangle);

            return Rectangle.Truncate(currentRectangle);
        }

        private void MoveOutOfCenterInDirection(ref RectangleF rect, Vector2 direction)
        {
            while (IntersectsWithAny(rect))
                rect.Offset(direction.X, direction.Y);
        }

        private void MoveTowardsCenter(ref RectangleF rect)
        {
            bool movedDx = true, movedDy = true;
            while (movedDx || movedDy)
            {
                var dx = (int)(center.X - rect.X);
                var dy = (int)(center.Y - rect.Y);
                dx /= dx != 0 ? Math.Abs(dx) : 1;
                dy /= dy != 0 ? Math.Abs(dy) : 1;

                movedDx = dx != 0 && OffsetIfDontCollide(ref rect, dx, 0);
                movedDy = dy != 0 && OffsetIfDontCollide(ref rect, 0, dy);
            }
        }

        private bool OffsetIfDontCollide(ref RectangleF rect, float x, float y)
        {
            rect.Offset(x, y);

            if (IntersectsWithAny(rect))
            {
                rect.Offset(-x, -y);
                return false;
            }

            return true;
        }

        private bool IntersectsWithAny(RectangleF rect)
        {
            rect.X = (int)rect.X;
            rect.Y = (int)rect.Y;
            return rectangles.Any(r => rect.IntersectsWith(r));
        }

        private Vector2 GetDirection()
        {
            if (i % 100 == 0)
            {
                segmentsCount++;
                UpdateDirections();
            }

            return directions[i++ % directions.Count];
        }

        private void UpdateDirections()
        {
            var directions = new List<Vector2>();

            var step = 90f / segmentsCount;

            var multipliers = new[] { -1, 1, 1, -1, -1 };

            for (var angle = step; angle <= 90 - step; angle += step)
            {
                var radians = angle * Math.PI / 180;
                var tangent = Math.Tan(radians);
                var x = 1f;
                var y = (float)(tangent * x);

                for (var j = 0; j < multipliers.Length - 1; j++)
                {
                    var vector = new Vector2(x * multipliers[j], y * multipliers[j + 1]);
                    vector /= vector.Length();
                    directions.Add(vector);
                }
            }

            this.directions = directions;
        }
    }
}
