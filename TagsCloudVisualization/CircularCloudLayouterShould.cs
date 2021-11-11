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
            var rectanglesCount = 325;
            var maxHeight = 100;
            var maxWidth = 100;
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rnd = new Random();

            PutRectangles(layouter, rnd, maxWidth, maxHeight, rectanglesCount);
            var rectanglesPoints = ConvexHullBuilder.GetRectanglesPointsSet(layouter.Rectangles);
            var cloudConvexHull = ConvexHullBuilder.GetConvexHull(rectanglesPoints);
            var deviation = GetCloudCircleDeviation(center, cloudConvexHull);

            deviation.Should().BeLessOrEqualTo(0.25);
        }

        [Test]
        public void PutRectanglesWithousIntersects()
        {
            var rectanglesCount = 50;
            var maxHeight = 100;
            var maxWidth = 100;
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
                var rect = layouter.PutNextRectangle(new Size(rnd.Next(20, maxWidth), rnd.Next(20, maxHeight)));
            }
        }

        private double GetCloudCircleDeviation(Point center, IEnumerable<Point> cloudConvexHull)
        {
            var minMaxLengths = GetMinMaxHullVectorsLengths(center, cloudConvexHull);
            var deviation = 1 - Math.Abs(minMaxLengths.Item1 / minMaxLengths.Item2);
            return deviation;
        }

        private (double, double) GetMinMaxHullVectorsLengths(Point center, IEnumerable<Point> hull)
        {
            var minLength = hull
                .Select(p => new Vector(center, p).GetLength())
                .Min();
            var maxLength = hull
                .Select(p => new Vector(center, p).GetLength())
                .Max();
            return (minLength, maxLength);
        }
    }
}
