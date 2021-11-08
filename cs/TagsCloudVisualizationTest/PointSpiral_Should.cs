using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;
using static FluentAssertions.FluentActions;


namespace TestProject1
{
    [TestFixture]
    public class PointSpiral_Should
    {
        private PointSpiral pointSpiral;
        
        [SetUp]
        public void Setup()
        {
            pointSpiral = PointSpiralBuilder
                .APointSpiral()
                .WithCenter(new Point(250, 250))
                .Build();
        }
        
        [TestCase(-1, -1)]
        [TestCase(-1, 0)]
        [TestCase(-1, 1)]
        [TestCase(0, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(0, 0, 2, 1)]
        [TestCase(0, 0, 1, 2)]
        public void PointSpiral_DoNotThrowAnyException_OnAnyCenterPointAndPositiveDensityParameterAndDegreesParameter(int x, int y, int degreesParameter=1, float densityParameter=1f)
        {
            var builder = PointSpiralBuilder.APointSpiral()
                .WithCenter(new Point(x, y))
                .WithDensityParameter(densityParameter)
                .WithDegreesDelta(degreesParameter);

            Invoking(() => builder.Build()).Should().NotThrow($"X = {x}; " +
                                                              $"Y = {y}, " +
                                                              $"degreesParameter = {degreesParameter}; " +
                                                              $"densityParameter = {densityParameter}");
        }
        
        [TestCase(0, 0, 0, 1)]
        [TestCase(0, 0, 1, 0)]
        [TestCase(0, 0, -1, 1)]
        [TestCase(0, 0, 1, -1)]
        [TestCase(0, 0, -1, -1)]
        public void PointSpiral_DoNotThrowAnyException_OnNonPositiveDensityParameterOrDegreesParameter(int x, int y, int degreesParameter=1, float densityParameter=1f)
        {
            var builder = PointSpiralBuilder.APointSpiral()
                .WithCenter(new Point(x, y))
                .WithDensityParameter(densityParameter)
                .WithDegreesDelta(degreesParameter);

            Invoking(() => builder.Build()).Should().Throw<ArgumentException>($"X = {x}; " +
                                                                              $"Y = {y}, " +
                                                                              $"degreesParameter = {degreesParameter}; " +
                                                                              $"densityParameter = {densityParameter}");
        }

        [TestCase(-1, -1)]
        [TestCase(-1, 0)]
        [TestCase(-1, 1)]
        [TestCase(0, -1)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        public void PointSpiral_FirstPointShouldBeOnMiddle(int x, int y)
        {
            var expected = new Point(x, y);
            var actual = PointSpiralBuilder
                .APointSpiral()
                .WithCenter(new Point(x, y))
                .Build()
                .GetPoints()
                .First();
            
            actual.Should().Be(expected, $"X = {x}; Y = {y}");
        }
        
        [Test]
        [Repeat(100)]
        public void AutoTest_PointSpiral()
        {
            pointSpiral = PointSpiralBuilder
                .APointSpiral()
                .WithCenter(Point.Empty)
                .WithDegreesDelta(25)
                .WithDensityParameter(10)
                .Build();
            
            var lastRadius = 0d;
            var lastPointDifferent = 0d;
            var lastPoint = Point.Empty;

            var pointCount = 1000;
            
            foreach (var point in pointSpiral)
            {
                var radius = point.MetricTo(new Point(0, 0));
                var pointDifferent = point.MetricTo(lastPoint);

                radius.Should().BeGreaterOrEqualTo(lastRadius, $"on {1000 - pointCount} try");
                pointDifferent.Should().BeGreaterOrEqualTo(lastPointDifferent - 1d, $"on {1000 - pointCount} try");

                lastPoint = point;
                lastRadius = radius;
                lastPointDifferent = pointDifferent;
                if (pointCount-- < 0) break;
            }
        }
    }
}