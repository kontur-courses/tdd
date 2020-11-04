using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layout;
        private Point center;
        private Size rectangleSize;
        private int rectangleCount;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            center = new Point(10, 10);
            layout = new CircularCloudLayouter(center);
            rectangleSize = new Size(10, 10);
            rectangles = new List<Rectangle>();
            rectangleCount = 10;
        }

        private void PutRectangles(int rectangleCount = 10)
        {
            this.rectangleCount = rectangleCount;
            for (int i = 0; i < rectangleCount; i++)
                rectangles.Add(layout.PutNextRectangle(rectangleSize));
        }

        [Test]
        public void PutNextRectangleShouldNotIntersect()
        {
            PutRectangles();

            var isIntesect = rectangles
                .Any(rect => rect.IntersectsWith(rectangles.Where(other => other != rect)));

            isIntesect.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangleShouldHaveMinimalLenWhenRectIsFirst()
        {
            var rect = layout.PutNextRectangle(new Size(10, 10));
            rect.Location.Should().Be(center);
        }

        [Test]
        public void PutNextRectangleAllRectangleShouldBeLikeACircle()
        {
            /// Для тестирования на расположение в виде окружности 
            /// Решил использовать следующую логику
            ///     предполагаю, что прямоугольники можно расположить крестом (поэтому rectCountInRadius = rectangleCount / 4)
            ///     определяю количесво прямоугольников которые содержать радиус
            ///     определяю радиус
            /// Проверка происходи на утверждении, что все прямоугольники должны быть вписаны в кадрат со стороной 2 радиуса
            ///     

            PutRectangles();
            var rectCountInRadius = rectangleCount / 4 + 1;
            var radius = Math.Max(rectangleSize.Height, rectangleSize.Width) * rectCountInRadius;
            var circumscribedCenter = new Point(center.X - radius, center.Y - radius);
            var circumscribedRectangle = new Rectangle(circumscribedCenter, new Size(radius * 2, radius * 2));

            var isIntersectAll = rectangles.All(rect => rect.IntersectsWith(circumscribedRectangle));

            isIntersectAll.Should().BeTrue();
        }

        [Test]
        public void PutNextRectangleReturnDifferentRectangles()
        {
            var allRectangles = new List<Rectangle>();
            var differentRectangles = new HashSet<Rectangle>();
            Rectangle rectangle;
            var size = new Size(10, 10);

            for (int i = 0; i < 300; i++)
            {
                rectangle = layout.PutNextRectangle(size);
                allRectangles.Add(rectangle);
                differentRectangles.Add(rectangle);
            }

            allRectangles.Should().HaveCount(differentRectangles.Count);
            allRectangles.Should().BeEquivalentTo(differentRectangles);
        }

        [Test]
        public void PutNextRectangleAllRectanglesAdded()
        {
            PutRectangles(100);

            layout.Should().HaveCount(rectangles.Count);
            layout.Should().BeEquivalentTo(rectangles);
        }

        [Test]
        public void PutNextRectangleAllRectangleShouldBeDense()
        {
            PutRectangles(30);

            foreach (var rectangle in rectangles)
            {
                var vector = new TargetVector(center, rectangle.Location);
                foreach (var delta in vector.GetPartialDelta().Take(3))
                    TryMoveRectangle(rectangle, delta).Should().BeFalse();
            }
        }

        private bool TryMoveRectangle(Rectangle rectangle, Point delta)
        {
            var movedRectangle = rectangle.MoveOnTheDelta(delta);
            return !movedRectangle.IntersectsWith(rectangles);
        }

        [TearDown]
        public void WriteErrorLog()
        {
            var testContext = TestContext.CurrentContext;
            if (!testContext.Result.Outcome.Status.HasFlag(TestStatus.Failed))
                return;
            var visualization = new CircularCloudVisualization(layout);
            visualization.DrawRectanglesOnImage();
            var path = visualization.SaveImage(testContext.Test.Name);

            TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }
}
