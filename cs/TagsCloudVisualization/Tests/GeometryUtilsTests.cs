using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class GeometryUtilsTests
    {
        [TestCase(6, Math.PI / 2, 0, 6)]
        [TestCase(5, 0, 5, 0)]
        [TestCase(3, Math.PI, -3, 0)]
        public void ConvertPolarToIntegerCartesian_ShouldReturnCorrectResult_OnPolar(
            double r, double phi, int x, int y)
        {
            var expectedPoint = new Point(x, y);

            var result = GeometryUtils.ConvertPolarToIntegerCartesian(r, phi);

            result.Should().Be(expectedPoint);
        }

        [Test]
        public void GetPossibleRectangles_ShouldReturnRectanglesWithFourDifferentStartPoints_OnPointAndSize()
        {
            var point = new Point(0, 0);
            var size = new Size(2, 1);
            var possibleRectangles = new[]
            {
                new Rectangle(point, size),
                new Rectangle(new Point(-2, 0), size),
                new Rectangle(new Point(-2, -1), size),
                new Rectangle(new Point(0, -1), size),
            };

            var result = GeometryUtils.GetPossibleRectangles(point, size);

            result.Should().BeEquivalentTo(possibleRectangles);
        }

        [Test]
        public void RectanglesAreIntersected_ShouldReturnFalse_WhenRectanglesAreNotIntersected()
        {
            var firstRectangle = new Rectangle(new Point(3, 3), new Size(3, 2));
            var secondRectangle = new Rectangle(new Point(0, 0), new Size(3, 2));

            GeometryUtils
                .RectanglesAreIntersected(firstRectangle, secondRectangle)
                .Should()
                .BeFalse();
        }

        [Test]
        public void RectanglesAreIntersected_ShouldReturnTrue_WhenRectanglesAreIntersectedInTheCorner()
        {
            var firstRectangle = new Rectangle(new Point(1, 1), new Size(3, 2));
            var secondRectangle = new Rectangle(new Point(0, 0), new Size(3, 2));

            GeometryUtils
                .RectanglesAreIntersected(firstRectangle, secondRectangle)
                .Should()
                .BeTrue();
        }

        [Test]
        public void RectanglesAreIntersected_ShouldReturnTrue_WhenOneInsideTheOther()
        {
            var firstRectangle = new Rectangle(new Point(1, 1), new Size(1, 1));
            var secondRectangle = new Rectangle(new Point(0, 0), new Size(3, 3));

            GeometryUtils
                .RectanglesAreIntersected(firstRectangle, secondRectangle)
                .Should()
                .BeTrue();
        }

        [Test]
        public void RectanglesAreIntersected_ShouldReturnTrue_WhenTheyPlacedAsACross()
        {
            var firstRectangle = new Rectangle(new Point(1, 0), new Size(1, 3));
            var secondRectangle = new Rectangle(new Point(0, 1), new Size(3, 1));

            GeometryUtils
                .RectanglesAreIntersected(firstRectangle, secondRectangle)
                .Should()
                .BeTrue();
        }

        [Test]
        public void GetRectanglesThatCloserToPoint_ShouldBeEmpty_WhenPointInRectangle()
        {
            var point = new Point(1, 1);
            var rectangle = new Rectangle(new Point(0, 0), new Size(3, 2));

            var result = GeometryUtils.GetRectanglesThatCloserToPoint(point, rectangle, 1);

            result.Should().BeEmpty();
        }

        [Test]
        public void GetRectanglesThatCloserToPoint_ShouldBeEmpty_WhenPointOnRectangleSide()
        {
            var point = new Point(2, 0);
            var rectangle = new Rectangle(new Point(0, 0), new Size(3, 2));

            var result = GeometryUtils.GetRectanglesThatCloserToPoint(point, rectangle, 1);

            result.Should().BeEmpty();
        }

        [Test]
        public void GetRectanglesThatCloserToPoint_ShouldNotReturnTheSameRectangle_WhenIntersectsPointCoordinate()
        {
            var point = new Point(0, 0);
            var size = new Size(2, 1);
            var rectangle = new Rectangle(new Point(0, 2), size);
            var expectedRectangle = new Rectangle(new Point(0, 1), size);

            var result = GeometryUtils.GetRectanglesThatCloserToPoint(point, rectangle, 1);

            result.Should().OnlyContain(r => r == expectedRectangle);
        }

        [TestCase(2, 2, 1, 1, TestName = "I quadrant")]
        [TestCase(-5, 2, -4, 1, TestName = "II quadrant")]
        [TestCase(-5, -4, -4, -3, TestName = "III quadrant")]
        [TestCase(2, -4, 1, -3, TestName = "IV quadrant")]
        public void GetRectanglesThatCloserToPoint_ShouldReturnCloserRectangles_OnRectangle(
            int x, int y, int dx, int dy)
        {
            var point = new Point(0, 0);
            var size = new Size(3, 2);
            var rectangle = new Rectangle(new Point(x, y), size);
            var expectedRectangles = new[]
            {
                new Rectangle(new Point(dx, y), size),
                new Rectangle(new Point(x, dy), size),
                new Rectangle(new Point(dx, dy), size),
            };

            var result = GeometryUtils.GetRectanglesThatCloserToPoint(point, rectangle, 1);

            result.Should().BeEquivalentTo(expectedRectangles);
        }
    }
}
