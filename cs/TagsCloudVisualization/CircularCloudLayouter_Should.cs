using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void CircularCloudLayouter_ShouldThrowArgumentException_WhenNegativeX_Y()
        {
            Action act = () =>
            {
                var cloudLayouter = new CircularCloudLayouter(new Point(-1, -1));
            };
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangleInCenter_WhenOneRectangle()
        {
            var center = new System.Drawing.Point(400, 250);
            var cloudLayouter = new CircularCloudLayouter(center);
            var rectangleSize = new Size(200, 30);
            var point = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
            var expectedRect = new Rectangle(point, rectangleSize);

            var rect = cloudLayouter.PutNextRectangle(rectangleSize);

            rect.Should().BeEquivalentTo(expectedRect);
        }
    }
}
