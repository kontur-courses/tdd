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
    class CircularCloudLayoterTests
    {
        [Test]
        public void ShouldCreateCircularCloud()
        {
            var center = new Point(15, 20);
            Action creating = () => new CircularCloudLayouter(center);
            creating.Should().NotThrow();
        }

        [Test]
        public void ShouldCreateCenterCorrectly()
        {
            var center = new Point(10, 30);
            var layouter =  new CircularCloudLayouter(center);
            layouter.Center.Should().Be(new Point(10,30));
        }

        [Test]
        public void ShouldPutOneRectangleInRightLocation()
        {
            var rectangleSize = new Size(30, 10);
            var layouterCenter = new Point(20, 10);
            var layouter = new CircularCloudLayouter(layouterCenter);
            var rectangle = layouter.PutNextRectangle(rectangleSize);
            rectangle.Location.Should().Be(new Point(5, 5));
        }
    }
}
