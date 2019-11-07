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

        [TestCase(0, 0, 4, 4, TestName = "GetCenter_WhenRectangleIsPoint_ReturnThisPoint")]
        [TestCase(10, 10, 9, 9, TestName = "GetCenter_WhenRectangleWithEvenSize_ReturnCorrectCenter")]
        [TestCase(-10, -10, -1, -1, TestName = "GetCenter_WhenRectangleWithEvenNegativeSize_ReturnCenterAsIfRectangleIsInverted")]
        [TestCase(-11, -11, -1, -1, TestName = "GetCenter_WhenRectangleWithOddNegativeSize_ReturnCenterAsIfRectangleIsInverted")]
        [TestCase(-11, 11, -1, 9, TestName = "GetCenter_WhenRectangleWithNegativeWidth_ReturnCenterAsIfRectangleIsInverted")]
        [TestCase(11, -11, 9, -1, TestName = "GetCenter_WhenRectangleWithNegativeHeight_ReturnCenterAsIfRectangleIsInverted")]
        [TestCase(11, 11, 9, 9, TestName = "GetCenter_WhenRectangleWithOddSize_ReturnCenterWithRoundingDownCoordinates")]
        [TestCase(11, 10, 9, 9, TestName = "GetCenter_WhenRectangleWithOddWidth_ReturnCenterWithRoundingDownCoordinates")]
        [TestCase(10, 11, 9, 9, TestName = "GetCenter_WhenRectangleWithOddHeight_ReturnCenterWithRoundingDownCoordinates")]
        [TestCase(0, 10, 4, 9, TestName = "GetCenter_WhenRectangleIsVerticalLine_ReturnPointOnThisLine")]
        [TestCase(10, 0, 9, 4, TestName = "GetCenter_WhenRectangleIsHorizontalLine_ReturnPointOnThisLine")]
        public void GetCenter_WhenRectangleIsLine_ReturnPointOnThisLine(int actualWidth, int actualHeight, int centerExpectedX, int centerExpectedY)
        {
            var rectangle = new Rectangle(point, new Size(actualWidth, actualHeight));
            rectangle.GetCenter().Should().Be(new Point(centerExpectedX, centerExpectedY));
        }

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
