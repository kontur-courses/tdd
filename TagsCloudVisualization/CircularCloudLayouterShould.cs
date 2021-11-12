using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using NUnit.Framework;
using FluentAssertions;


namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        [Test]
        public void InitializeFieldsAfterInstanceCreation()
        {
            var center = new Point(10, 10);

            var layouter = new CircularCloudLayouter(center);

            layouter.Rectangles.Should().NotBeNull();
            layouter.CloudCenter.Should().BeEquivalentTo(center);
        }

        [TestCase(-1, 20, TestName = "width or height is negative")]
        [TestCase(10, 0, TestName = "width or height equals zero")]
        public void ThrowExceptionWhenRectangleSizeIsIncorrect(int width, int height)
        {
            var rectangleSize = new Size(-1, 20);
            var center = new Point(10, 10);
            var layouter = new CircularCloudLayouter(center);
            Action act = () => layouter.PutNextRectangle(rectangleSize);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutFirstRectangleInTheCloudCenter()
        {
            var rectangleSize = new Size(100, 100);
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var expectedLocation = new Point(-50, -50);

            var firstRectangle = layouter.PutNextRectangle(rectangleSize);

            firstRectangle.Location.Should().BeEquivalentTo(expectedLocation);
        }

        [Test]
        public void MakeCloudCircleDeviationLessThanTwentyFivePercents()
        {
            var rectanglesCount = 1000;
            var maxHeight = 35;
            var maxWidth = 70;
            var center = new Point(750, 750);
            var layouter = new CircularCloudLayouter(center);
            var rnd = new Random();

            PutRectangles(layouter, rnd, maxWidth, maxHeight, rectanglesCount);
            var cloudConvexHull = GetCloudConvexHull(layouter);
            var minMaxLengths = GetMinMaxHullVectorsLengths(center, cloudConvexHull);
            var deviation = 1 - Math.Abs(minMaxLengths.Item1 / minMaxLengths.Item2);

            deviation.Should().BeLessOrEqualTo(0.25);
        }

        [Test]
        public void MakeCloudDense()
        {
            var rectanglesCount = 1000;
            var maxHeight = 35;
            var maxWidth = 70;
            var center = new Point(750, 750);
            var layouter = new CircularCloudLayouter(center);
            var rnd = new Random();

            PutRectangles(layouter, rnd, maxWidth, maxHeight, rectanglesCount);
            var cloudConvexHull = GetCloudConvexHull(layouter);

            var cloudArea = layouter.Rectangles.Sum(rect => rect.Width * rect.Height);
            var enclosingCircleRadius = GetMinMaxHullVectorsLengths(center, cloudConvexHull).Item2;
            var enclosingCircleArea = Math.PI * enclosingCircleRadius * enclosingCircleRadius;
            var deviation = 1 - Math.Abs(cloudArea / enclosingCircleArea);

            deviation.Should().BeLessOrEqualTo(0.25);
        }

        private static IEnumerable<Point> GetCloudConvexHull(CircularCloudLayouter layouter)
        {
            var rectanglesPoints = ConvexHullBuilder.GetRectanglesPointsSet(layouter.Rectangles);
            var cloudConvexHull = ConvexHullBuilder.GetConvexHull(rectanglesPoints);
            return cloudConvexHull;
        }

        [Test]
        public void PutRectanglesWithousIntersects()
        {
            var rectanglesCount = 100;
            var maxHeight = 35;
            var maxWidth = 70;
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rnd = new Random();

            PutRectangles(layouter, rnd, maxWidth, maxHeight, rectanglesCount);

            DoRectanglesIntersect(layouter).Should().BeFalse();
        }


        private static bool DoRectanglesIntersect(CircularCloudLayouter layouter)
        {
            foreach (var firstRect in layouter.Rectangles)
                foreach (var secondRect in layouter.Rectangles)
                    if (firstRect != secondRect && firstRect.IntersectsWith(secondRect))
                        return true;
            return false;
        }

        private static void PutRectangles(
            CircularCloudLayouter layouter, 
            Random rnd, 
            int maxWidth, 
            int maxHeight,
            int rectanglesCount)
        {
            for (var i = 0; i < rectanglesCount; i++)
            {
                var rect = layouter.PutNextRectangle(new Size(rnd.Next(20, maxWidth), rnd.Next(10, maxHeight)));
            }
        }

        private double GetCloudDeviation(double cloudValue, double deviateFrom)
        {
            return 1 - Math.Abs(cloudValue / deviateFrom);
        }

        private (double, double) GetMinMaxHullVectorsLengths(Point center, IEnumerable<Point> hull)
        {
            var hullVectorsLengths = hull.Select(point => new Vector(center, point).GetLength());
            return (hullVectorsLengths.Min(), hullVectorsLengths.Max());
        }
    }
}
