using FluentAssertions;
using NUnit.Framework;
using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    class RectangleExtension_should
    {
        private static Point point;

        [SetUp]
        public void SetUp()
        {
            point = new Point(4, 4);

        }

        [Test]
        public void GetCenter_WhenRectangleIsPoint_ReturnThisPoint()
        {
            var rectangle = new Rectangle(point, new Size(0, 0));
            rectangle.GetCenter().Should().Be(point);
        }

        [Test]
        public void GetCenter_WhenRectangle_ReturnCenter()
        {
            var rectangle = new Rectangle(point, new Size(10, 10));
            rectangle.GetCenter().Should().Be(new Point(9, 9));
        }

        [TestCase(0, 10, 4, 9,TestName = "GetCenter_WhenRectangleIsVerticalLine_ReturnPointOnThisLine")]
        [TestCase(10, 0, 9, 4, TestName = "GetCenter_WhenRectangleIsHorizontalLine_ReturnPointOnThisLine")]
        public void GetCenter_WhenRectangleIsLine_ReturnPointOnThisLine(int width, int height, int x, int y)
        [TestCase(0, 50, 50, 50, ExpectedResult = false, TestName = "IntersectsWith_WhenRectanglesIntersectsOnEdge_ReturnFalse")]
        [TestCase(50, 50, 50, 50, ExpectedResult = false, TestName = "IntersectsWith_WhenRectanglesIntersectsOnPoint_ReturnFalse")]
        [TestCase(0, 0, 50, 50, ExpectedResult = true, TestName = "IntersectsWith_WhenRectangleEqualOneRectangle_ReturnTrue")]
        [TestCase(-10, -10, 100, 100, ExpectedResult = true, TestName = "IntersectsWith_WhenRectangleContainsOneRectangle_ReturnTrue")]
        [TestCase(10, 10, 10, 10, ExpectedResult = true, TestName = "IntersectsWith_WhenRectangleComesInOneRectangle_ReturnTrue")]
        [TestCase(-150, -150, 50, 50, ExpectedResult = false, TestName = "IntersectsWith_WhenRectangleWithCoordinatesWithOppositeSign_ReturnFalse")]
        [TestCase(90, 90, 10, 40, ExpectedResult = false, TestName = "IntersectsWith_WhenRectanglesDontIntersects_ReturnFalse")]
        public bool IntersectsWithOtherRectangles(int actualX, int actualY, int actualWidth, int actualHeight)
        {
            var rectangle = new Rectangle(actualX, actualY, actualWidth, actualHeight);
            var rectangles = new[] { new Rectangle(0, 0, 50, 50), new Rectangle(150, 150, 50, 50) };
            return rectangle.IntersectsWith(rectangles);
        }
    }
}
