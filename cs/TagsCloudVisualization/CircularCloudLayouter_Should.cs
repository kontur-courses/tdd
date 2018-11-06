using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private const int DistanceBetweenPoints = 1;

        private CircularCloudLayouter layouter;
        private Point center;
        private Size rectangleSize;
        private IEnumerable<Point> points;

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
                return;
            var fileName = $"{TestContext.CurrentContext.Test.Name}TestLayout.png";
            var path = $"{TestContext.CurrentContext.WorkDirectory}/{fileName}";
            new RectanglesDrawer().GenerateImage(layouter.Rectangles, fileName);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }

        [Test]
        public void Have_ConstructorWithPointAndGeneratorInput()
        {
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);

            Action action = () => new CircularCloudLayouter(center, points);

            action.Should().NotThrow();
        }

        [Test]
        public void Have_PutNextRectangleMethod()
        {
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);
            layouter = new CircularCloudLayouter(center, points);
            rectangleSize = new Size(10, 10);

            Action action = () => layouter.PutNextRectangle(rectangleSize);

            action.Should().NotThrow();
        }

        [Test]
        public void Have_EmptyRectanglesList_AfterCreating()
        {
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);
            layouter = new CircularCloudLayouter(center, points);

            var rectangles = layouter.Rectangles;

            rectangles.Should().BeEmpty();
        }

        private static IEnumerable RectanglesAmountTestCases
        {
            get
            {
                yield return new TestCaseData(1).SetName("OneRectangle");
                yield return new TestCaseData(2).SetName("TwoRectangles");
                yield return new TestCaseData(1000).SetName("ThousandRectangles");
            }
        }

        [TestCaseSource(nameof(RectanglesAmountTestCases))]
        public void Have_AllRectanglesInList_AfterAdding(int rectangleAmount)
        {
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);
            layouter = new CircularCloudLayouter(center, points);
            rectangleSize = new Size(10, 10);

            AddRectangles(rectangleAmount);
            var rectanglesAmount = layouter.Rectangles.Count;

            rectanglesAmount.Should().Be(rectangleAmount);
        }

        [TestCase(0, 1, TestName = "WidthIsZero")]
        [TestCase(1, 0, TestName = "HeightIsZero")]
        [TestCase(-1, 0, TestName = "WidthIsNegative")]
        [TestCase(0, -1, TestName = "HeightIsNegative")]
        [TestCase(0, 0, TestName = "BothDimensionsAreZero")]
        [TestCase(-1, -1, TestName = "BothDimensionsAreNegative")]
        public void PutNextRectangle_ShouldThrowArgumentException_OnInvalidSize(int width, int height)
        {
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);
            layouter = new CircularCloudLayouter(center, points);
            var invalidRectangleSize = new Size(width, height);

            Action action = () => layouter.PutNextRectangle(invalidRectangleSize);

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void FirstRectangle_ShouldBeInCenter()
        {
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);
            layouter = new CircularCloudLayouter(center, points);
            rectangleSize = new Size(10, 10);

            layouter.PutNextRectangle(rectangleSize);
            var isRectangleInCenter = layouter.Rectangles.First().Contains(center);

            isRectangleInCenter.Should().BeTrue();
        }

        [TestCaseSource(nameof(RectanglesAmountTestCases))]
        public void RectanglesWithSameSize_ShouldNotIntersect(int rectanglesAmount)
        {
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);
            layouter = new CircularCloudLayouter(center, points);
            rectangleSize = new Size(10, 10);

            AddRectangles(rectanglesAmount);

            CheckIntersection(layouter.Rectangles);
        }

        [TestCaseSource(nameof(RectanglesAmountTestCases))]
        public void RectanglesWithRandomSize_ShouldNotIntersect(int rectanglesAmount)
        {
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);
            layouter = new CircularCloudLayouter(center, points);
            var random = new Random();

            for (var i = 0; i < rectanglesAmount; i++)
            {
                var width = random.Next(1, 10);
                var height = random.Next(1, 10);
                var size = new Size(width, height);
                layouter.PutNextRectangle(size);
            }

            CheckIntersection(layouter.Rectangles);
        }

        [Test]
        public void FirstRectangleCenter_ShouldBeCloudCenter()
        {
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);
            layouter = new CircularCloudLayouter(center, points);
            rectangleSize = new Size(10, 10);

            layouter.PutNextRectangle(rectangleSize);
            var firstRectangleCenter = layouter.Rectangles.First().GetCenter();

            firstRectangleCenter.Should().Be(center);
        }

        [TestCase(100)]
        [TestCase(200)]
        [TestCase(500)]
        public void DensityShouldBeMoreThanSeventyPercent(int rectanglesAmount)
        {
            const double expectedDensity = 0.7;
            center = new Point(5, 5);
            points = new SpiralPointsGenerator().GetPoints(DistanceBetweenPoints);
            layouter = new CircularCloudLayouter(center, points);

            for (var i = rectanglesAmount; i > 0; i--)
                layouter.PutNextRectangle(new Size(i * 3, i));
            var radius = GetDistanceToFatherPoint(layouter.Rectangles);
            var circleSquare = Math.PI * radius * radius;
            var rectanglesSquare = layouter.Rectangles.Sum(rectangle => rectangle.Width * rectangle.Height);
            var density = rectanglesSquare / circleSquare;

            density.Should().BeGreaterOrEqualTo(expectedDensity);
        }

        private double GetDistanceToFatherPoint(IEnumerable<Rectangle> rectangles)
        {
            var maxDistance = double.MinValue;
            foreach (var rectangle in rectangles)
            {
                foreach (var corner in rectangle.GetCorners())
                {
                    var distance = GetDistance(center, corner);
                    if (distance > maxDistance)
                        maxDistance = distance;
                }
            }
            return maxDistance;
        }

        private double GetDistance(Point firstPoint, Point secondPoint) =>
            Math.Sqrt(Math.Pow(firstPoint.X + secondPoint.X, 2) + Math.Pow(firstPoint.Y + secondPoint.Y, 2));

        private void AddRectangles(int rectanglesAmount)
        {
            for (var i = 0; i < rectanglesAmount; i++)
                layouter.PutNextRectangle(rectangleSize);
        }

        private void CheckIntersection(List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                    RectanglesIntersect(rectangles[i], rectangles[j]).Should().BeFalse();
            }
        }

        private bool RectanglesIntersect(Rectangle firstRectangle, Rectangle secondRectangle)
        {
            return secondRectangle.Left < firstRectangle.Right &&
                   firstRectangle.Left < secondRectangle.Right &&
                   secondRectangle.Top < firstRectangle.Bottom &&
                   firstRectangle.Top < secondRectangle.Bottom;
        }
    }
}