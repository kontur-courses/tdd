using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouts;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayout_should
    {
        [Test]
        public void PutNextRectangle_FirstRectangle_ShouldBeInCenter()
        {
            var center = new Point();
            var size = new Size(100, 100);
            var expected = new Rectangle(center, size);
            var layout = new CircularCloudLayout(center);

            var result = layout.PutNextRectangle(size);

            result.Should().Be(expected);
        }

        [Test]
        public void PutNextRectangle_SecondRectangle_ShouldNotIntersect_WithFirst()
        {
            var size = new Size(100, 100);
            var center = new Point();
            var layout = new CircularCloudLayout(center);

            var firstRectangle = layout.PutNextRectangle(size);
            var secondRectangle = layout.PutNextRectangle(size);

            firstRectangle.Should().NotBe(secondRectangle);
        }

        [Test]
        public void PutNextRectangle_RectanglesShouldBe_InsideCircle()
        {
            var size = new Size(100, 100);
            var center = new Point();
            var layout = new CircularCloudLayout(center);
            var rectangles = new List<Rectangle>();
            
            for (var i = 0; i < 10; i++)
            {
                rectangles.Add(layout.PutNextRectangle(size));
            }
            var maxRadius = CalculateRadius(rectangles.Last(), center);
            
            rectangles
                .Should()
                .Match(rects => rects.All(rect => CalculateRadius(rect, center) <= maxRadius));
        }

        [Test]
        public void PutNextRectangle_RectanglesSquares_ShouldBeLesser_ThanCircleSquare()
        {
            var size = new Size(100, 100);
            var center = new Point();
            var layout = new CircularCloudLayout(center);
            var rectangles = new List<Rectangle>();
            
            for (int i = 0; i < 10; i++)
            {
                rectangles.Add(layout.PutNextRectangle(size));
            }
            var radius = CalculateRadius(rectangles.Last(), center);
            var circleSquare = Math.PI * radius * radius;
            var squareSum = rectangles.Aggregate(0.0, ((i, rectangle) => i + rectangle.Height * rectangle.Width));
            
            squareSum.Should().BeLessThan(circleSquare * 0.5);
        }

        private double CalculateRadius(Rectangle rect, Point center)
        {
            var xDistance = Math.Abs(rect.X) + rect.Width - center.X;
            var yDistance = Math.Abs(rect.Y) + rect.Height - center.Y;
            return Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
        }
    }
}