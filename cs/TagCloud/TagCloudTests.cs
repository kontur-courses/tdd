using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    public class TagCloudTests
    {
        private CircularCloudLayouter cloudLayouter;

        [SetUp]
        public void PrepareCircularCloudLayouter()
        {
            cloudLayouter = new CircularCloudLayouter();
        }

        [TestCase(0, 0, 35, 75, TestName = "center in zero point")]
        [TestCase(300, 300, 5, 5, TestName = "center in positive point")]
        [TestCase(-300, 300, 5, 5, TestName = "center in X negative point")]
        [TestCase(300, -300, 5, 5, TestName = "center in Y negative point")]
        [TestCase(-300, -300, 5, 5, TestName = "center in XY negative point")]
        public void PutNextRectangle_FirstRectangleMustBeInCenterOfCloud_When(int centerX, int centerY, int reactWidth, int reactHeight)
        {
            cloudLayouter = new CircularCloudLayouter(new Point(centerX, centerY));

            var rectangle = cloudLayouter.PutNextRectangle(new Size(reactWidth, reactHeight));
            var planningReactLocation = new Point(centerX - reactWidth / 2, centerY - reactHeight / 2);

            rectangle.Location.Should().BeEquivalentTo(planningReactLocation);

            var tagCloud = cloudLayouter.GetTagCloud();

            tagCloud.GetHeight().Should().Be(reactHeight);
            tagCloud.GetWidth().Should().Be(reactWidth);
            tagCloud.GetLeftBound().Should().Be(planningReactLocation.X);
            tagCloud.GetTopBound().Should().Be(planningReactLocation.Y);
        }
    }
}
