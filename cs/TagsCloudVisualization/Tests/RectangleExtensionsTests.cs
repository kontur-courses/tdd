using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    class RectangleExtensionsTests
    {
        [Test]
        public void IntersectsWith_True_WhenEnumerableContainsIntersectingRectangle()
        {
            var rectangles = new Rectangle[]
            {
                new Rectangle(-1, -1, 2, 2)
            };
            var rectangle = new Rectangle(0, 0, 2, 2);
            rectangle.IntersectsWith(rectangles).Should().BeTrue();
        }

        [Test]
        public void IntersectsWith_False_WhenEnumerableDoNotContainsIntersectingRectangle()
        {
            var rectangles = new Rectangle[]
            {
                new Rectangle(-1, -1, 2, 2)
            };
            var rectangle = new Rectangle(1, 1, 2, 2);
            rectangle.IntersectsWith(rectangles).Should().BeFalse();
        }

        [Test]
        public void GetMiddlePoint_ReturnCorrectPoint()
        {
            var rectangle = new Rectangle(0,0, 4, 4);
            var expectedPoint = new Point(2, 2);
            rectangle.GetMiddlePoint().Should().BeEquivalentTo(expectedPoint);
        }
    }
}
