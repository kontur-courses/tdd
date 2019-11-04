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
        {
            var rectangle = new Rectangle(point, new Size(width, height));
            rectangle.GetCenter().Should().Be(new Point(x, y));
        }
    }
}
