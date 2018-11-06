using System;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    [TestFixture]
    public class SpiralPointsSequence_Should
    {
        [TestCase(Math.PI * 3, TestName = "If step equal to PI * 3")]
        [TestCase(100, TestName = "If step equal to 100")]
        [TestCase(1000, TestName = "If step equal to 1000")]
        public void ReturnDifferentPoints(double step)
        {
            var sequence = new SpiralPointsSequence(step);

            var first = sequence.GetNextPoint();
            var second = sequence.GetNextPoint();

            first.Should().NotBe(second);
        }

        [TestCase(0,TestName = "After 0 steps")]
        [TestCase(1, TestName = "After 1 steps")]
        [TestCase(100, TestName = "After 100 steps")]
        [TestCase(1000, TestName = "After 1000 steps")]
        public void ResetEnumeration_ThenResetMethodCalled(int steps)
        {
            var sequence = new SpiralPointsSequence(1);
            var first = sequence.GetNextPoint();

            for (var i = 0; i < steps; i++)
                sequence.GetNextPoint();
            sequence.Reset();

            first.Should().Be(sequence.GetNextPoint());
        }

        [TestCase(0, TestName = "Then step is 0")]
        [TestCase(-0.00000000001, TestName = "Then step is small negative number")]
        [TestCase(-1, TestName = "Then step is -1")]
        [TestCase(-150, TestName = "Then step is -150")]
        public void ConstructorThrowArgumentException(double step)
        {
            Action constructor = () => new SpiralPointsSequence(step);

            constructor.Should().Throw<ArgumentException>();
        }
    }
}