using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class ArchimedesSpiralConstructor_Should
    {
        [TestCase(0, 10, TestName = "radius is zero")]
        [TestCase(10, 0, TestName = "increment is zero")]
        [TestCase(0, 0, TestName = "radius and increment are zero")]
        public void Constructor_Should_ThrowArgumentException_When(float radius, float increment)
        {
            Action action = () => new ArchimedesSpiral(new Point(1, 1), radius, increment);

            action.Should().Throw<ArgumentException>();
        }
    }

    [TestFixture]
    public class ArchimedesSpiralGetNextCoordinates_Should
    {
        private ArchimedesSpiral archimedesSpiral;
        private Point spiralCenter = new Point(500, 500);

        [SetUp]
        public void Init()
        {
            archimedesSpiral = new ArchimedesSpiral(spiralCenter, 1, 1);
        }

        [Test]
        public void FirstCoordinateEqualToCenter()
        {
            var coordinates = archimedesSpiral.GetNextCoordinates();

            coordinates.Should().BeEquivalentTo(spiralCenter);
        }

        [Test]
        public void IncrementCoordinatesAutomatically()
        {
            var firstCoordinates = archimedesSpiral.GetNextCoordinates();
            var nextCoordinates = archimedesSpiral.GetNextCoordinates();

            firstCoordinates.Should().NotBeEquivalentTo(nextCoordinates);
        }
    }
}