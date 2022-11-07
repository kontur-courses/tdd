﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }

        public List<Rectangle> Reactangles { get; }

        public CircularCloudLayouter()
        {
            Center = new Point();
            Reactangles = new List<Rectangle>();
        }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Reactangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 1 || rectangleSize.Width < 1)
                throw new ArgumentException("");

            var rectangle = GetNextReactangle(rectangleSize);

            Reactangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle GetNextReactangle(Size rectangleSize) =>
            new Rectangle(GetNextReactanglePoint(rectangleSize), rectangleSize);

        private Point GetNextReactanglePoint(Size rectangleSize)
        {
            if (Reactangles.Count == 0)
                return ShiftPointRelativeTo(GetCenterPointFor(rectangleSize), Center);
            else
            {
                throw new NotImplementedException();
            }
        }

        private Point ShiftPointRelativeTo(Point point, Point otherPoint) =>
            Point.Subtract(point, new Size(otherPoint));

        private Point GetCenterPointFor(Size rectangleSize) =>
            new Point(-rectangleSize.Width / 2, -rectangleSize.Height / 2);

        public int GetWidth()
        {
            if (Reactangles.Count == 0)
                return 0;

            return Reactangles.Max(r => r.Right) - Reactangles.Min(r => r.Left);
        }

        public int GetHeight()
        {
            if (Reactangles.Count == 0)
                return 0;

            return Reactangles.Min(r => r.Bottom) - Reactangles.Max(r => r.Top);
        }
    }
}