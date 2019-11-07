using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private readonly Point defaultCenter = new Point(0, 0);

        [SetUp]
        public void CreateCircularCloudLayouter()
        {
            layouter = new CircularCloudLayouter(defaultCenter);
        }

        [Test]
        public void PutNextRectangle_ShouldPutRectangleWithSameSizeAsInArgument_IfFirstRectangle()
        {
            var rectangleSize = new Size(10, 10);

            layouter.PutNextRectangle(rectangleSize);
            var rectangle = layouter.Rectangles[0];
            var size = rectangle.Size;

            size.Should().Be(rectangleSize);
        }

        /*
            Я хотел написать в TestCase названия тестовых случаев,
            например в данном случае TestName = "IfSizesAreEven" и TestName = "IfSizesAreOdd", 
            но почему то если их прописать, то тест перестает выполняться, хотя в прошлой домашке все работало хорошо
            Не знаю в чем может быть проблема. Может эта ошибка связана с проектом. Но я пока убрал названия.
            Такая же проблема и в PointsRadiusComparerTests.
         */
        [Test]
        [TestCase(10, 10)]
        [TestCase(9, 9)]
        public void PutNextRectangle_ShouldPutRectangleInCenter_IfFirstRectangle(int width, int height)
        {
            var rectangleSize = new Size(width, height);

            var rectangle = layouter.PutNextRectangle(rectangleSize);
            var location = rectangle.Location;

            var expectedLocation = new Point(-width / 2, -height / 2);
            location.Should().Be(expectedLocation);
        }

        [Test]
        public void PutNextRectangle_RectanglesShouldBeArrangedAsCircle()
        {
            var sizes = Generator.GetRandomSizesList(1, 10, 1, 10, 500, new Random(100));
            var rectangelsArea = 0;

            foreach (var size in sizes)
            {
                var rectangle = layouter.PutNextRectangle(size);
                rectangelsArea += rectangle.Width * rectangle.Height;
            }

            var squaredMaxRadius = (int)Math.Ceiling(rectangelsArea / Math.PI);
            squaredMaxRadius += squaredMaxRadius / 4;
            var corners = layouter.Rectangles
                .SelectMany(rectangle => rectangle.GetCorners()).ToList();
            foreach (var corner in corners)
                corner.SquaredDistanceTo(layouter.Center).Should().BeLessThan(squaredMaxRadius);
        }

        [Test]
        [Repeat(5)]
        public void PutNextRectangle_RectanglesShouldNotIntersect()
        {
            var sizes = Generator.GetRandomSizesList(1, 10, 1, 10, 100, new Random());

            foreach (var size in sizes)
                layouter.PutNextRectangle(size);

            var rectangles = layouter.Rectangles;
            for (var i = 0; i < rectangles.Count; i++)
            {
                for (var j = 0; j < rectangles.Count; j++)
                {
                    if (i == j)
                        continue;
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }
    }
}