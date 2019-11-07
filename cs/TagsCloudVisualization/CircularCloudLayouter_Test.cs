using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

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
        public void PutNextRectangle_Should_ThrowArgumentException_When_SizeIsEmpty()
        {
            Following.Code(() => circularCloudLayouter.PutNextRectangle(Size.Empty)).ShouldThrow<ArgumentException>();
        }

        private int CalculateRectanglesSquaresSum(IEnumerable<Rectangle> rectangles)
        {
            return rectangles.Select(rect => rect.Height * rect.Width).Sum();
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