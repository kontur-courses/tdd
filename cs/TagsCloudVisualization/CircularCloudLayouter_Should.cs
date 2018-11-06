using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter();
        }

        [TestCase(1, 0, TestName = "width is positive, height is zero")]
        [TestCase(0, 1, TestName = "width is zero, height is positive")]
        [TestCase(0, 0, TestName = "width is zero, height is zero")]
        [TestCase(-1, 1, TestName = "width is negative, height is positive")]
        [TestCase(1, -1, TestName = "width is positive, height is negative")]
        [TestCase(-1, -1, TestName = "width is negative, height is negative")]
        public void PutNextRectangle_WhenSizeValuesLessOrEqualZero_ThrowsArgumentException(int sizeWidth,
            int sizeHeight)
        {
            var size = new Size(sizeWidth, sizeHeight);
            Action act = () => layouter.PutNextRectangle(size);
            act.ShouldThrow<ArgumentException>()
                .WithMessage($"rectangleSize must have only positive values: {size}");
        }

        [Test]
        public void PutNextRectangle_WhenSizeValuesArePositive_DoesNotThrowsArgumentException()
        {
            var size = new Size(1, 1);
            Action act = () => layouter.PutNextRectangle(size);
            act.ShouldNotThrow<ArgumentException>();
        }

        [TestCase(10, 2)]
        [TestCase(2, 10)]
        [TestCase(2, 2)]
        public void PutNextRectangle_WhenSizeIsCorrect_SizeValuesAreEqualReturnedRectangleValues(int sizeWidth,
            int sizeHeight)
        {
            var size = new Size(sizeWidth, sizeHeight);
            var rect = layouter.PutNextRectangle(size);
            rect.Width.Should()
                .Be(size.Width);
            rect.Height.Should()
                .Be(sizeHeight);
        }

        [TestCaseSource(nameof(RectanglesAreIntersectedTestCases))]
        public void PutNextRectangle_WhenPuttedTwoRectangles_RectanglesAreNotIntersected(Size[] rectangleSizes)
        {
            var rectangles = rectangleSizes.Select(size => layouter.PutNextRectangle(size))
                .ToList();

            Assert
                .IsFalse(rectangles[0].IntersectsWith(rectangles[1]), $"failed on size1: {rectangleSizes[0]}, size2:{rectangleSizes[1]}");
        }

        private static IEnumerable RectanglesAreIntersectedTestCases
        {
            get
            {
                var random = new Random();

                for (int i = 0; i < 100; i++)
                {
                    yield return new TestCaseData(new[]
                    {
                        new Size(random.Next(100) + 1, random.Next(100) + 1),
                        new Size(random.Next(100) + 1, random.Next(100) + 1)
                    });
                }
            }
        }
    }
}