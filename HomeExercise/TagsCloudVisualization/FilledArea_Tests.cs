using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class FilledArea_Tests
    {
        [TestCase(-1, 1, 1, -1, true, TestName = "Second inside first => true")]
        [TestCase(-3, 3, 3, -3, true, TestName = "First inside second => true")]
        [TestCase(2, 1, 4, -1, true, TestName = "Intersects with one edge => true")]
        [TestCase(1, 1, 3, -1, true, TestName = "Intersects with half area => true")]
        [TestCase(1, 3, 3, 1, true, TestName = "Intersects with one corner => true")]
        [TestCase(2, -2, 3, -3, true, TestName = "Have one common point => true")]
        [TestCase(-2, 2, 2, -2, true, TestName = "Have same area => true")]
        [TestCase(3, 1, 4, 0, false, TestName = "Second is outside of first => false")]
        public void IntersectsWith_ReturnsCorrectResult(int topLeftX,
            int topLeftY,
            int bottomRightX,
            int bottomRightY,
            bool expectedResult)
        {
            var rectangle1 = new Rectangle(-2, 2, 4, 4);
            var area1 = new FilledArea(rectangle1);
            var topLeft = new Point(topLeftX, topLeftY);
            var bottomRight = new Point(bottomRightX, bottomRightY);
            var area2 = new FilledArea(topLeft, bottomRight);

            var actualResult = area1.IntersectsWith(area2);
            actualResult.Should().Be(expectedResult);
        }
    }
}