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
    public class CircularCloudLayouter_Test
    {
        private CircularCloudLayouter circularCloudLayouter;

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [Test]
        public void PutNextRectangle_Should_PutRectangleInTheCenter_When_FirstRectangle()
        {
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            rectangle.Location.Should().Be(new Point(-15, -10));
        }

        [TestCaseSource(nameof(DifferentTypesOfTwoRectangles))]
        public void PutNextRectangle_Should_NotIntersectRectangles_When_TwoRectangles(Size firstSize, Size secondSize)
        {
            var firstRectangle = circularCloudLayouter.PutNextRectangle(firstSize);
            var secondRectangle = circularCloudLayouter.PutNextRectangle(secondSize);
            secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
        }
        
        private static IEnumerable<TestCaseData> DifferentTypesOfTwoRectangles
        {
            get
            {
                yield return new TestCaseData(new Size(30, 20), new Size(30, 20)).SetName("have equal size");
                yield return new TestCaseData(new Size(30, 20), new Size(40, 10)).SetName("have different size");
                yield return new TestCaseData(new Size(10, 60), new Size(40, 10)).SetName("have different proportions");
                yield return new TestCaseData(new Size(10, 60), new Size(400, 100)).SetName("have big size difference");
            }
        }

        [Test]
        public void PutNextRectangle_Should_NotIntersect_1000Rectangles()
        {
            for (var i = 0; i < 1000; i++)
                circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            CheckIfAnyRectanglesIntersect(circularCloudLayouter.Rectangles).Should().Be(false);
        }

        [Test]
        public void PutNextRectangle_Should_ThrowArgumentException_When_SizeIsEmpty()
        {
            Following.Code(() => circularCloudLayouter.PutNextRectangle(Size.Empty)).ShouldThrow<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_Should_PutRectanglesDenseEnough()
        {
            for (var i = 0; i < 100; i++)
                circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            var circumscribedCircleArea = GetCircumscribedCircleArea();
            var rectangleAreaSum = GetRectanglesAreaSum(circularCloudLayouter.Rectangles);
            var isRatioSmallEnough = rectangleAreaSum / circumscribedCircleArea >= 0.6;
            isRatioSmallEnough.Should().Be(true);
        }

        [Test]
        public void PutNextRectangle_Should_PutRectanglesInCircleShape()
        {
            for (var i = 0; i < 100; i++)
                            circularCloudLayouter.PutNextRectangle(new Size(30, 20));
            var verticalLength = circularCloudLayouter.GetCloudVerticalLength();
            var horizontalLength = circularCloudLayouter.GetCloudHorizontalLength();
            var ratio = verticalLength / horizontalLength;
            ratio.Should().BeInRange(0.9, 1.1);
        }

        private double GetCircumscribedCircleArea()
        {
            var verticalLength = circularCloudLayouter.GetCloudVerticalLength();
            var horizontalLength = circularCloudLayouter.GetCloudHorizontalLength();
            var radius = horizontalLength > verticalLength ? horizontalLength / 2 : verticalLength / 2;
            return Math.PI * radius * radius;
        }

        private double GetRectanglesAreaSum(IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Select(rect => rect.Width * rect.Height).Sum();
        }

        private bool CheckIfAnyRectanglesIntersect(IEnumerable<Rectangle> rectangles)
        {
            var enumerable = rectangles.ToList();
            return enumerable
                .Select(rectFirst => enumerable
                    .Select(rectSecond => rectFirst.IntersectsWith(rectSecond) && rectFirst != rectSecond)
                    .Any(value => value))
                .Any(value => value);
        }

        [TearDown]
        public void TearDown()
        {
            var currentContext = TestContext.CurrentContext;
            if (currentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var visualizer = new CircularCloudLayouterVisualizer();
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + currentContext.Test.Name +".png";
                Console.WriteLine("Tag cloud visualization saved to file " + path);
                visualizer.SaveImage(circularCloudLayouter.Rectangles, path);
            }
        }
    }

    public static class Following
    {
        public static Action Code(Action action)
        {
            return action;
        }
    }
}