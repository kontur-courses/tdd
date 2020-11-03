using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualizationTests.PointsGeneratorsTests
{
    [TestFixture]
    public class ArchimedesSpiral_Should
    {
        [SetUp]
        public void SetUp()
        {
            var canvasSize = new Size(500, 500);
            var centerPoint = new Point(canvasSize.Width / 2, canvasSize.Height / 2);
            pointsGenerator = new ArchimedesSpiral(centerPoint, canvasSize);
        }

        private IPointGenerator pointsGenerator;

        [TestCase(3, 1f, TestName = "Spiral parameter is positive")]
        [TestCase(-3, 1f, TestName = "Spiral parameter is negative")]
        [TestCase(1, 0.3f, TestName = "Angle step is positive")]
        [TestCase(1, -0.3f, TestName = "Angle step is negative")]
        public void InitArchimedesSpiral_InitParamsEqualProperties_CorrectArguments(
            int spiralParameter,
            float angleStep)
        {
            var center = new Point(10, 20);
            var canvasSize = new Size(50, 200);

            var spiral = new ArchimedesSpiral(center, canvasSize, spiralParameter, angleStep);

            spiral.Center.Should().Be(center);
            spiral.CanvasSize.Should().Be(canvasSize);
            spiral.AngleStep.Should().Be(angleStep);
            spiral.SpiralParameter.Should().Be(spiralParameter);
        }

        [TestCase(10, 10, 0,
            TestName = "Angle step equal 0 without frac part")]
        [TestCase(10, 10, 0.0000f,
            TestName = "Angle step approximately equal 0")]
        [TestCase(10, 10, -0.0000000f,
            TestName = "Angle step approximately equal minus 0")]
        [TestCase(0, 10, 0.5f,
            TestName = "Canvas width equal 0")]
        [TestCase(10, 0, 0.5f,
            TestName = "Canvas height equal 0")]
        [TestCase(-10, 10, 0.5f,
            TestName = "Canvas width is negative")]
        [TestCase(10, 10, 0.5f, 0,
            TestName = "Spiral parameter equal 0")]
        public void InitArchimedesSpiral_Throws_IncorrectArguments(int canvasWidth, int canvasHeight, float angleStep,
            int spiralParameter = 1)
        {
            var canvasSize = new Size(canvasWidth, canvasHeight);

            Assert.Throws<ArgumentException>(
                () => new ArchimedesSpiral(new Point(10, 20), canvasSize, spiralParameter, angleStep));
        }

        [Test]
        public void StartOverPointGenerator_ShouldResetGeneration()
        {
            var expectedPoints = new Point[3];
            for (int i = 0; i < 3; i++)
                expectedPoints[i] = pointsGenerator.GetNextPoint();

            pointsGenerator.StartOver();

            var actualPoints = new Point[3];
            for (int i = 0; i < 3; i++)
                actualPoints[i] = pointsGenerator.GetNextPoint();

            actualPoints.Should().Equal(expectedPoints);
        }

        [TestCase(2, 0.2f, 250, 250, 252, 252, 250, 252, TestName = "Spiral parameter and angle step are positive")]
        [TestCase(-2, 0.2f, 250, 250, 248, 248, 250, 248, TestName = "Negative spiral parameter")]
        [TestCase(2, -1f, 250, 250, 248, 252, 252, 254, TestName = "Negative angle step")]
        [TestCase(-2, -1f, 250, 250, 252, 248, 248, 246, TestName = "Spiral parameter and angle step are negative")]
        public void GetNextGeneratedPoint_ShouldConsistentlyReturnsNextSpiralPoints(int spiralParameter, float angleStep,
            params int[] expectedPointCoordinates)
        {
            var center = new Point(250, 250);
            var canvasSize = new Size(500, 600);
            var archimedesSpiral = new ArchimedesSpiral(center, canvasSize, spiralParameter, angleStep);
            var expectedPoints = new Point[3];
            for (int i = 0; i < expectedPoints.Length; i++)
                expectedPoints[i] = new Point(expectedPointCoordinates[i * 2], expectedPointCoordinates[i * 2 + 1]);

            var actualPoints = new Point[3];
            for (int i = 0; i < 3; i++)
                actualPoints[i] = archimedesSpiral.GetNextPoint();

            actualPoints.Should().Equal(expectedPoints);
        }
    }
}