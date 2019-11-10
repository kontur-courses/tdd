using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class UtilsTests
    {
        [TestCase(50, -5, 0)]
        [TestCase(-5, 50, Math.PI / 2)]
        [TestCase(-50, -5, Math.PI)]
        [TestCase(-5, -50, 1.5 * Math.PI)]
        [TestCase(50, 50, Math.PI / 4)]
        [TestCase(-50, 50, Math.PI * 3 / 4)]
        [TestCase(-50, -50, Math.PI * 1.25)]
        [TestCase(50, -50, 1.76 * Math.PI)]
        public void Rectangle_IsIntersectsByRay_ShouldReturnTrue(int rectX, int rectY, double rayAngle)
        {
            new Rectangle(rectX, rectY, 10, 10).IsIntersectsByRay(rayAngle, out double _).Should().BeTrue();
        }

        [TestCase(50, 20, 0, ExpectedResult = 25)]
        [TestCase(10, 10, Math.PI / 4, ExpectedResult = 7)]
        [TestCase(66, 11, 0.17453292519943295, ExpectedResult = 33)]
        public int LengthOfRayFromCenterOfRectangle_ReturnsCorrectValue(int width, int height, double rayAngle)
        {
            return (int)Utils.LengthOfRayFromCenterOfRectangle(new Rectangle(0, 0, width, height), rayAngle);
        }

        [TestCase(0, 10, 10, 0)]
        [TestCase(Math.PI, 10, -10, 0)]
        [TestCase(Math.PI / 4, 10, 7, 7)]
        public void Point_FromPolar_ReturnsCorrectValue(double angle, double dist, int x, int y)
        {
            new Point().FromPolar(angle, dist).Should().Be(new Point(x, y));
        }

        [TestCase(0, 0, (int)(1.41 * 10))]
        [TestCase(-10, 0, (int)(1.41 * 10))]
        [TestCase(-10, -10, (int)(1.41 * 10))]
        [TestCase(0, -10, (int)(1.41 * 10))]
        public void GetDistanceOfFathestFromCenterVertex_ReturnsCorrectValue(int rectX, int rectY, int distance)
        {
            ((int)new Rectangle(rectX, rectY, 10, 10).GetDistanceOfFathestFromCenterVertex()).Should().Be(distance);
        }

        [Test]
        public void GetDistanceIfIntersectsByRay_ReturnsCorrectValue()
        {
            new Rectangle[] { new Rectangle(-10, -10, 20, 20), new Rectangle(-5, 15, 10, 10) }
                .Max(r => r.GetDistanceIfIntersectsByRay(0.5 * Math.PI))
                .Should().Be(25);
        }
    }
}
