using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private const int SpiralCoefficient = 8;
        private Point centerPosition;
        public List<Rectangle> layout = new List<Rectangle>();

        private int spiralCounter = 0;

        public CircularCloudLayouter(Point center)
        {
            centerPosition = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rect = new Rectangle(GetNextPointOnSpiral(centerPosition) - rectangleSize / 2, rectangleSize);
            while (layout.Any(r => r.IntersectsWith(rect)))
                rect = new Rectangle(GetNextPointOnSpiral(centerPosition) - rectangleSize / 2, rectangleSize);
            layout.Add(rect);
            return rect;
        }

        public Point GetNextPointOnSpiral(Point center)
        {
            int x = (int) (SpiralCoefficient * MathF.Pow(spiralCounter, 1 / 2f) * MathF.Cos(spiralCounter)) + center.X;
            int y = (int) (SpiralCoefficient * MathF.Pow(spiralCounter, 1 / 2f) * MathF.Sin(spiralCounter)) + center.Y;
            Point pos = new Point(x, y);
            spiralCounter++;
            return pos;
        }
    }
}