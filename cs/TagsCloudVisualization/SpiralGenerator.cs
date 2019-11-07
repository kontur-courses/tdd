﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralGenerator
    {
        public Point Center;
        private IEnumerator<Point> Generator { get; }
        private readonly HashSet<Point> prevPoints = new HashSet<Point>();

        public SpiralGenerator(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException();
            Center = center;
            Generator = GetNextPoints().GetEnumerator();
        }

        public Point GetNextPoint()
        {
            Generator.MoveNext();
            return Generator.Current;
        }

        private IEnumerable<Point> GetNextPoints()
        {
            float angle = 0;
            const float offsetAngle = (float)(5 * Math.PI / 180);
            while (true)
            {
                var radius = 7 * angle;
                var point = new Point(
                    (int)(Center.X + radius * Math.Cos(angle)),
                    (int)(Center.Y + radius * Math.Sin(angle)));
                if (!prevPoints.Contains(point))
                {
                    prevPoints.Add(point);
                    yield return point;
                }
                angle += offsetAngle;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
