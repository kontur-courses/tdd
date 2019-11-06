using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class GeometryUtils_Should
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
    }
}
