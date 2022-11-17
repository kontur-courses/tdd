using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class SpiralPointScattererTests
    {
        private static readonly PointF Center = new(0, 0);
        private static readonly int ApproximatePointSpacing = 5;
        private static List<PointF> spiralUnderTesting;

        [SetUp]
        public void SetUp()
        {
            spiralUnderTesting = SpiralPointScatterer.Scatter(Center, ApproximatePointSpacing)
                .Take(1000).ToList();
        }

        [Test]
        public void Scatter_PlacesEachPointFurtherFromCenter()
        {
            var lastDistance = 0d;
            for (var i = 0; i < spiralUnderTesting.Count; i++)
            {
                var currentDistance = spiralUnderTesting[i].GetDistanceTo(Center);
                currentDistance.Should().BeGreaterThanOrEqualTo(lastDistance);
                lastDistance = currentDistance;
            }
        }

        [Test]
        public void Scatter_PlacesConsecutivePointsWithApproximatelySpecifiedSpacing()
        {
            var maxRelativeError = 0.2;

            var spiral = SpiralPointScatterer.Scatter(Center, ApproximatePointSpacing)
                .Take(1000)
                .ToList();

            for (var i = 1; i < spiral.Count; i++)
                spiral[i].GetDistanceTo(spiral[i - 1]).Should().BeApproximately(
                    ApproximatePointSpacing,
                    ApproximatePointSpacing * maxRelativeError);
        }

        [Test]
        public void Scatter_DoesntPlaceAnyPointsTooClose()
        {
            var maxRelativeError = 0.2;

            for (var i = 0; i < spiralUnderTesting.Count; i++)
            for (var j = i + 1; j < spiralUnderTesting.Count; j++)
                spiralUnderTesting[i].GetDistanceTo(spiralUnderTesting[j]).Should().BeGreaterThan(
                    ApproximatePointSpacing * (1 - maxRelativeError));
        }
    }
}