using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudVisualization.Models.Interfaces;

namespace TagsCloudVisualization.Models
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles = new();
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0) //первый прямоугольник по центру
            {
                var firstRectangle = CreateRectangle(rectangleSize, center);
                rectangles.Add(firstRectangle);
                return firstRectangle;
            }

            var step = 0;

            while(true)
            {
                var candidateCenter = GetSpiralPoint(step);
                var candidateRectangle = CreateRectangle(rectangleSize, candidateCenter);

                if (IntersectAny(candidateRectangle))
                {
                    step++;
                    continue;
                }

                rectangles.Add(candidateRectangle);
                return candidateRectangle;
            }
        }

        private Rectangle CreateRectangle(Size size, Point rectangleCenter) =>
            new Rectangle
            (
                rectangleCenter.X - size.Width / 2,
                rectangleCenter.Y - size.Height / 2,
                size.Width,
                size.Height
            );

        private bool IntersectAny(Rectangle rectangle)
        {
            foreach (var placedRectangle in rectangles)
            {
                if (rectangle.IntersectsWith(placedRectangle))
                    return true;    
            }

            return false;
        }

        private Point GetSpiralPoint(int step)
        {
            var angle = step * 0.1;
            var radius = step * 0.5;

            var x = center.X + radius * Math.Cos(angle);
            var y = center.Y + radius * Math.Sin(angle);

            return new Point
                (
                    (int)Math.Round(x),
                    (int)Math.Round(y)
                );
        }
    }
}
