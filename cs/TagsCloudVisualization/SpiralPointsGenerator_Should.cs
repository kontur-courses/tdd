using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class SpiralPointsGenerator_Should
    {
        private const int DistanceBetweenPoints = 1;
        private SpiralPointsGenerator generator;

        [SetUp]
        public void SetUp()
        {
            generator = new SpiralPointsGenerator(DistanceBetweenPoints);
        }

        [Test]
        public void Have_ConstructorTakingDistanceBetweenPoints()
        {
            Action action = () => new SpiralPointsGenerator(DistanceBetweenPoints);
            action.Should().NotThrow();
        }

        [TestCase(0, TestName = "DistanceIsZero")]
        [TestCase(-1, TestName = "DistanceIsNegative")]
        public void ThrowArgumentException_OnInvalidDistance(int distanceBetweenPoints)
        {
            Action action = () => new SpiralPointsGenerator(distanceBetweenPoints);
            action.Should().NotThrow<ArgumentException>();
        }

        [Test]
        public void ReturnZeroPoint_OnFirstGenerating()
        {
            generator.GetNextPoint().Should().Be(new Point());
        }

        [Test]
        public void ReturnNotZeroPoint_AfterFirstGenerating()
        {
            generator.GetNextPoint();
            generator.GetNextPoint().Should().NotBe(new Point());
        }

        [Test]
        public void Have_EmptyList_AfterCreating()
        {
            generator.AllGeneratedPoints.Should().BeEmpty();
        }

        [TestCase(1, TestName = "OnePoint")]
        [TestCase(2, TestName = "TwoPoints")]
        [TestCase(1000, TestName = "ThousandPoints")]
        public void Have_AllPointsInList_AfterGenerating(int pointsAmount)
        {
            for (var i = 0; i < pointsAmount; i++)
                generator.GetNextPoint();
            generator.AllGeneratedPoints.Count.Should().Be(pointsAmount);
        }
    }
}