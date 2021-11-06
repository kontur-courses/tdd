using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public void MakeCloudCircleDeviationLessThanTwentyPercents()
        {
            var rectanglesCount = 150;
            var maxHeight = 35;
            var maxWidth = 35;
            var center = new Point(0, 0);
            var layouter = new CircularCloudLayouter(center);
            var rnd = new Random();

            layouter.PutNextRectangle(new Size(rnd.Next(1, maxWidth), rnd.Next(1, maxHeight)));
            for (var i = 0; i < rectanglesCount; i++)
                layouter.PutNextRectangle(new Size(rnd.Next(1, maxWidth), rnd.Next(1, maxHeight)));
            var rectanglesPoints = ConvexHullBuilder.GetRectanglesPointsSet(layouter.Rectangles);
            var cloudConvexHull = ConvexHullBuilder.GetConvexHull(rectanglesPoints);
            var minMaxHullVectorsLengths = GetMinMaxHullVectorsLengths(center, cloudConvexHull);
            var deviation = 1 - (minMaxHullVectorsLengths.Item1 / minMaxHullVectorsLengths.Item2);

            deviation.Should().BeLessOrEqualTo(0.2);
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
