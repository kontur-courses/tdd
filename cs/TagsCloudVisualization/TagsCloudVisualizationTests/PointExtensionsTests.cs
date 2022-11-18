using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Utils;

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

        [TestCaseSource(nameof(PointF_GetDistanceTo_TestData))]
        public void PointF_GetDistanceTo_WorksCorrectly(PointF a, PointF b, double expectedDistance)
        {
            var distance = a.GetDistanceTo(b);

            distance.Should().BeApproximately(expectedDistance, expectedDistance * .001);
        }
    }
}