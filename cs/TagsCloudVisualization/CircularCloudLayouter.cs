using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Drawing;
using FluentAssertions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                rectangles.Add(new Rectangle(Center.X - rectangleSize.Width / 2,
                    Center.Y - rectangleSize.Height / 2,
                    rectangleSize.Width, rectangleSize.Height));
            else if (rectangles.Count == 1)
            {
                rectangles.Add(new Rectangle(Center.X - rectangleSize.Width / 2, rectangles[0].Y - rectangleSize.Height,
                    rectangleSize.Width, rectangleSize.Height));
            }
            return rectangles[rectangles.Count - 1];
        }
    }

    [TestFixture]
    public class CircluarCloudLayouterTests
    {
        [Test]
        public void FirstRectangle_ShouldBeInCenter()
        {
            var center = new Point(200, 200);
            var layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(new Size(100, 70))
                .Should().BeEquivalentTo(new Rectangle(150, 165, 100, 70));
        }

        [Test]
        public void SecondRectangle_ShouldBeAboveTheFirst()
        {
            var center = new Point(200, 200);
            var layouter = new CircularCloudLayouter(center);
            layouter.PutNextRectangle(new Size(100, 70));
            layouter.PutNextRectangle(new Size(70, 50))
                .Should().BeEquivalentTo(new Rectangle(165, 115, 70, 50));
        }
    }
}
