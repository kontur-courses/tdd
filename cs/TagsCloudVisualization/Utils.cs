﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Utils
    {
        public static Point NextPoint(this Random me, int minX, int maxX, int minY, int maxY) =>
            new Point(me.Next(minX, maxX), me.Next(minY, maxY));

        public static Size NextSize(this Random me, int minHeight, int maxHeight, double minWidthFactor, double maxWidthFactor)
        {
            if (minWidthFactor > maxWidthFactor)
                throw new ArgumentException("'maxWidthFactor' must be more than 'minWidthFactor'.");

            var height = me.Next(minHeight, maxHeight);
            var width = (int)(height * (minWidthFactor + (maxWidthFactor - minWidthFactor) * me.NextDouble()));
            return new Size(width, height);
        }

        public static IEnumerable<Size> NextSizes(this Random me, int minHeight, int maxHeight, int minWidthFactor, int maxWidthFactor, int count)
        {
            if (count <= 0)
                throw new ArgumentException("'Count' parameter must be a positive nonzero number.");

            return Enumerable.Range(0, count).Select(i => me.NextSize(minHeight, maxHeight, minWidthFactor, maxWidthFactor));
        }

        public static IEnumerable<Size> NextSizes(this Random me, int minHeight, int maxHeight, int minWidthFactor, int maxWidthFactor, int minCount, int maxCount)
        {
            if (minCount > maxCount)
                throw new ArgumentException("'maxCount' must be more than 'minCount'.");

            return me.NextSizes(minHeight, maxHeight, minWidthFactor, maxWidthFactor, me.Next(minCount, maxCount));
        }

        private static bool IsApproximatelyEquals(this double me, double val)
        {
            return Math.Abs(me - val) < 1E-6;
        }

        private static bool IsApproximatelyMoreThan(this double me, double val)
        {
            return me > val || me.IsApproximatelyEquals(val);
        }

        private static bool IsApproximatelyLessThan(this double me, double val)
        {
            return me < val || me.IsApproximatelyEquals(val);
        }

        private static double DistFromCenter(this Point me) => Math.Sqrt(me.X * me.X + me.Y * me.Y);

        public static Point FromPolar(this Point me, double angle, double dist)
        {
            me.X = (int)(dist * Math.Cos(angle));
            me.Y = (int)(dist * Math.Sin(angle));
            return me;
        }

        public static double Square(this Rectangle me) => me.Width * me.Height;

        private static Point LeftTop(this Rectangle me) => me.Location;
        private static Point RightTop(this Rectangle me) => new Point(me.Right, me.Top);
        private static Point RightBottom(this Rectangle me) => new Point(me.Right, me.Bottom);
        private static Point LeftBottom(this Rectangle me) => new Point(me.Left, me.Bottom);
        private static IEnumerable<Point> Vertices(this Rectangle me) => new Point[] { me.LeftTop(), me.RightTop(), me.RightBottom(), me.LeftBottom() };

        public static double GetDistanceOfFathestFromCenterVertex(this Rectangle me) => me.Vertices().Max(v => v.DistFromCenter());

        private static (Point First, Point Second) LeftVerticalSegment(this Rectangle me) =>
            (me.LeftTop(), me.LeftBottom());

        private static (Point First, Point Second) RightVerticalSegment(this Rectangle me) =>
            (me.RightTop(), me.RightBottom());

        private static (Point First, Point Second) TopHorizontalSegment(this Rectangle me) =>
            (me.LeftTop(), me.RightTop());

        private static (Point First, Point Second) BottomHorizontalSegment(this Rectangle me) =>
            (me.LeftBottom(), me.RightBottom());

        private static bool IsRayIntersectsHorizontalSegment(double rayAngle, (Point First, Point Second) segment, out double intersectionPointDistance)
        {
            if (rayAngle.IsApproximatelyEquals(1.5 * Math.PI) && segment.First.Y < 0)
            {
                if (segment.First.X <= 0 && segment.Second.X >= 0)
                {
                    intersectionPointDistance = DistFromCenter(new Point(0, segment.First.Y));
                    return true;
                }
            }
            else if (rayAngle.IsApproximatelyEquals(0.5 * Math.PI) && segment.First.Y > 0)
            {
                if (segment.First.X <= 0 && segment.Second.X >= 0)
                {
                    intersectionPointDistance = DistFromCenter(new Point(0, segment.First.Y));
                    return true;
                }
            }
            else if ((rayAngle > Math.PI && segment.First.Y < 0) || (rayAngle < Math.PI && segment.First.Y > 0))
            {
                double x = segment.First.Y / Math.Tan(rayAngle);
                if (x.IsApproximatelyMoreThan(segment.First.X) && x.IsApproximatelyLessThan(segment.Second.X))
                {
                    intersectionPointDistance = DistFromCenter(new Point((int)x, segment.First.Y));
                    return true;
                }
            }

            intersectionPointDistance = default;
            return false;
        }

        private static bool IsRayIntersectsVerticalSegment(double rayAngle, (Point First, Point Second) segment, out double intersectionPointDistance)
        {
            if (rayAngle.IsApproximatelyEquals(Math.PI) && segment.First.X < 0)
            {
                if (segment.First.Y <= 0 && segment.Second.Y >= 0)
                {
                    intersectionPointDistance = DistFromCenter(new Point(segment.First.X, 0));
                    return true;
                }
            }
            else if (rayAngle.IsApproximatelyEquals(0) && segment.First.X > 0)
            {
                if (segment.First.Y <= 0 && segment.Second.Y >= 0)
                {
                    intersectionPointDistance = DistFromCenter(new Point(segment.First.X, 0));
                    return true;
                }
            }
            else if (((rayAngle > 0.5 * Math.PI || rayAngle < 1.5 * Math.PI) && segment.First.X < 0)
                || ((rayAngle < 0.5 * Math.PI || rayAngle > 1.5 * Math.PI) && segment.First.X > 0))
            {
                double y = Math.Tan(rayAngle) * segment.First.X;
                if (y.IsApproximatelyMoreThan(segment.First.Y) && y.IsApproximatelyLessThan(segment.Second.Y))
                {
                    intersectionPointDistance = DistFromCenter(new Point(segment.First.X, (int)y));
                    return true;
                }
            }

            intersectionPointDistance = default;
            return false;
        }

        public static bool IsIntersectsByRay(this Rectangle me, double rayAngle, out double intersectionPointDistance)
        {
            (Point First, Point Second) horizontalSegment, verticalSegment;

            if (rayAngle >= 1.5 * Math.PI)
            {
                horizontalSegment = me.TopHorizontalSegment();
                verticalSegment = me.RightVerticalSegment();
            }
            else if (rayAngle >= Math.PI)
            {
                horizontalSegment = me.TopHorizontalSegment();
                verticalSegment = me.LeftVerticalSegment();
            }
            else if (rayAngle >= 0.5 * Math.PI)
            {
                horizontalSegment = me.BottomHorizontalSegment();
                verticalSegment = me.LeftVerticalSegment();
            }
            else
            {
                horizontalSegment = me.BottomHorizontalSegment();
                verticalSegment = me.RightVerticalSegment();
            }

            return IsRayIntersectsHorizontalSegment(rayAngle, horizontalSegment, out intersectionPointDistance)
                || IsRayIntersectsVerticalSegment(rayAngle, verticalSegment, out intersectionPointDistance);
        }

        public static double LengthOfRayFromCenterOfRectangle(Rectangle rect, double rayAngle)
        {
            var tmpRect = new Rectangle(-rect.Width / 2, -rect.Height / 2, rect.Width, rect.Height);
            tmpRect.IsIntersectsByRay(rayAngle, out double intersectionPointDistance);
            return intersectionPointDistance;
        }

        public static void SaveRectanglesToPngFile(IEnumerable<Rectangle> rectangles, string filename)
        {
            if (rectangles.Count() == 0)
                throw new ArgumentException("There are no items.");

            var left = rectangles.Min(r => r.Left);
            var right = rectangles.Max(r => r.Right);
            var top = rectangles.Min(r => r.Top);
            var bottom = rectangles.Max(r => r.Bottom);

            using (var bmp = new Bitmap(right - left, bottom - top))
            {
                using (var graphics = Graphics.FromImage(bmp))
                {
                    graphics.Clear(Color.RosyBrown);
                    var brush = new SolidBrush(Color.Green);
                    var pen = new Pen(Color.Black);
                    var textBrush = new SolidBrush(Color.Black);
                    var i = 0;
                    foreach (var rect in rectangles)
                    {
                        graphics.FillRectangle(brush, rect.X - left, rect.Y - top, rect.Width, rect.Height);
                        graphics.DrawRectangle(pen, rect.X - left, rect.Y - top, rect.Width, rect.Height);
                        graphics.DrawString((++i).ToString(), new Font("Tahoma", rect.Height, GraphicsUnit.Pixel), textBrush, rect.X - left, rect.Y - top - 1);
                    }
                }
                bmp.Save(filename, ImageFormat.Png);
            }
        }
    }
}