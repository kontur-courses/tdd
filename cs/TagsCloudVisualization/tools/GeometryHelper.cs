using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace TagsCloudVisualization.tools
{
    public static class GeometryHelper
    {
        private class PointByAngleComparer : IComparer<Point>
        {
            private readonly Point basePoint;

            public PointByAngleComparer(Point basePoint)
            {
                this.basePoint = basePoint;
            }

            public int Compare(Point x, Point y)
            {
                var a1 = GetAngle(x);
                var a2 = GetAngle(y);

                if (Math.Abs(a1 - a2) < 0.00001)
                {
                    return basePoint.Distance(x).CompareTo(basePoint.Distance(y));
                }

                return a1.CompareTo(a2);
            }

            private double GetAngle(Point p)
            {
                var dx = p.X - basePoint.X;
                var dy = p.Y - basePoint.Y;
                var angle = Math.Atan2(dy, dx);

                if (angle < 0)
                {
                    angle += 2 * Math.PI;
                }

                return angle;
            }
        }

        public static double GetSquareOfConvexHull(List<Point> hull)
        {
            var square = 0.0;
            var count = hull.Count;

            for (var i = 0; i < count; i++)
            {
                var j = i == count - 1 ? 0 : i + 1;
                var p1 = hull[i];
                var p2 = hull[j];

                square += (p1.X + p2.X) * (p1.Y - p2.Y);
            }

            return 0.5 * Math.Abs(square);
        }

        public static List<Point> BuildConvexHull(List<Rectangle> rectangles)
        {
            var points = rectangles.SelectMany(t => t.Vertexes()).ToList();

            return BuildConvexHull(points);
        }

        public static List<Point> BuildConvexHull(List<Point> points)
        {
            if (points.Count < 3)
                throw new ArgumentException("There are too little points for hull");

            var down = GetDownLeft(points);
            var comparer = new PointByAngleComparer(down);

            points = points
                .Where(p => p != down)
                .OrderBy(p => p, comparer)
                .ToList();

            var hull = new Stack<Point>();

            hull.Push(down);
            hull.Push(points[0]);

            var m = points.Count;

            for (var i = 1; i < m; i++)
            {
                var pi = points[i];
                var nextToTop = hull.NextToTop();
                var top = hull.Peek();

                if (OnLine(nextToTop, top, pi))
                {
                    hull.Pop();
                    hull.Push(pi);
                    continue;
                }
                
                while (!IsLeftRotation(nextToTop, top, pi))
                {
                    hull.Pop();

                    top = hull.Peek();
                    nextToTop = hull.NextToTop();
                }

                hull.Push(pi);
            }

            return hull.ToList();
        }

        private static bool OnLine(Point a, Point b, Point c)
        {
            return GetRotation(a, b, c) == 0;
        }

        private static bool IsLeftRotation(Point a, Point b, Point c)
        {
            return GetRotation(a, b, c) > 0;
        }

        private static int GetRotation(Point a, Point b, Point c)
        {
            var ux = b.X - a.X;
            var uy = b.Y - a.Y;
            var vx = c.X - b.X;
            var vy = c.Y - b.Y;

            return ux * vy - uy * vx;
        }

        private static Point GetDownLeft(List<Point> points)
        {
            var current = points[0];

            foreach (var p in points)
            {
                if (p.Y < current.Y || p.Y == current.Y && p.X < current.X)
                {
                    current = p;
                }
            }

            return current;
        }
    }
}
