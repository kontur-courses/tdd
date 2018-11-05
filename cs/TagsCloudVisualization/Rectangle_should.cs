using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    [TestFixture]
    class Rectangle_should
    {
        [Test]
        public void ReturnFalse_WhenRectanglesAreNotIntersect()
        {
            var size = new Size(10, 10);
            var rect1 = new Rectangle(new Point(0, 0), size);
            var rect2 = new Rectangle(new Point(20, 0), size);

            var result = rect1.Intersects(rect2);

            result.Should().Be(false);
        }

        [Test]
        public void ReturnTrue_WhenRectanglesAreIntersect()
        {
            var size = new Size(10, 10);
            var rect1 = new Rectangle(new Point(0, 0), size);
            var rect2 = new Rectangle(new Point(5, 0), size);

            var result = rect1.Intersects(rect2);

            result.Should().Be(true);
        }
         
        [TestCase(0, 0, 10, 10, new []{Quarter.BottomLeft, Quarter.BottomRight, Quarter.TopLeft, Quarter.TopRight}, TestName = "all quarters when all quarters are connected")]
        [TestCase(0, 20, 10, 10, new []{Quarter.TopLeft, Quarter.TopRight}, TestName = "top left and right when its quarters are connected")]
        [TestCase(0, -20, 10, 10, new []{Quarter.BottomLeft, Quarter.BottomRight}, TestName = "bottom left and right when its quarters are connected")]
        [TestCase(20, 0, 10, 10, new []{Quarter.TopRight, Quarter.BottomRight}, TestName = "top right and bottom right when its quarters are connected")]
        [TestCase(-20, 0, 10, 10, new []{Quarter.TopLeft, Quarter.BottomLeft}, TestName = "top left and bottom left when its quarters are connected")]
        [TestCase(20, 20, 10, 10, new []{Quarter.TopRight}, TestName = "top right quarter when its is connected")]
        [TestCase(-20, 20, 10, 10, new []{Quarter.TopLeft}, TestName = "top left quarter when its is connected")]
        [TestCase(-20, -20, 10, 10, new []{Quarter.BottomLeft}, TestName = "bottom left quarter when its is connected")]
        [TestCase(20, -20, 10, 10, new []{Quarter.BottomRight}, TestName = "bottom right quarter when its is connected")]
        public void GetQuarter(double x, double y, double width, double height, Quarter[] expectedResult)
        {
            var rectCenter = new Point(0, 0);
            var size = new Size(width, height);
            var rectangle = new Rectangle(rectCenter, size);

            var actualResult = rectangle.GetQuarters();

            actualResult.Should().Contain(expectedResult);
        }
    }
}
