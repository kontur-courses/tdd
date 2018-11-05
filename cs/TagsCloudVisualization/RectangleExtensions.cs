using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static double DistanceToPoint(this Rectangle rectangle, Point point)
        => point.DistanceTo(rectangle);

        private static long GetDistanceToRange(int number, int minValue, int maxValue)
        {
            if (number < minValue)
                return (long)minValue - number;
            return number > maxValue
                ? (long)number - maxValue
                : 0;
        }

        public static double DistanceToRectangle(this Rectangle firstRectangle, Rectangle secondRectangle)
        {
            var differenceX = Math.Min(GetDistanceToRange(firstRectangle.Left, secondRectangle.Left, secondRectangle.Right),
                GetDistanceToRange(firstRectangle.Right, secondRectangle.Left, secondRectangle.Right));
            var differenceY = Math.Min(GetDistanceToRange(firstRectangle.Bottom, secondRectangle.Top, secondRectangle.Bottom),
                GetDistanceToRange(firstRectangle.Top, secondRectangle.Top, secondRectangle.Bottom));


            return Math.Sqrt(differenceX * differenceX + differenceY * differenceY);
        }
    }

    [TestFixture]
    public class RectangleExtensions_Should
    {
        [Test]
        [TestCase(0, 0, 10, 10, 5, 5, 0, TestName = "point inside of rectangle")]
        [TestCase(0, 0, 10, 10, 0, 10, 0, TestName = "point in bottom-left vertex")]
        [TestCase(0, 0, 10, 10, 10, 10, 0, TestName = "point in bottom-right vertex")]
        [TestCase(0, 0, 10, 10, 0, 0, 0, TestName = "point in top-left vertex")]
        [TestCase(0, 0, 10, 10, 10, 0, 0, TestName = "point in top-right vertex")]
        [TestCase(0, 0, 10, 10, 0, 5, 0, TestName = "point in left side")]
        [TestCase(0, 0, 10, 10, 10, 5, 0, TestName = "point in right side")]
        [TestCase(0, 0, 10, 10, 5, 0, 0, TestName = "point in top side")]
        [TestCase(0, 0, 10, 10, 5, 10, 0, TestName = "point in bottom side")]
        [TestCase(0, 0, 10, 10, 0, 12, 2, TestName = "point in left side straight below rectangle")]
        [TestCase(0, 0, 10, 10, 0, -2, 2, TestName = "point in left side straight above rectangle")]
        [TestCase(0, 0, 10, 10, 10, 12, 2, TestName = "point in right side straight below rectangle")]
        [TestCase(0, 0, 10, 10, 10, -2, 2, TestName = "point in right side straight above rectangle")]
        [TestCase(0, 0, 10, 10, -2, 10, 2, TestName = "point in bottom side straight to the left of rectangle")]
        [TestCase(0, 0, 10, 10, 12, 10, 2, TestName = "point in bottom side straight to the right right of rectangle")]
        [TestCase(0, 0, 10, 10, -2, 0, 2, TestName = "point in top side straight to the left of rectangle")]
        [TestCase(0, 0, 10, 10, 12, 0, 2, TestName = "point in top side straight to the right right of rectangle")]
        [TestCase(0, 0, 10, 10, 5, 20, 10, TestName = "point x is in range of rectangle x")]
        [TestCase(0, 0, 10, 10, 20, 5, 10, TestName = "point y is in range of rectangle y")]
        [TestCase(0, 0, 10, 10, 20, 20, 14.1421356237, TestName = "point x and y is out of range rectangle x and y")]
        [TestCase(20, 20, 10, 10, 10, 10, 14.1421356237, TestName = "rectangle starts not in zero point")]
        [TestCase(10, 10, 5, 5, -10, -10, 28.2842712475, TestName = "point has negative coordinates")]
        [TestCase(-15, -15, 5, 5, 10, 10, 28.2842712475, TestName = "rectangle start point has negative coordinates")]
        [TestCase(0, 0, 10, 10, 16, 18, 10, TestName = "angle between minimum length segment and ox axis is 60 degrees")]
        public void DistanceToPoint_ReturnsCorrectResult(int startRectangleX,
            int startRectangleY,
            int width,
            int height,
            int pointX,
            int pointY,
            double expectedResult)
        {
            var rectangle = new Rectangle(startRectangleX, startRectangleY, width, height);
            var point = new Point(pointX, pointY);

            var actualLength = rectangle.DistanceToPoint(point);

            actualLength.Should()
                .BeApproximately(expectedResult, 1e-9);
        }

        [Test]
        public void DistanceToPoint_ReturnsCorrectResult_WhenLengthIsCloseToMaxInteger()
        {
            var startRectanglePoint = new Point(0, 0);
            var rectangleSize = new Size(10, 10);
            var rectangle = new Rectangle(0, 0, 10, 10);
            var bigInteger = (int)1e9 + 10;
            var point = new Point(bigInteger, 11);

            var actualLength = rectangle.DistanceToPoint(point);

            actualLength.Should()
                .BeApproximately(bigInteger - 10, 1);
        }

        [TestCase(0, 0, 10, 10, 0, 0, 10, 10, 0, TestName = "rectangles are same")]
        [TestCase(0, 0, 10, 10, 10, 10, 10, 10, 0, TestName = "rectangles have same vertex")]
        [TestCase(0, 0, 10, 10, 5, 5, 10, 10, 0, TestName = "rectangles have interstection with positive square")]
        [TestCase(0, 0, 10, 10, 10, 0, 10, 10, 0, TestName = "rectangles have common side")]
        [TestCase(0, 0, 10, 10, 11, 11, 10, 10, 1.41421356237, TestName = "rectangles do not contains points with same x or y")]
        [TestCase(0, 0, 10, 10, 5, 11, 10, 10, 1, TestName = "rectangles have points with same x")]
        [TestCase(0, 0, 10, 10, 11, 5, 10, 10, 1, TestName = "rectangles have points with same y")]
        [TestCase(0, 0, 10, 10, 10, 20, 10, 10, 10, TestName = "there is only one x such that points from rectangles have such x")]
        [TestCase(0, 0, 10, 10, 20, 10, 10, 10, 10, TestName = "there is only one y such that points from rectangles have such y")]
        [TestCase(10, 10, 10, 10, 30, 30, 40, 40, 14.1421356237, TestName = "first rectangle starts not in zero point")]
        [TestCase(-15, -15, 5, 5, 10, 10, 10, 10, 28.2842712475, TestName = "first rectangle start point has negative coordinates")]
        [TestCase(10, 10, 10, 10, -15, -15, 5, 5, 28.2842712475, TestName = "second rectangle start point has negative coordinates")]
        [TestCase(-10, -10, 10, 10, -35, -35, 5, 5, 28.2842712475, TestName = "rectangles start points have negative coordinates")]
        [TestCase(-10, 10, 10, 10, 30, 50, 5, 5, 42.4264068712, TestName = "rectangle start has negative x and positive y")]
        [TestCase(0, 0, 10, 10, 16, 18, 10, 10, 10, TestName = "angle between minimum length segment and ox axis has angle of 60 degrees")]

        public void DistanceToRectangle_ReturnsCorrectResult(int startX,
            int startY,
            int width,
            int height,
            int secondStartX,
            int secondStartY,
            int secondWidth,
            int secondHeight,
            double expectedLength)
        {
            var firstRectangle = new Rectangle(startX, startY, width, height);
            var secondRectangle = new Rectangle(secondStartX, secondStartY, secondWidth, secondHeight);

            var actualLength = firstRectangle.DistanceToRectangle(secondRectangle);

            actualLength.Should()
                .BeApproximately(expectedLength, 1e-9);
        }

        [Test]
        public void DistanceToRectangle_ReturnsCorrectResult_WhenLengthIsCloseToMaxInteger()
        {
            var rectangle = new Rectangle(0, 0, 10, 10);
            var bigInteger = 10 + (int)1e9;
            var secondRectangle = new Rectangle(bigInteger, 10, 11, 10);

            var actualLength = rectangle.DistanceToRectangle(secondRectangle);

            actualLength.Should()
                .BeApproximately(bigInteger - 10, 1);
        }

        [Test, Timeout(1000)]
        public void DistanceToRectangle_WorksFast_WhenItExecutesManyTimes()
        {
            var firstRectangle = new Rectangle(0, 0, 100, 100);
            var secondRectangle = new Rectangle(1000, 2000, 100, 100);
            for (var index = 0; index < 100000; index++)
            {
                firstRectangle.DistanceToRectangle(secondRectangle);
            }
        }

        [Test, Timeout(1000)]
        public void DistanceToPoint_WorksFast_WhenItExecutesManyTimes()
        {
            var rectangle = new Rectangle(0, 0, 100, 100);
            var point = new Point(1000, 2000);
            for (var index = 0; index < 100000; index++)
            {
                rectangle.DistanceToPoint(point);
            }
        }
    }
}
