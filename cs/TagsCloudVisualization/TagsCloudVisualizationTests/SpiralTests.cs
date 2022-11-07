using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.TagsCloudVisualizationTests
{
    [TestFixture]
    internal class SpiralTests
    {
        [Test]
        public void spiralInitialization_PropertyCenterEqualParamCenter_OnCorrectValue()
        {
            var center = new Point(500, 500);
            var spr = new Spiral(center);
            spr.Center.Should().BeEquivalentTo(center);
        }

        [TestCase(0.0, 0.0)]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(0.1, 0)]

        public void spiralInitialization_ThrowsArgumentException_OnIncorrectValue(
            double step, double parameter)
        {
            Assert.Throws<ArgumentException>(() => new Spiral(new Point(4, 4), step, parameter));
        }

        [TestCase(0.1, 2, new int[]
        { 400, 399, 399, 399, 400, 400, 401, 401, 402, 402 }, new int[]
        { 401, 401, 400, 399, 399, 398, 398, 399, 399, 400 })]

        [TestCase(0.2, 1, new int[]
        { 399, 400, 400, 401, 401, 401, 400, 399, 399, 398 }, new int[]
        { 400, 400, 399, 399, 400, 401, 401, 401, 400, 400 })]

        [TestCase(-10, Math.PI, new int[]
        { 404, 396, 398, 413, 376, 429, 378, 404, 420, 357 }, new int[]
        { 397, 409, 385, 415, 393, 391, 427, 360, 440, 375 })]
        public void GetPoints_ShouldReturnSpiralPoints_OnCorrectValue(
            double step, double parameter, int[] x, int[] y)
        {
            var points = new List<Point>();
            var expectedPoints = new Point[10];

            for (int i = 0; i < x.Length; i++)
            {
                expectedPoints[i] = new Point(x[i], y[i]);
            }

            var spiral = new Spiral(new Point(400, 400), step, parameter);
            spiral.GetPoints(10).Should().BeEquivalentTo(expectedPoints);
        }
    }
}
