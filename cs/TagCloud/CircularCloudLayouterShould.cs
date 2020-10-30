using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FluentAssertions;

namespace TagCloud
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        [Test]
        public void PutNextRectangle_ReturnRectangleWithCorrectSize()
        {
            var layouter = new CircularCloudLayouter(new Point(0, 0));
            var size = new Size(10, 10);
            layouter.PutNextRectangle(size).Size.Should().Be(size);
        }
        
        [Test]
        public void PutNextRectangle_ReturnNotIntersectedRectangles()
        {
            var layouter  = new CircularCloudLayouter(new Point(0, 0));
            var firstRectangle = layouter.PutNextRectangle(new Size(10, 10));
            var secondRectangle = layouter.PutNextRectangle(new Size(10, 10));
            firstRectangle.IntersectsWith(secondRectangle).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ManyRectangles_ReturnNotIntersectRectangles()
        {
            var layouter  = new CircularCloudLayouter(new Point(0, 0));
            var size = new Size(2, 1);
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 10; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }

        [Test]
        public void PutNextRectangles_LayoutRectanglesInCircle()
        {
            var center = new Point(0, 0);
            var layouter  = new CircularCloudLayouter(center);
            var size = new Size(2, 1);
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 10; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            foreach (var rectangle in rectangles)
            {
                GetDistance(rectangle.Location, center).Should().BeLessThan(5);
            }
        }
        
        private double GetDistance(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}