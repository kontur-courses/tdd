using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    public class BruteForceNearestFinderTests
    {
        private BruteForceNearestFinder finder;
        [SetUp]
        public void SetUp()
        {
            finder = new BruteForceNearestFinder();
        }
        [Test]
        public void FindNearest_ShouldReturnNull_OnEmptyRectangles()
        {
            var rectangleForFind = new Rectangle(new Point(5, 7), new Size(4, 2));

            finder.FindNearestByDirection(rectangleForFind, Direction.Top, []).Should().BeNull();
        }

        [TestCase(4, 10, Direction.Top)]
        [TestCase(2, 7, Direction.Top, true)]
        [TestCase(2, 7, Direction.Right)]
        [TestCase(0, 0, Direction.Right, true)]
        [TestCase(0, 0, Direction.Bottom, true)]
        [TestCase(7, 4, Direction.Bottom)]
        [TestCase(10, 11, Direction.Left)]
        [TestCase(7, 4, Direction.Left, true)]
        public void FindNearest_ShouldReturnNearestRectangleByDirection_ForArgumentRectangle(int x, int y, Direction direction, bool isFirstNearest = false)
        {
            var addedRectangle1 = new Rectangle(new Point(2, 2), new Size(3, 4));
            var addedRectangle2 = new Rectangle(new Point(5, 7), new Size(4, 2));
            var rectangleForFind = new Rectangle(new Point(x, y), new Size(2, 1));
            var rectangles = new List<Rectangle> { addedRectangle1, addedRectangle2 };

            var nearest = finder.FindNearestByDirection(rectangleForFind, direction, rectangles);

            nearest.Should().Be(isFirstNearest ? addedRectangle1 : addedRectangle2);
        }
    }
}
