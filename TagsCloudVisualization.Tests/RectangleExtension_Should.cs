using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class RectangleExtension_Should
    {
        [Test]
        public void returnFalse_forСoncerningRectangles()
        {
            var rect = new Rectangle(0, 0, 100, 200);
            var rectangles = new List<Rectangle>()
            {
                new Rectangle(100, 10, 10, 23)
            };
            rect.IntersectsWith(rectangles).Should().Be(false);
        }

        [Test]
        public void returnTrue_forIntersectionrectangles()
        {
            var rect = new Rectangle(100, 100, 100, 200);
            var rectangles = new List<Rectangle>()
            {
                new Rectangle(90, 90, 9, 9),
                new Rectangle(170, 170, 50, 100)
            };
            rect.IntersectsWith(rectangles).Should().Be(true);
        }

        [Test]
        public void returnFalse_forNoIntersectionrectangles()
        {
            var rect = new Rectangle(100, 100, 100, 200);
            var rectangles = new List<Rectangle>()
            {
                new Rectangle(90, 90, 9, 9),
                new Rectangle(201, 170, 50, 100)
            };
            rect.IntersectsWith(rectangles).Should().Be(false);
        }

        [Test]
        public void ReturnCorrectMaxDistanceToPoint()
        {
            var point = new Point(10, 10);
            var rectangle = new Rectangle(20, 20, 10, 10);
            rectangle.MaxDistanceToPoint(point).Should().Be(20 * Math.Sqrt(2));
        }
    }
}
