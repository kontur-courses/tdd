using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterConstructor_Should
    {
        [TestCase(0, 10, TestName = "Center x is zero")]
        [TestCase(10, 0, TestName = "Center y is zero")]
        [TestCase(-1, 10, TestName = "Center x is negative")]
        [TestCase(10, -1, TestName = "Center y is negative")]
        public void ThrowArgumentException_When(int x, int y)
        {
            Action action = () => new CircularCloudLayouter(new Point(x, y));

            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CreateNewInstance_WhenCenterCoordinatesArePositive()
        {
            Action action = () => new CircularCloudLayouter(new Point(150, 250));

            action.Should().NotThrow<Exception>();
        }
    }

    [TestFixture]
    public class CircularCloudLayouterPutNextRectangle_Should
    {
        [SetUp]
        public void Init()
        {
            layouterCenter = new Point(500, 500);
            cloudLayouter = new CircularCloudLayouter(layouterCenter);
            layouterRectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            var testContext = TestContext.CurrentContext;
            if (testContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var filename = $"{AppDomain.CurrentDomain.BaseDirectory} {testContext.Test.Name} Failed.png";
                CloudVisualizator.Visualize(VisualizatorTheme.Theme.Red, layouterRectangles).Save(filename);
                Console.WriteLine($"Tag cloud visualization saved to file {filename}");
            }
        }

        private Point layouterCenter;
        private CircularCloudLayouter cloudLayouter;
        private List<Rectangle> layouterRectangles;

        [TestCase(0, 10, TestName = "Width x is zero")]
        [TestCase(10, 0, TestName = "Height y is zero")]
        [TestCase(-1, 10, TestName = "Width x is negative")]
        [TestCase(10, -1, TestName = "Height y is negative")]
        public void ThrowArgumentException_When(int width, int height)
        {
            Action action = () => cloudLayouter.PutNextRectangle(new Size(width, height));

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(50, 40, 40, 2048, TestName = "Rectangles are squares")]
        [TestCase(50, 50, 70, 1000, TestName = "Rectangles are big (50 to 70)")]
        [TestCase(50, 30, 50, 42, TestName = "Rectangles are medium (30 to 50)")]
        [TestCase(50, 10, 30, 777, TestName = "Rectangles are small (10 to 30)")]
        [TestCase(1000, 70, 100, 555555, TestName = "Rectangles count is big (1000)")]
        [TestCase(10, 10, 20, -555, TestName = "Rectangles count is small (10)")]
        public void AddMultipleRectangles_That_FormACircleLikeShape_When(int count, int minSize, int maxSize,
            int randomSeed)
        {
            var acceptableRatio = 50;
            var random = new Random(randomSeed);
            var furthestDistance = 0d;
            var rectanglesSquare = 0d;

            for (var i = 0; i < count; i++)
            {
                var size = new Size(random.Next(minSize, maxSize), random.Next(minSize, maxSize));
                var rectangle = cloudLayouter.PutNextRectangle(size);
                layouterRectangles.Add(rectangle);
            }

            foreach (var rectangle in layouterRectangles)
            {
                var distance = GetDistanceBetweenRectangleAndPoint(rectangle, layouterCenter);
                if (distance > furthestDistance)
                    furthestDistance = distance;
                rectanglesSquare += rectangle.Width * rectangle.Height;
            }

            var circleSquare = furthestDistance * furthestDistance * Math.PI;
            var squareRatio = rectanglesSquare / circleSquare * 100;
            squareRatio.Should().BeGreaterOrEqualTo(acceptableRatio);
        }

        private static double GetDistanceBetweenRectangleAndPoint(Rectangle rectangle, Point point)
        {
            var rectangleCentre = new Point(rectangle.Location.X + rectangle.Width / 2,
                rectangle.Location.Y + rectangle.Height / 2);

            return Math.Sqrt(Math.Pow(rectangleCentre.X - point.X, 2) + Math.Pow(rectangleCentre.Y - point.Y, 2));
        }

        [Test]
        public void AddFirstRectangleInTheCloudCenter()
        {
            var addedRectangle = cloudLayouter.PutNextRectangle(new Size(100, 200));

            layouterRectangles = new List<Rectangle> {addedRectangle};

            addedRectangle.Location.Should().BeEquivalentTo(layouterCenter);
        }

        [Test]
        public void AddMultipleRectangles_That_DontIntersectWithEachOther()
        {
            var rectangles = new List<Rectangle>
            {
                cloudLayouter.PutNextRectangle(new Size(100, 200)),
                cloudLayouter.PutNextRectangle(new Size(130, 250)),
                cloudLayouter.PutNextRectangle(new Size(210, 160)),
                cloudLayouter.PutNextRectangle(new Size(120, 115))
            };

            rectangles.Any(r1 => rectangles.Any(r2 => r1.IntersectsWith(r2) && r1 != r2)).Should().BeFalse();
        }

        [Test]
        public void AddNextRectangle_That_DoesntIntersectWithFirst()
        {
            var firstRectangle = cloudLayouter.PutNextRectangle(new Size(100, 200));
            var secondRectangle = cloudLayouter.PutNextRectangle(new Size(50, 100));

            layouterRectangles = new List<Rectangle> {firstRectangle, secondRectangle};

            secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
        }

        [Test]
        public void NotChangeRectangleSize()
        {
            var addedRectangle = cloudLayouter.PutNextRectangle(new Size(100, 200));

            layouterRectangles = new List<Rectangle> {addedRectangle};

            addedRectangle.Size.Should().BeEquivalentTo(new Size(100, 200));
        }

        [Test]
        public void PlaceTwoRectanglesCloseToEachOther()
        {
            var acceptableXAxisShift = 50;
            var firstRectangle = cloudLayouter.PutNextRectangle(new Size(100, 100));
            var secondRectangle = cloudLayouter.PutNextRectangle(new Size(20, 102));

            layouterRectangles = new List<Rectangle> {firstRectangle, secondRectangle};

            secondRectangle.Y.Should().Be(firstRectangle.Top);
            secondRectangle.X.Should().BeInRange(firstRectangle.Left - acceptableXAxisShift,
                firstRectangle.Right + acceptableXAxisShift);
        }
    }
}