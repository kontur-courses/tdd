using System;
using System.Collections.Generic;
using NUnit.Framework;
using TagsCloudVisualization;
using System.Drawing;
using System.Linq;
using FluentAssertions;

namespace TagCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter sut;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(100, 100);
            sut = new CircularCloudLayouter(center);
        }

        [TestCase(0, 0, TestName = "WithZeroPoint")]
        [TestCase(100, 100, TestName = "WithNonZeroPoint")]
        public void CtorShouldHaveCenter(int x, int y)
        {
            var layouter = new CircularCloudLayouter(new Point(x, y));

            layouter.Center.Should().Be(new Point(x, y));
        }

        [TestCaseSource(nameof(CasesForPutNextRectangles))]
        public void PutNextRectangles(int rectanglesNumber, Point expectedLocation)
        {
            for (var i = 0; i < rectanglesNumber; i++)
                sut.PutNextRectangle(new Size(50, 50));
            var rectangle = sut.PutNextRectangle(new Size(50, 50));
            rectangle.Location.Should().Be(expectedLocation);
        }

        private static IEnumerable<TestCaseData> CasesForPutNextRectangles
        {
            get
            {
                yield return new TestCaseData(0, new Point(75, 75))
                    .SetName("FirstRectangleShouldBeCenter");
                yield return new TestCaseData(1, new Point(75, 25))
                    .SetName("SecondRectangleShouldBeOverFirst");
                yield return new TestCaseData(4, new Point(25, 75))
                    .SetName("RectangleShouldGoClockwise");
            }
        }

        [TestCaseSource(nameof(CasesForPutNextRectangleThrows))]
        public void PutNextRectangle_ShouldThrows(Size rectangleSize)
        {
            Assert.Throws<ArgumentException>(() => sut.PutNextRectangle(rectangleSize));
        }

        private static IEnumerable<TestCaseData> CasesForPutNextRectangleThrows
        {
            get
            {
                yield return new TestCaseData(new Size(0, 1))
                    .SetName("WhenSizeWidthZero");
                yield return new TestCaseData(new Size(1, 0))
                    .SetName("WhenSizeHeightZero");
                yield return new TestCaseData(new Size(0, 0))
                    .SetName("WhenSizeWidthAndHeightZero");
                yield return new TestCaseData(new Size(-1, 1))
                    .SetName("WhenNegativeSizeWidth");
                yield return new TestCaseData(new Size(1, -1))
                    .SetName("WhenNegativeSizeHeight");
                yield return new TestCaseData(new Size(-1, -1))
                    .SetName("WhenNegativeSizeWidthAndHeight");
            }
        }

        [Test]
        public void PutNextRectangle_ShouldNotIntersectWithOther()
        {
            var rectangles = new List<Rectangle>();

            for (var i = 0; i < 50; i++)
                rectangles.Add(sut.PutNextRectangle(new Size(10, 10)));
            var rectanglesCopy = rectangles.ToArray();

            rectangles.Count(rect => rectanglesCopy
                .All(r => r.IntersectsWith(rect)))
                .Should().Be(0);
        }

        [Test]
        public void PutNextRectangle_ShouldBeDense()
        {
            var rectangles = new Rectangle[100];

            for (var i = 0; i < 100; i++)
                rectangles[i] = sut.PutNextRectangle(new Size(10, 10));

            CalculateDensityRatio(rectangles).Should().BeGreaterOrEqualTo(0.5);
        }

        private double CalculateDensityRatio(Rectangle[] rectangles)
        {
            var circleRadius = GetCircumscribeCircleRadius(rectangles);
            var rectanglesSquare = rectangles.Sum(r => r.Width * r.Height);
            var circleSquare = Math.PI * circleRadius * circleRadius;
            return rectanglesSquare / circleSquare;
        }

        private double GetCircumscribeCircleRadius(Rectangle[] rectangles)
        {
            return rectangles.SelectMany(rect => GetRectangleCorners(rect))
                .Max(current =>
                    DistanceBetween(center, current));
        }

        private double DistanceBetween(Point first, Point second)
        {
            var dx = first.X - second.X;
            var dy = first.Y - second.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private IEnumerable<Point> GetRectangleCorners(Rectangle rectangle)
        {
            yield return new Point(rectangle.Left, rectangle.Top);
            yield return new Point(rectangle.Left, rectangle.Bottom);
            yield return new Point(rectangle.Right, rectangle.Top);
            yield return new Point(rectangle.Right, rectangle.Bottom);
        }
    }
}
