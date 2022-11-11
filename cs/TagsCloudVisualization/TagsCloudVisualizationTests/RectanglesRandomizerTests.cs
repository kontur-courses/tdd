using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.TagsCloudVisualizationTests
{
    [TestFixture]
    internal class RectanglesRandomizerTests
    {
        [TestCase(0)]
        [TestCase(-1)]
        public void GetSortedRectangles_ShouldThrowArgumentException_OnNonPositiveCount(int count)
        {
            var action = () => RectanglesRandomizer.GetSortedRectangles(
                10, 10, count);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Sides of the rectangle and rectangle count should not be non-positive");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void GetSortedRectangles_ShouldThrowArgumentException_OnNonPositiveMaxHeight(int maxHeight)
        {
            var action = () => RectanglesRandomizer.GetSortedRectangles(
                10, maxHeight, 10);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Sides of the rectangle and rectangle count should not be non-positive");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void GetSortedRectangles_ShouldThrowArgumentException_OnNonPositiveMaxWidth(int maxWidth)
        {
            var action = () => RectanglesRandomizer.GetSortedRectangles(
                maxWidth, 10, 10);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Sides of the rectangle and rectangle count should not be non-positive");
        }


        [TestCase(1)]
        [TestCase(100)]
        public void GetSortedRectangles_ShouldReturnСountRectangles_WhenCountRectanglesWasRequested(
            int count)
        {
            var rectangles = RectanglesRandomizer.GetSortedRectangles(8, 9, count);
            rectangles.Should().HaveCount(count);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        public void GetSortedRectangles_ShouldReturnRectanglesSortedInDescendingOrder_OnCorrectValues(
            int count)
        {
            var sizeComparer = Comparer<Size>.Create((rect1, rect2) =>
                rect1.Height * rect1.Width > rect2.Height * rect2.Width ? 1 :
                rect1.Height * rect1.Width < rect2.Height * rect2.Width ? -1 : 0);

            var rectangles = RectanglesRandomizer
                .GetSortedRectangles(100, 100, count);

            rectangles.Should().BeInDescendingOrder(sizeComparer);
        }
    }
}