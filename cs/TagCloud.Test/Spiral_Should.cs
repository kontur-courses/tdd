using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud.Test
{
    [TestFixture]
    public class Spiral_Should
    {
        [TestCase(0, TestName = "Then step length is 0")]
        [TestCase(-0.00000000001, TestName = "Then step length is small negative number")]
        [TestCase(-5, TestName = "Then step length is negative")]
        public void ConstructorThrowArgumentException(double stepLength)
        {
            Action constructor = () => Spiral.Create().WithStepLength(stepLength);

            constructor.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 0)]
        [TestCase(-1, -1)]
        [TestCase(1, 1)]
        public void ReturnCenter_ThenGettingFirstPoint(int x, int y)
        {
            Spiral spiral = Spiral.Create().WithCenterIn(new Point(x, y));

            var result = spiral.GetNextPoint();

            result.Should().Be(new Point(x, y));
        }

        [Test]
        public void ReturnPointsInAlmostCircleWay()
        {
            var spiral = new Spiral();
            var points = new Point[100];

            for (var i = 0; i < points.Length; i++)
                points[i] = spiral.GetNextPoint();
            var size = points.GetBounds().Size;

            size.Width.Should().BeCloseTo(size.Height, 10);
        }

        [TestCase(Math.PI * 3, TestName = "If step equal to PI * 3")]
        [TestCase(100, TestName = "If step equal to 100")]
        public void ReturnDifferentPoints(double stepLength)
        {
            Spiral sequence = Spiral.Create().WithStepLength(stepLength);
            var first = sequence.GetNextPoint();
            var second = sequence.GetNextPoint();

            first.Should().NotBe(second);
        }

        [TestCase(0, TestName = "After 0 steps")]
        [TestCase(1, TestName = "After 1 steps")]
        [TestCase(1000, TestName = "After 1000 steps")]
        public void ResetEnumeration_ThenResetMethodCalled(int steps)
        {
            var sequence = new Spiral();
            var first = sequence.GetNextPoint();

            for (var i = 0; i < steps; i++)
                sequence.GetNextPoint();
            sequence.Reset();

            first.Should().Be(sequence.GetNextPoint());
        }
    }
}