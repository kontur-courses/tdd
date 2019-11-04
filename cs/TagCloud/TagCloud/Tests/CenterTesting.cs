using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud.Tests
{
    internal class CenterTesting
    {
        [TestCaseSource(nameof(_nonCoordinateCenter))]
        public void Should_CorrectPlaceInstance_WithDifferentCenterPoints(Point center)
        {
            Action a = () => new CircularCloudLayouter(center);
            a.Should().NotThrow();
        }

        [TestCaseSource(nameof(_nonCoordinateCenter))]
        public void Should_LocateFirstRectangle_OnSpecifiedByCenter(Point center)
        {
            var layouter = new CircularCloudLayouter(center);
            var rect = layouter.PutNextRectangle(new Size(2, 2));
            (rect.Y + rect.Height / 2)
                .Should()
                .Be(center.Y, "Y coords not equal");
            (rect.X + rect.Width / 2)
                .Should()
                .Be(center.X, "X coords not equal");
        }
        

        private static IEnumerable<TestCaseData> _nonCoordinateCenter = new List<TestCaseData>
        {
            new TestCaseData(new Point(-1, 0)).SetName("{m}: x: -1, y = 0"),
            new TestCaseData(new Point(-1, 1)).SetName("{m}: x: -1, y = 1"),
            new TestCaseData(new Point(-1, -1)).SetName("{m}: x: -1, y = -1"),
            new TestCaseData(new Point(0, 0)).SetName("{m}: x: 0, y = 0"),
            new TestCaseData(new Point(0, 1)).SetName("{m}: x: 0, y = 1"),
            new TestCaseData(new Point(0, -1)).SetName("{m}: x: 0, y = -1"),
            new TestCaseData(new Point(1, 0)).SetName("{m}: x: 1, y = 0"),
            new TestCaseData(new Point(1, 1)).SetName("{m}: x: 1, y = 1"),
            new TestCaseData(new Point(1, -1)).SetName("{m}: x: 1, y = -1"),
        };

    }
}