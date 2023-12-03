using NUnit.Framework;
using FluentAssertions;
using TagsCloudVisualization;
using System.Drawing;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouterTests
    {
        private static Point center = new(10, 10);

        [Test]
        public void Constructor_NotTrows()
        {
            var action = () => new CircularCloudLayouter(center);
            action.Should().NotThrow();
        }

        [TestCase(-1, 1, TestName = "PutNextRectangle_WidthNotPositive_ThrowsArgumentException")]
        [TestCase(1, -1, TestName = "PutNextRectangle_HeightNotPositive_ThrowsArgumentException")]
        public void PutNextRectangle_IncorrectSize_ThrowsArgumentException(int rectangleWidth, int rectangleHeight)
        {
            var layouter = new CircularCloudLayouter(center);
            var action = () => layouter.PutNextRectangle(new Size(rectangleWidth, rectangleHeight));
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_WithCorrectSize_ReturnsCorrectRectangle()
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangle = layouter.PutNextRectangle(new Size(15, 15));
            rectangle.Size.Should().BeEquivalentTo(new Size(15, 15));
        }

        [Test]
        public void PutNextRectangle_FirstRectangle_ReturnsRectangleWithCenterInLayoutCenter()
        {
            var layouter = new CircularCloudLayouter(center);

            var rectangle = layouter.PutNextRectangle(new Size(15, 15));
            var expectedRectangleCenter = new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);

            expectedRectangleCenter.Should().BeEquivalentTo(center);
        }

        [Test]
        public void PutNextRectangle_TwoRectangles_ReturnsSecondRectangleWithCenterNotInLayoutCenter()
        {
            var layouter = new CircularCloudLayouter(center);

            layouter.PutNextRectangle(new Size(15, 15));
            var secondRectangle = layouter.PutNextRectangle(new Size(10, 10));
            var expectedRectangleCenter = new Point(secondRectangle.Left + secondRectangle.Width / 2,
                secondRectangle.Top + secondRectangle.Height / 2);

            expectedRectangleCenter.Should().NotBeEquivalentTo(center);
        }

        [Test]
        public void PutNextRectangle_TwoRectangles_ReturnsTwoNotIntersectedRectangles()
        {
            var layouter = new CircularCloudLayouter(center);

            var firstRectangle = layouter.PutNextRectangle(new Size(15, 15));
            var secondRectangle = layouter.PutNextRectangle(new Size(10, 10));
            var isIntersected = firstRectangle.IntersectsWith(secondRectangle);
            
            isIntersected.Should().BeFalse();
        }

        [TestCase(10, TestName = "PutNextRectangle_10Rectangles_RectanglesWithNoIntersects")]
        [TestCase(100, TestName = "PutNextRectangle_100Rectangles_RectanglesWithNoIntersects")]
        [TestCase(200, TestName = "PutNextRectangle_200Rectangles_RectanglesWithNoIntersects")]
        public void PutNextRectangle_ManyRectangles_RectanglesWithNoIntersects(int rectanglesCount)
        {
            var layouter = new CircularCloudLayouter(center);

            for (var i = 0; i < rectanglesCount; i++)
            {
                layouter.PutNextRectangle(new Size(10, 10));
            }

            HasIntersectedRectangles(layouter.Rectangles).Should().BeFalse();
        }

        private bool HasIntersectedRectangles(IList<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j <  rectangles.Count; j++)
                {
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return true;
                }
            }

            return false;
        }
    }
}