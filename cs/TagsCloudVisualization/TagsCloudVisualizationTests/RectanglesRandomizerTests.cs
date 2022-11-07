using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.TagsCloudVisualizationTests
{
    [TestFixture]
    internal class RectanglesRandomizerTests
    {
        [TestCase(0, 1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 0)]
        [TestCase(-1, 1, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(1, 1, -1)]

        public void GetSortedRectangles_ShouldThrowArgumentException_OnAnyNonPositiveValue(
            int maxHeight, int maxWidth, int count)
        {
            Assert.Throws<ArgumentException>(() => RectanglesRandomizer.GetSortedRectangles(
                maxHeight, maxWidth, count));
        }

        [TestCase(1, 1, 1)]
        [TestCase(1, 1, 2)]
        [TestCase(1, 1, 10)]
        [TestCase(1, 1, 100)]

        public void GetSortedRectangles_ShouldReturnСountRectangles_WhenCountRectanglesWasRequested(
            int maxWidth, int maxHeight, int count)
        {
            var rectangles = RectanglesRandomizer.GetSortedRectangles(maxWidth, maxHeight, count);
            rectangles.Should().HaveCount(count);
        }

        [TestCase(100, 100, 1)]
        [TestCase(100, 100, 2)]
        [TestCase(100, 100, 10)]
        public void GetSortedRectangles_ShouldReturnRectanglesSortedInDescendingOrder_OnCorrectValues(
            int maxWidth, int maxHeight, int count)
        {
            var sizeComparer = Comparer<Size>.Create((rect1, rect2) =>
                rect1.Height * rect1.Width > rect2.Height * rect2.Width ? 1 :
                rect1.Height * rect1.Width < rect2.Height * rect2.Width ? -1 : 0);

            var rectangles = RectanglesRandomizer.GetSortedRectangles(maxWidth, maxHeight, count);

            rectangles.Should().BeInDescendingOrder(sizeComparer);
        }
    }
}