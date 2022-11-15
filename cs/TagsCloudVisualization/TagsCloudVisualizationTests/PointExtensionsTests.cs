using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class PointExtensionsTests
    {
        private static IEnumerable<TestCaseData> PointF_GetDistanceTo_TestData => new[]
        {
            new TestCaseData(new PointF(0, 0), new PointF(0, 0), 0),
            new TestCaseData(new PointF(0, 0), new PointF(5, 0), 5),
            new TestCaseData(new PointF(0, 0), new PointF(3, 4), 5),
            new TestCaseData(new PointF(0, 0), new PointF(10, 10), Math.Sqrt(10 * 10 + 10 * 10)),
            new TestCaseData(new PointF(1, 1), new PointF(6, 1), 5),
            new TestCaseData(new PointF(1.5f, 1.5f), new PointF(6, 6), Math.Sqrt(4.5 * 4.5 + 4.5 * 4.5))
        };

        [Test]
        public void ScatterPointsSpirally_PlacesEachPointFurtherFromCenter()
        {
            var center = new PointF(0, 0);

            var spiral = center.ScatterPointsBySpiralAround(5).Take(1000).ToList();

            var lastDistance = 0d;
            for (var i = 0; i < spiral.Count; i++)
            {
                var currentDistance = spiral[i].GetDistanceTo(center);
                currentDistance.Should().BeGreaterThanOrEqualTo(lastDistance);
                lastDistance = currentDistance;
            }
        }

        [Test]
        public void ScatterPointsSpirally_PlacesConsecutivePointsWithApproximatelySpecifiedSpacing()
        {
            var center = new PointF(0, 0);
            var approximatePointSpacing = 5;
            var maxRelativeError = 0.2;

            var spiral = center.ScatterPointsBySpiralAround(approximatePointSpacing).Take(1000).ToList();

            for (var i = 1; i < spiral.Count; i++)
                spiral[i].GetDistanceTo(spiral[i - 1]).Should().BeApproximately(approximatePointSpacing,
                    approximatePointSpacing * maxRelativeError);
        }

        [Test]
        public void ScatterPointsSpirally_DoesntPlaceAnyPointsTooClose()
        {
            var center = new PointF(0, 0);
            var approximatePointSpacing = 5;
            var maxRelativeError = 0.2;

            var spiral = center.ScatterPointsBySpiralAround(approximatePointSpacing).Take(1000).ToList();

            for (var i = 0; i < spiral.Count; i++)
            for (var j = i + 1; j < spiral.Count; j++)
                spiral[i].GetDistanceTo(spiral[j]).Should().BeGreaterThan(
                    approximatePointSpacing * (1 - maxRelativeError));
        }

        [TestCaseSource(nameof(PointF_GetDistanceTo_TestData))]
        public void PointF_GetDistanceTo_WorksCorrectly(PointF a, PointF b, double expectedDistance)
        {
            var distance = a.GetDistanceTo(b);

            distance.Should().BeApproximately(expectedDistance, expectedDistance * .001);
        }
    }
}