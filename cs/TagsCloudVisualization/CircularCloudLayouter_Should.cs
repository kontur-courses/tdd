using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    internal class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = new Point(120,150);
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void HaveCorrectCenter()
        {
            layouter.Center.Should().BeEquivalentTo(center);
        }

        [Test]
        public void PutFirstRectangleCenterOnLayoutCenter()
        {
            var rect = layouter.PutNextRectangle(new Size(48, 24));
            rect.Center().Should().BeEquivalentTo(center);
        }
    }
}
