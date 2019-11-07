using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Utils
    {
        public static Size GenerateRandomSize(this Random me)
        {
            int height = me.Next(10, 50);
            int width = (int)(height * (2 + 3 * me.NextDouble()));
            return new Size(width, height);
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

        private static ValueTuple<Point, Point> LeftVerticalSegment(this Rectangle me) =>
            new ValueTuple<Point, Point>(me.LeftTop(), me.LeftBottom());

        private static ValueTuple<Point, Point> RightVerticalSegment(this Rectangle me) =>
            new ValueTuple<Point, Point>(me.RightTop(), me.RightBottom());

        private static ValueTuple<Point, Point> TopHorizontalSegment(this Rectangle me) =>
            new ValueTuple<Point, Point>(me.LeftTop(), me.RightTop());

        private static ValueTuple<Point, Point> BottomHorizontalSegment(this Rectangle me) =>
            new ValueTuple<Point, Point>(me.LeftBottom(), me.RightBottom());

        private static bool IsRayIntersectsHorizontalSegment(double rayAngle, ValueTuple<Point, Point> segment, out double intersectionPointDistance)
        {
            if (rayAngle.IsApproximatelyEquals(1.5 * Math.PI) && segment.Item1.Y < 0)
            {
                if (segment.Item1.X <= 0 && segment.Item2.X >= 0)
                {
                    intersectionPointDistance = DistFromCenter(new Point(0, segment.Item1.Y));
                    return true;
                }
            }
            else if (rayAngle.IsApproximatelyEquals(0.5 * Math.PI) && segment.Item1.Y > 0)
            {
                if (segment.Item1.X <= 0 && segment.Item2.X >= 0)
                {
                    intersectionPointDistance = DistFromCenter(new Point(0, segment.Item1.Y));
                    return true;
                }
            }
            else if ((rayAngle > Math.PI && segment.Item1.Y < 0) || (rayAngle < Math.PI && segment.Item1.Y > 0))
            {
                double x = segment.Item1.Y / Math.Tan(rayAngle);
                if (x.IsApproximatelyMoreThan(segment.Item1.X) && x.IsApproximatelyLessThan(segment.Item2.X))
                {
                    intersectionPointDistance = DistFromCenter(new Point((int)x, segment.Item1.Y));
                    return true;
                }
            }

            intersectionPointDistance = default;
            return false;
        }

        private static bool IsRayIntersectsVerticalSegment(double rayAngle, ValueTuple<Point, Point> segment, out double intersectionPointDistance)
        {
            if (rayAngle.IsApproximatelyEquals(Math.PI) && segment.Item1.X < 0)
            {
                if (segment.Item1.Y <= 0 && segment.Item2.Y >= 0)
                {
                    intersectionPointDistance = DistFromCenter(new Point(segment.Item1.X, 0));
                    return true;
                }
            }
            else if (rayAngle.IsApproximatelyEquals(0) && segment.Item1.X > 0)
            {
                if (segment.Item1.Y <= 0 && segment.Item2.Y >= 0)
                {
                    intersectionPointDistance = DistFromCenter(new Point(segment.Item1.X, 0));
                    return true;
                }
            }
            else if (((rayAngle > 0.5 * Math.PI || rayAngle < 1.5 * Math.PI) && segment.Item1.X < 0)
                || ((rayAngle < 0.5 * Math.PI || rayAngle > 1.5 * Math.PI) && segment.Item1.X > 0))
            {
                double y = Math.Tan(rayAngle) * segment.Item1.X;
                if (y.IsApproximatelyMoreThan(segment.Item1.Y) && y.IsApproximatelyLessThan(segment.Item2.Y))
                {
                    intersectionPointDistance = DistFromCenter(new Point(segment.Item1.X, (int)y));
                    return true;
                }
            }

            intersectionPointDistance = default;
            return false;
        }

        public static bool IsIntersectsByRay(this Rectangle me, double rayAngle, out double intersectionPointDistance)
        {
            ValueTuple<Point, Point> horizontalSegment, verticalSegment;

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
    }

    [TestFixture]
    public class UtilsTests
    {
        [TestCase(50, -5, 0)]
        [TestCase(-5, 50, Math.PI / 2)]
        [TestCase(-50, -5, Math.PI)]
        [TestCase(-5, -50, 1.5 * Math.PI)]
        [TestCase(50, 50, Math.PI / 4)]
        [TestCase(-50, 50, Math.PI * 3 / 4)]
        [TestCase(-50, -50, Math.PI * 1.25)]
        [TestCase(50, -50, 1.76 * Math.PI)]
        public void Rectangle_IsIntersectsByRay_ShouldReturnTrue(int rectX, int rectY, double rayAngle)
        {
            new Rectangle(rectX, rectY, 10, 10).IsIntersectsByRay(rayAngle, out double _).Should().BeTrue();
        }

        [TestCase(50, 20, 0, ExpectedResult = 25)]
        [TestCase(10, 10, Math.PI / 4, ExpectedResult = 7)]
        [TestCase(66, 11, 0.17453292519943295, ExpectedResult = 33)]
        public int LengthOfRayFromCenterOfRectangle_ReturnsCorrectValue(int width, int height, double rayAngle)
        {
            return (int)Utils.LengthOfRayFromCenterOfRectangle(new Rectangle(0, 0, width, height), rayAngle);
        }

        [TestCase(0, 10, 10, 0)]
        [TestCase(Math.PI, 10, -10, 0)]
        [TestCase(Math.PI / 4, 10, 7, 7)]
        public void Point_FromPolar_ReturnsCorrectValue(double angle, double dist, int x, int y)
        {
            new Point().FromPolar(angle, dist).Should().Be(new Point(x, y));
        }

        [TestCase(0, 0, (int)(1.41 * 10))]
        [TestCase(-10, 0, (int)(1.41 * 10))]
        [TestCase(-10, -10, (int)(1.41 * 10))]
        [TestCase(0, -10, (int)(1.41 * 10))]
        public void GetDistanceOfFathestFromCenterVertex_ReturnsCorrectValue(int rectX, int rectY, int distance)
        {
            ((int)new Rectangle(rectX, rectY, 10, 10).GetDistanceOfFathestFromCenterVertex()).Should().Be(distance);
        }

        [Test]
        public void IsIntersectsByRay_ReturnsCorrectValue()
        {
            var layouter = new CircularCloudLayouter(default);
            layouter.PutNextRectangle(new Size(20, 20));
            layouter.PutNextRectangle(new Size(10, 10));

            layouter
                .GetRectangles()
                .Select(r => r.IsIntersectsByRay(0.5 * Math.PI, out double intersectionPointDistance) ? intersectionPointDistance : 0)
                .Max()
                .Should().BeInRange(20, 25);
        }
    }
}
