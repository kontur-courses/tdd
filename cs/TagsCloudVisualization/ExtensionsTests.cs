using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class ExtensionsTests
    {
        [TestCase(50, 60, 25, 15, TestName = "GetRectangleCenter_Rect[50;60;25;15]")]
        [TestCase(0, 0, 10, 10, TestName = "GetRectangleCenter_Rect[0;0;10;10]")]
        [TestCase(-50, -60, 25, 15, TestName = "GetRectangleCenter_Rect[-50;-60;25;15]")]
        public void GetRectangleCenter_RectParams_ShouldCalcRectCenterCorrectly(int x, int y, int width, int height)
        {
            var actualCenter = new Rectangle(x, y, width, height).GetRectangleCenter();
            var expectedCenter = new Point(x + width / 2, y + height / 2);
            actualCenter.Should().BeEquivalentTo(expectedCenter);
        }

        [TestCase(0, 0, 5, 5, ExpectedResult = 7.071, TestName = "GetDistance_D[0,0;5,5]_Result[7.071]")]
        [TestCase(-10, -12, -3, -4, ExpectedResult = 10.630, TestName = "GetDistance_D[-10,-12;-3,-4]_Result[10.630]")]
        [TestCase(0, 0, 0, 0, ExpectedResult = 0, TestName = "GetDistance_D[0,0;0,0]_Result[0]")]
        [DefaultFloatingPointTolerance(0.001)]
        public double GetDistance_PointsParams_ShouldReturnCorrectDistance(int x1, int y1, int x2, int y2)
        {
            return new Point(x1, y1).GetDistance(new Point(x2, y2));
        }

        [TestCase(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, TestName = "MinBy_Array[1-10:1]_ShouldReturnMin")]
        [TestCase(new[] {10, 1, 1, 1, 10}, TestName = "MinBy_Array[10, 1, 1, 1, 10]_ShouldReturnMin")]
        [TestCase(new[] {10, 1, -1, 1, 10}, TestName = "MinBy_Array[10, 1, -1, 1, 10]_ShouldReturnMin")]
        public void MinBy_IntArray_ShouldReturnMin(int[] array)
        {
            array.MinBy(item => item).Should().Be(array.Min());
        }
    }
}