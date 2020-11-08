using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualizationTests.PointsGeneratorsTests
{
    [TestFixture]
    public class ArchimedesSpiral
    {
        [SetUp]
        public void SetUp()
        {
            sut = new TagsCloudVisualization.PointsGenerators.ArchimedesSpiral(new Point(250, 250));
        }

        private IPointGenerator sut;

        [Test]
        public void InitArchimedesSpiral_CenterParamEqualProperty_CorrectArguments()
        {
            var center = new Point(10, 20);

            var spiral = new TagsCloudVisualization.PointsGenerators.ArchimedesSpiral(center, 2, 0.2f);

            spiral.Center.Should().Be(center);
        }

        [TestCase(0, TestName = "Angle step equal 0 without frac part")]
        [TestCase(0.0000f, TestName = "Angle step approximately equal 0")]
        [TestCase(-0.0000000f, TestName = "Angle step approximately equal minus 0")]
        [TestCase(0.5f, 0, TestName = "Spiral parameter equal 0")]
        public void InitArchimedesSpiral_Throws_IncorrectArguments(float angleStep,
            int spiralParameter = 1)
        {
            Assert.Throws<ArgumentException>(
                () => new TagsCloudVisualization.PointsGenerators.ArchimedesSpiral(new Point(10, 20), spiralParameter, angleStep));
        }

        [Test]
        public void StartOverPointGenerator_ShouldResetGeneration()
        {
            var expectedPoints = new Point[3];
            for (int i = 0; i < 3; i++)
                expectedPoints[i] = sut.GetNextPoint()!.Value;

            sut.StartOver();

            var actualPoints = new Point[3];
            for (int i = 0; i < 3; i++)
                actualPoints[i] = sut.GetNextPoint()!.Value;

            actualPoints.Should().Equal(expectedPoints);
        }

        [TestCase(2, 0.2f, 250, 250, 252, 252, 250, 252, 
            TestName = "Spiral parameter and angle step are positive")]
        [TestCase(-2, 0.2f, 250, 250, 248, 248, 250, 248, 
            TestName = "Negative spiral parameter")]
        [TestCase(2, -1f, 250, 250, 248, 252, 252, 254, 
            TestName = "Negative angle step")]
        [TestCase(-2, -1f, 250, 250, 252, 248, 248, 246, 
            TestName = "Spiral parameter and angle step are negative")]
        public void GetNextGeneratedPoint_ShouldConsistentlyReturnsNextSpiralPoints(int spiralParameter, float angleStep,
            params int[] expectedPointCoordinates)
        {
            var center = new Point(250, 250);
            var archimedesSpiral = new TagsCloudVisualization.PointsGenerators.ArchimedesSpiral(center, spiralParameter, angleStep);
            var expectedPoints = new Point[3];
            for (int i = 0; i < expectedPoints.Length; i++)
                expectedPoints[i] = new Point(expectedPointCoordinates[i * 2], expectedPointCoordinates[i * 2 + 1]);

            var actualPoints = new Point[3];
            for (int i = 0; i < 3; i++)
                actualPoints[i] = archimedesSpiral.GetNextPoint()!.Value;

            actualPoints.Should().Equal(expectedPoints);
        }
    }
}