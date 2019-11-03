using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouterTests
    {
        CircularCloudLayouter CircularCloudLayouter;

        [SetUp]
        public void SetUp()
        {
            var center = new Point(1, 3);
            CircularCloudLayouter = new CircularCloudLayouter(center);
        }

        public List<(Size size, Point location)> SizeGenerator() => new List<(Size size, Point location)>
        {
            (new Size(2, 4), new Point(1, 3)),
            (new Size(3, 4), new Point(-2, 3)),
            (new Size(2, 2), new Point(-1, 1)),
            (new Size(3, 3), new Point(1, 0)),
            (new Size(1, 1), new Point(3, 3)),
            (new Size(2, 2), new Point(-1, -1)),
            (new Size(3, 2), new Point(-4, 1)),
        };

        [Test]
        [Description("Проверка на то, что первый элемент установлен в центре")]
        public void Should_ReturnCenterPoint_When_FirstRectangleAdded()
        {
            var size = new Size(2, 4);
            CircularCloudLayouter.PutNextRectangle(size).Location.Should().Be(new Point(1, 3));
        }

        [Test]
        [Description("Проверка на то, что первые 4 прямоугольника будут соприкосяться в центре")]
        public void Should_ReturnPositionClosestToCenter()
        {
            var sizesWithLocations = SizeGenerator();
            sizesWithLocations
                .Take(4)
                .ToList()
                .ForEach(b => CircularCloudLayouter.PutNextRectangle(b.size).Location
                                                                              .Should()
                                                                              .Be(b.location));
        }

        [TestCase(4)]
        [TestCase(10)]
        [TestCase(15)]
        [Description("Проверка на то, что прямоугольники не пересекаются")]
        public void RectanglesShouldNotIntersect(int rectanglesCount)
        {
            var random = new Random();
            var rectangles = new List<Rectangle>();
            var sizesWithLocations = Enumerable
                                            .Range(0, rectanglesCount)
                                            .Select(size => new Size(random.Next(0, 100), random.Next(0, 100)))
                                            .ToList();
            sizesWithLocations.ForEach(size => rectangles.Add(CircularCloudLayouter.PutNextRectangle(size)));

            rectangles
                .ForEach(rec1 => rectangles
                                .Where(rec2 => rec2 != rec1)
                                .ToList()
                                .ForEach(rec2 => rec2.IntersectsWith(rec1).Should().Be(false)));
        }

        [Test]
        [Description("Проверка на то, что остальные элементы ставятся на ближайшее к центру место")]
        public void Should_ReturnPositionClosestToCenter_When_CanNotBeConnectedWithCenter()
        {
            var sizesWithLocations = SizeGenerator();
            sizesWithLocations
                .ForEach(b => CircularCloudLayouter.PutNextRectangle(b.size).Location
                                                                              .Should()
                                                                              .Be(b.location));
        }
    }

    [TestFixture]
    class RectangleGeometryTest
    {
        RectangleGeometry rectangleGeometry;

        [SetUp]
        public void SetUp()
        {
            rectangleGeometry = new RectangleGeometry();
        }

        [TestCase(1, 1, 1, 1)]
        [TestCase(3, 2, 1, 4)]
        [TestCase(1, 23, 434, 23)]
        public void GetCornerRectangles_Should_ReturnRightRectangles(int X, int Y, int width, int height)
        {
            var expectedRec = new List<Rectangle>()
            {
                new Rectangle(X, Y, width, height),
                new Rectangle(X - width, Y, width, height),
                new Rectangle(X, Y - height, width, height),
                new Rectangle(X - width, Y - height, width, height)
            };
            rectangleGeometry
                .GetCornerRectangles(new Size(width, height), new HashSet<Point>() { new Point(X, Y) })
                .ToList()
                .Should()
                .OnlyHaveUniqueItems()
                .And
                .BeEquivalentTo(expectedRec);
        }

        [TestCase(1, 2, 3, 4)]
        [TestCase(12, 24, 32, 4)]
        [TestCase(1, 21, 3, 432)]
        [TestCase(12, 2, 33, 14)]

        public void GetRectangleCorners_Should_ReturnRightRectangles(int X, int Y, int width, int height)
        {
            var expectedPoints = new List<Point>()
            {
                new Point(X, Y),
                new Point(X + width, Y),
                new Point(X, Y + height),
                new Point(X + width, Y + height)
            };
            rectangleGeometry.GetConers(new Rectangle(X, Y, width, height))
                .ToList()
                .Should()
                .OnlyHaveUniqueItems()
                .And
                .BeEquivalentTo(expectedPoints);
        }


        [TestCase(1, 0, 1, 0, 0)]
        [TestCase(1, 0, 1, 1, 1)]
        [TestCase(6, 0, 0, 8, 10)]
        public void GetDistance_Should_ReturnRightDestance(int X1, int Y1, int X2, int Y2, double distance)
        {
            rectangleGeometry.GetDistanceBetweenpPoints(new Point(X1, Y1), new Point(X2, Y2))
                .Should()
                .Be(distance);
        }

        [Test]
        public void CheckCircleForm_Should_ReturnRectangleCenter_When_OneRectangleAdded()
        {
            var rectangles = new List<Rectangle>();
            var centre = new Point(0, 0);
            var tagCloud = new CircularCloudLayouter(centre);
            rectangles.Add(tagCloud.PutNextRectangle(new Size(2, 2)));
            rectangleGeometry.CheckCircleForm(rectangles, centre).Should().Be(new PointF(1, 1));
        }

        public void CheckCircleForm_ShouldReturn_RightVector()
        {
            var rectangles = new List<Rectangle>();
            var centre = new Point(0, 0);
            var tagCloud = new CircularCloudLayouter(centre);
            rectangles.Add(tagCloud.PutNextRectangle(new Size(2, 2)));
            rectangles.Add(tagCloud.PutNextRectangle(new Size(2, 2)));
            rectangles.Add(tagCloud.PutNextRectangle(new Size(2, 2)));
            rectangles.Add(tagCloud.PutNextRectangle(new Size(2, 2)));
            rectangleGeometry.CheckCircleForm(rectangles, centre).Should().Be(new PointF(0, 0));
        }
    }
}
