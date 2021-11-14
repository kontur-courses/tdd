using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudTests
{
    [TestFixture]
    public class ExtensionsTests
    {
        private Utils utils;

        [SetUp]
        public void SetUp()
        {
            utils = new Utils();
        }
        
        [TestCase(0, 2, 0, 4, TestName = "Point{0,2} Point{0,4}", ExpectedResult = 2)]
        [TestCase(5, 3, 6, 1, TestName = "Point{5,3} Point{6,1}", ExpectedResult = 2.23)]
        [TestCase(1, 1, 1, 1, TestName = "Point{1,1} Point{1,1}", ExpectedResult = 0)]
        [DefaultFloatingPointTolerance(0.01)]
        public double Point_GetDistanceTo_ShouldReturnCorrectDistance(int srcX, int srcY, int destX, int destY)
        {
            var point = new Point(srcX, srcY);

            return point.DistanceTo(new Point(destX, destY));
        }


        [TestCaseSource(typeof(ExtensionsTestData), nameof(ExtensionsTestData.RectangleGetCenterTestCaseData))]
        public void Rectangle_GetCenter_ShouldReturnCorrectCenter(int x, int y, int width, int height, Point expected)
        {
            var rectangle = new Rectangle(x, y, width, height);

            var actualCenter = rectangle.GetCenter();

            actualCenter.Should().Be(expected);
        }

        [TestCaseSource(typeof(ExtensionsTestData), nameof(ExtensionsTestData.RectanglesGetMinCanvasSizeData))]
        public void RectangleCollection_GetMinCanvasSize_ShouldReturnCorrectMinCanvasSize(int count,
            int widthMultiplier, int heightMultiplier, Size expectedSize)
        {
            var rectanglesFactory = new Func<int, Rectangle>(x => new Rectangle(x, x, x * widthMultiplier, x * heightMultiplier));
            var rectangles = utils.GetRectangles(count, rectanglesFactory);

            var actual = rectangles.GetMinCanvasSize();

            actual.Should().Be(expectedSize);
        }
        
        [Test]
        public void RectanglesCollection_ContainsIntersectingRectangles_ShouldBeTrue_WhenContains()
        {
            var rectanglesFactory = new Func<int, Rectangle>(x => new Rectangle(x, x, x, x));
            
            var actual = utils.GetRectangles(100, rectanglesFactory).ContainsIntersectingRectangles();
            
            actual.Should().BeTrue();
        }
        
        [Test]
        public void RectanglesCollection_ContainsIntersectingRectangles_ShouldBeFalse_WhenNotContains()
        {
            var rectanglesFactory = new Func<int, Rectangle>(x => new Rectangle(x * 2, x * 2, x, x));
            
            var actual = utils.GetRectangles(10, rectanglesFactory).ContainsIntersectingRectangles();
            
            actual.Should().BeTrue();
        }
    }
    
    public class ExtensionsTestData
    {
        public static IEnumerable<TestCaseData> RectangleGetCenterTestCaseData()
        {
            yield return new TestCaseData(100, 33, 90, 3, new Point(145, 34)).SetName("Rectangle{X=100,Y=33,Width=90,Height=3}");
            yield return new TestCaseData(43, 46, 57, 13, new Point(71, 52)).SetName("Rectangle{X=43,Y=46,Width=57,Height=13}");
            yield return new TestCaseData(3, 4, 4, 6, new Point(5,  7)).SetName("Rectangle{X=3,Y=4,Width=4,Height=7}");
        }
        
        public static IEnumerable<TestCaseData> RectanglesGetMinCanvasSizeData()
        {
            yield return new TestCaseData(100, 2, 3, new Size(299, 399));
            yield return new TestCaseData(3, 3, 2, new Size(11, 8));
        }
    }
}