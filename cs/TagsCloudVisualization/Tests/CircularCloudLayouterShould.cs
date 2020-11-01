using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        CircularCloudLayouter layouter;
        Point center;
        Size rectangleSize;
        int rectangleCount;
        List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            center = new Point(10, 10);
            layouter = new CircularCloudLayouter(center);
            rectangleSize = new Size(10, 10);
            rectangles = new List<Rectangle>();
            rectangleCount = 10;
        }

        private void PutRectangles()
        {
            for (int i = 0; i < rectangleCount; i++)
                rectangles.Add(layouter.PutNextRectangle(rectangleSize));
        }

        [Test]
        public void PutNextRectangleDontShouldIntersect()
        {
            PutRectangles();
            var isIntesect = rectangles
                .Any(rect => rect.IsIntersectsWith(rectangles.Where(other => other != rect)));
            isIntesect.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangleShouldHaveMinimalLenWhenRectIsFirst()
        {
            var rect = layouter.PutNextRectangle(new Size(10, 10));
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

    }
}
