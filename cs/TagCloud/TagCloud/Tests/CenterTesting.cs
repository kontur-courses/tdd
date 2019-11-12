using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud.Tests
{
    internal class CenterTesting : OnFailDrawer
    {
        [TestCaseSource(nameof(coordinateCenter))]
        public void Should_LocateFirstRectangleCenter_OnSpecifiedPoint(Point center)
        {
            var layouter = new CircularCloudLayouter(center);
            var rect = layouter.PutNextRectangle(new Size(2, 2));
            (rect.X + rect.Width / 2)
                .Should()
                .Be(center.X, "X coords not equal");
            (rect.Y + rect.Height / 2)
                .Should()
                .Be(center.Y, "X coords not equal");
        }


        private static IEnumerable<TestCaseData> coordinateCenter = new List<TestCaseData>
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