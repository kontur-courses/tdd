using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    [TestFixture]
    class CenterMassCheckerTests
    {
        [Test]
        public void CheckCircleForm_Should_ReturnRectangleCenter_When_OneRectangleAdded()
        {
            var centre = new Point(0, 0);
            var tagCloud = new CircularCloudLayouter(centre);
            var rectangles = new List<Rectangle>() { tagCloud.PutNextRectangle(new Size(2, 2)) };
            CenterMassChecker.FindCenterMass(rectangles).Should().Be(new PointF(1, 1));
        }

        [Test]
        public void CheckCircleForm_ShouldReturn_RightVector()
        {
            var centre = new Point(0, 0);
            var tagCloud = new CircularCloudLayouter(centre);
            var rectangles = Enumerable.Range(0, 4).Select(b => tagCloud.PutNextRectangle(new Size(2, 2))).ToList();
            CenterMassChecker.FindCenterMass(rectangles).Should().Be(new PointF(0, 0));
        }
    }
}
