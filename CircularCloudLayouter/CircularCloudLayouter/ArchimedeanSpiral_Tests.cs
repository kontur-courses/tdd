using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;

namespace TagsCloudVisualizer
{
    [TestFixture]
    class ArchimedeanSpiral_Tests
    {
        [Test]
        public void GetNextPoint_AfterCreation_ShouldReturnCenter()
        {
            var point = new Point(1, 1);
            var spiral = new ArchimedeanSpiral(point);
            spiral.GetNextPoint().Should().Be(point);
        }
        [TestCase(0, 0, Math.PI, 1, ExpectedResult = new int[] { -1, 0 })]
        [TestCase(0, 0, -Math.PI, 1, ExpectedResult = new int[] { -1, 0 })]
        [TestCase(0, 0, Math.PI, -1, ExpectedResult = new int[] { 1, 0 })]
        [TestCase(1, 1, Math.PI, 1, ExpectedResult = new int[] { 0, 1 })]
        [TestCase(3, 3, 0.0001, 0.0001, ExpectedResult = new int[] { 3, 3 })]
        public int[] GetNextPoint_AfterOneInvoking_ShouldReturnCorrectPoint(int x, int y, double angle, double linear)
        {
            var point = new Point(x, y);
            var spiral = new ArchimedeanSpiral(point, angle, linear);
            spiral.GetNextPoint();
            var next = spiral.GetNextPoint();
            return new int[] { next.X, next.Y };
        }
        [TestCase(0, 0, Math.PI, 1, ExpectedResult = new int[] { 2, 0 })]
        [TestCase(0, 0, -Math.PI, 2, ExpectedResult = new int[] { 4, 0 })]
        public int[] GetNextPoint_AfterTwoInvokings_ShouldReturnCorrectPoint(int x, int y, double angle, double linear)
        {
            var point = new Point(x, y);
            var spiral = new ArchimedeanSpiral(point, angle, linear);
            spiral.GetNextPoint();
            spiral.GetNextPoint();
            var next = spiral.GetNextPoint();
            return new int[] { next.X, next.Y };
        }
        [Test]
        public void Constructor_IfBothSpeedsAreZero_ShouldThrow()
        {
            Action action = () => new ArchimedeanSpiral(new Point(0, 0), 0, 0);
            action.Should().Throw<Exception>();
        }
        [Test]
        public void Constructor_IfAngleSpeedIsZero_ShouldThrow()
        {
            Action action = () => new ArchimedeanSpiral(new Point(0, 0), 0, 100);
            action.Should().Throw<Exception>();
        }
        [Test]
        public void Constructor_IfLinearSpeedIsZero_ShouldThrow()
        {
            Action action = () => new ArchimedeanSpiral(new Point(0, 0), 100, 0);
            action.Should().Throw<Exception>();
        }
    }
}
