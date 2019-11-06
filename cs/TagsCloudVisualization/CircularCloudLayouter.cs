using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public readonly Size center;
        private readonly List<Rectangle> rectangles;
        private readonly IEnumerator<Point> spiralEnumerator;
        private Rectangle cloudRectangle;

        public CircularCloudLayouter(Point center)
        {
            rectangles = new List<Rectangle>();
            this.center = new Size(center);
            spiralEnumerator = new Spiral(5).GetNextPointOnSpiral();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Directions should be non-negative");

            Rectangle rectangle = new Rectangle(spiralEnumerator.Current + center, rectangleSize);

            while (spiralEnumerator.MoveNext())
            {
                rectangle = new Rectangle(spiralEnumerator.Current + center, rectangleSize);
                if (rectangles.Count == 0 || !rectangles.Any(rect => rect.IntersectsWith(rectangle)))
                    break;
            }

            rectangle = SnuggleRectangle(rectangle);

            rectangles.Add(rectangle);
            
            UpdateCloudRectangle(rectangle);

            return rectangle;
        }

        public IEnumerable<Rectangle> GetRectangles()
        {
            foreach (var rectangle in rectangles)
            {
                yield return rectangle;
            }
        }

        public Rectangle CloudRectangle => cloudRectangle;

        private Rectangle SnuggleRectangle(Rectangle rectangle)
        {
            var tempRectangle = rectangle;
            var additionalLoop = true;
            while (additionalLoop)
            {
                var deltaX = Math.Sign(center.Width - rectangle.X);
                var deltaY = Math.Sign(center.Height - rectangle.Y);
                while (deltaX != 0 && !rectangles.Any(rect => rect.IntersectsWith(tempRectangle)))
                {
                    rectangle = tempRectangle;
                    tempRectangle.X += deltaX;
                    deltaX = Math.Sign(center.Width - rectangle.X);
                }
                additionalLoop = false;

                while (deltaY != 0 && !rectangles.Any(rect => rect.IntersectsWith(tempRectangle)))
                {
                    rectangle = tempRectangle;
                    tempRectangle.Y += deltaY;
                    deltaY = Math.Sign(center.Height - rectangle.Y);
                    additionalLoop = true;
                }
            }

            return rectangle;
        }

        private void UpdateCloudRectangle(Rectangle rectangle)
        {
            if (rectangle.X < CloudRectangle.X)
            {
                cloudRectangle.Width += cloudRectangle.X - rectangle.X;
                cloudRectangle.X = rectangle.X;
            }

            if (rectangle.Y < CloudRectangle.Y)
            {
                cloudRectangle.Height += cloudRectangle.Y - rectangle.Y;
                cloudRectangle.Y = rectangle.Y;
            }

            if (rectangle.Right > cloudRectangle.Right)
            {
                cloudRectangle.Width += rectangle.Right - cloudRectangle.Right;
            }

            if (rectangle.Bottom > cloudRectangle.Bottom)
            {
                cloudRectangle.Height += rectangle.Bottom - cloudRectangle.Bottom;
            }
        }
    }
}
