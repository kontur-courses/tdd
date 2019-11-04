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
        [TestCase(0, 0)]
        [TestCase(10, 10)]
        public void PutNextRectangle_ShouldChooseNextCornerPointCloserToCenter_IfThereIsFreeSpace(int centerX, int centerY)
        {
            var center = new Point(centerX, centerY);
            layouter = new CircularCloudLayouter(center);
            var size = new Size(centerX, centerY);
            var firstRectangleSize = new Size(10, 10);

            layouter.PutNextRectangle(firstRectangleSize);
            var location1 = layouter.PutNextRectangle(new Size(7, 7)).Location;
            var location2 = layouter.PutNextRectangle(new Size(5, 3)).Location;
            var location3 = layouter.PutNextRectangle(new Size(5, 3)).Location;
            var location4 = layouter.PutNextRectangle(new Size(7, 2)).Location;
            var location5 = layouter.PutNextRectangle(new Size(10, 1)).Location;

            location1.Should().Be(new Point(-5, -12) + size);
            location2.Should().Be(new Point(2, -8) + size);
            location3.Should().Be(new Point(-10, -5) + size);
            location4.Should().Be(new Point(-12, -2) + size);
            location5.Should().Be(new Point(-15, 0) + size);
        }

        [Test]
        public void PutNextRectangle_RectanglesShouldBeArrangedAsCircle()
        {
            var rectangleSize = new Size(5, 2);

            for (var i = 0; i < 100; i++)
                layouter.PutNextRectangle(rectangleSize);

            var minTop = layouter.Rectangles.Min(rect => rect.Top);
            var maxBottom = layouter.Rectangles.Max(rect => rect.Bottom);
            var maxRight = layouter.Rectangles.Max(rect => rect.Right);
            var minLeft = layouter.Rectangles.Min(rect => rect.Left);

            var maxRadius = 18;
            minTop.Should().BeGreaterOrEqualTo(-maxRadius);
            maxBottom.Should().BeLessOrEqualTo(maxRadius);
            maxRight.Should().BeLessOrEqualTo(maxRadius);
            minLeft.Should().BeGreaterOrEqualTo(-maxRadius);
        }

        private List<Size> GetRandomSizesList(int minWidth, int maxWidth, int minHeight, int maxHeight, int numberOfSizes)
        {
            var random = new Random();
            var sizes = new List<Size>();

            for (var i = 0; i < numberOfSizes; i++)
            {
                var width = random.Next(minWidth, maxWidth);
                var height = random.Next(minHeight, maxHeight);
                sizes.Add(new Size(width, height));
            }

            return sizes;
        }

        [Test]
        [Repeat(5)]
        public void PutNextRectangle_RectanglesShouldNotIntersect()
        {
            var sizes = GetRandomSizesList(1, 10, 1, 10, 100);

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