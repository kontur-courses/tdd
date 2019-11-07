using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Drawing;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        public class TestBase
        {
            public CircularCloudLayouter BaseLayouter;

            public Size BasicSize = new Size(5, 2);

            public Point BasicCenter = new Point(50, 50);

            private CircularCloudLayouter CreateBaseLayouter()
            {
                return new CircularCloudLayouter(BasicCenter);
            }

            [SetUp]
            public void BaseSetUp()
            {
                BaseLayouter = CreateBaseLayouter();
            }

            public bool AreIntersecting(List<Rectangle> rectangles)
            {
                var areIntersecting = false;
                for (int i = 0; i < rectangles.Count; ++i)
                    for (int j = 0; j < rectangles.Count; ++j)
                    {
                        var rectangleX = rectangles[i];
                        var rectangleY = rectangles[j];
                        if (i != j && rectangleX.IntersectsWith(rectangleY))
                            areIntersecting = true;
                    }
                return areIntersecting;
            }
        }

        public class ConstructorShould : TestBase
        {
            [TestCase(0, -1)]
            [TestCase(-1, 0)]
            public void ThrowArgumentException_OnInvalidCenter(int x, int y)
            {
                Action create = () => new CircularCloudLayouter(new Point(x, y));
                create.Should().Throw<ArgumentException>().WithMessage("Invalid center");
            }
        }

        public class PutNextRectangleShould : TestBase
        {
            [TestCase(-1, 1)]
            [TestCase(1, -1)]
            [TestCase(0, 1)]
            public void ThrowArgumentException_OnNegativeOrZeroRectangleSize(int height, int width)
            {
                var size = new Size(height, width);
                Action getRectangle = () => BaseLayouter.PutNextRectangle(size);
                getRectangle.Should().Throw<ArgumentException>().WithMessage("Invalid size");
            }

            [Test]
            public void PutFirstRectangleInCenter()
            {
                var rectangle = BaseLayouter.PutNextRectangle(BasicSize);
                rectangle.X.Should().Be(BasicCenter.X - BasicSize.Width / 2);
                rectangle.Y.Should().Be(BasicCenter.Y - BasicSize.Height / 2);
            }

            [TestCase(2, 2)]
            [TestCase(1, 10)]
            [TestCase(6, 3)]
            public void PlaceSameRectanglesWithoutIntersection(int height, int width)
            {
                var size = new Size(height, width);
                var rectangles = new List<Rectangle>();
                for (int i = 0; i < 10; ++i)
                {
                    rectangles.Add(BaseLayouter.PutNextRectangle(size));
                }
                AreIntersecting(rectangles).Should().BeFalse();
            }

            [Test]
            public void PlaceDifferentRectanglesWithoutIntersection()
            {
                var rectangles = new List<Rectangle>();
                var size = new Size(2, 1);
                for (int i = 0; i < 10; ++i)
                {
                    rectangles.Add(BaseLayouter.PutNextRectangle(size));
                    size.Height++;
                    size.Width++;
                }
                AreIntersecting(rectangles).Should().BeFalse();
            }

            [Test]
            public void NotPlaceRectangles_OnNegativeCoordinates()
            {
                var rectangles = new List<Rectangle>();
                var layouter = new CircularCloudLayouter(new Point(0, 0));
                var size = new Size(1, 1);
                for (int i = 0; i < 10; ++i)
                {
                    rectangles.Add(layouter.PutNextRectangle(size));
                }
                rectangles.Any(rectangle => rectangle.X < 0 || rectangle.Y < 0).Should().BeFalse();
            }

            [Test, Timeout(1000)]
            public void PlaceRectanglesFast()
            {
                var size = new Size(1, 1);
                for (int i = 0; i < 10000; ++i)
                    BaseLayouter.PutNextRectangle(size);
            }
        }
    }
}
