using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class GeometryTests
    {
        [TestCase(10, 0, 10, 0)]
        [TestCase(15, 2.0 / 3, 11, 9)]
        [TestCase(100, 1, 54, 84)]
        [TestCase(-10, 2, 4, -9)]
        [TestCase(40, -8, -5, -39)]
        public void PolarToCartesian_ReturnsCorrectPoint_OnDifferentPolarValues(double ro, double phi, int x, int y)
        {
            Geometry.PolarToCartesian(ro, phi).Should().Be(new Point(x, y));
        }


        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(int.MinValue, int.MinValue)]
        public void ShiftPointBySizeOffsets_ThrowsException_IfSizeHasNegativeParameter(int width, int height)
        {
            Action action = () => Geometry.ShiftPointBySizeOffsets(Point.Empty, new Size(width, height));

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0, 10, 10)]
        [TestCase(25, -10, 10, 10)]
        [TestCase(0, 0, 10, 0)]
        [TestCase(0, 0, 0, 1)]
        public void ShiftPointBySizeOffsets_ReturnsCorrectPoint_OnDifferentValues(int x, int y, int width, int height)
        {
            var shiftedPoint = Geometry.ShiftPointBySizeOffsets(new Point(x, y), new Size(width, height));

            shiftedPoint.Should().Be(new Point(x - width / 2, y - height / 2));
        }

        [TestCase(0, 0, 10, 10, 15, 0, 5)]
        [TestCase(0, 0, 10, 10, 15, 15, 7.071)]
        [TestCase(9, 3, 2, 2, 8, 1, 1.118)]
        [TestCase(0, 0, 20, 20, 15, 30, 11.180)]
        public void GetLengthFromRectCenterToBorderOnVector_ReturnsCorrectLength_OnDifferentValues(
            int rectX, int rectY, int width, int height, int endX, int endY, double expectedLength)
        {
            var epsilon = 0.001;
            var rectangle = new Rectangle(rectX, rectY, width, height);
            var endPoint = new Point(endX, endY);

            var length = Geometry.GetLengthFromRectCenterToBorderOnVector(rectangle, endPoint);

            length.Should().BeInRange(expectedLength - epsilon, expectedLength + epsilon);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(int.MinValue, int.MinValue)]
        public void GetLengthFromRectangleCenterToBorderOnVector_ThrowsException_IfSizeHasNegativeValue(int width, int height)
        {
            Action action = () =>
                Geometry.GetLengthFromRectCenterToBorderOnVector(new Rectangle(0, 0, width, height), Point.Empty);

            action.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0, 10, 10, 0, 0)]
        [TestCase(0, 0, 10, 10, 3, -3)]
        [TestCase(10, -40, 20, 20, 5, -35)]
        public void GetLengthFromRectCenterToBorderOnVector_ReturnsZero_IfPointIsInsideRectangle(
            int rectX, int rectY, int width, int height, int endX, int endY)
        {
            var rectangle = new Rectangle(rectX, rectY, width, height);
            var endPoint = new Point(endX, endY);

            var length = Geometry.GetLengthFromRectCenterToBorderOnVector(rectangle, endPoint);

            length.Should().Be(0);
        }
    }
}