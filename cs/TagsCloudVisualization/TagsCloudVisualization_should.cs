using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class TagsCloudVisualization_should
    {
        [Test]
        public void DoesNotThrowException_WhenInitWithCorrectArguments()
        {
            var point = new Point(0, 0);

            Action act = () => new CircularCloudLayouter(point);

            act.Should().NotThrow();
        }

        [Test]
        public void ReturnRectangleAtCenter_WhenAddFirstRectangle()
        {
            var center = new Point(0, 0);
            var circularCloudLayouter = new CircularCloudLayouter(center);
            var size = new Size(100, 100);

            var rectangle = circularCloudLayouter.PutNextRectangle(size);

            rectangle.Center.Should().BeEquivalentTo(center);
        }

        [Test]
        public void ArrangeSecondRectangleLeftByFirst_WhenAddTwoRectangles()
        {
            var center = new Point(0, 0);
            var circularCloudLayouter = new CircularCloudLayouter(center);
            var size = new Size(100, 100);
            var expectedCenterOfSecondRect = new Point(size.Width, center.Y);

            circularCloudLayouter.PutNextRectangle(size);
            var rectangle2 = circularCloudLayouter.PutNextRectangle(size);

            rectangle2.Center.Should().BeEquivalentTo(expectedCenterOfSecondRect);
        }
    }
}
