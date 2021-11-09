using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouter_Should
    {
        private readonly Point center = new Point(0, 0);

        [TestCase(0, 3, TestName = "width of rectangle is not expected to be zero")]
        [TestCase(3, 0, TestName = "height of rectangle is not expected to be zero")]
        [TestCase(-1, 3, TestName = "width of rectangle is not expected to be negative")]
        [TestCase(3, -1, TestName = "height of rectangle is not expected to be zero")]
        public void Throw_WhenSizeIsIncorrect(int width, int height)
        {
            var layouter = new CircularCloudLayouter(center);
            var size = new Size(width, height);
            Action act = () => layouter.PutNextRectangle(size);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PutFirstRectangleInCenter()
        {
            var layouter = new CircularCloudLayouter(center);
            var size = new Size(5, 5);
            layouter.PutNextRectangle(new Size(5, 5)).Should().Be(new Rectangle(center, size));
        }

        [TestCase(1)]
        [TestCase(100)]
        public void CreateExpectedNumberOfRectangles(int rectanglesCount)
        {
            var layouter = new CircularCloudLayouter(center);
            var size = new Size(5, 5);
            for (var i = 1; i <= rectanglesCount; i++)
                layouter.PutNextRectangle(size);


            layouter.Rectangles.Count.Should().Be(rectanglesCount);
        }

        [Test]
        public void PutNewRectangleNotIntersectedWithOthers()
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            var size = new Size(5, 5);
            var rectanglesCount = 100;
            for (var i = 1; i <= rectanglesCount; i++)
            {
                var rect = layouter.PutNextRectangle(size);
                rectangles.Any(r => r.IntersectsWith(rect)).Should().BeFalse();
                rectangles.Add(rect);
            }
        }

        [Test]
        public void PutRandomSizeRectanglesNotIntersectedWithOthers()
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            var rnd = new Random();
            var rectanglesCount = 100;
            for (var i = 1; i <= rectanglesCount; i++)
            {
                var size = new Size(rnd.Next(1, 10), rnd.Next(1, 10));
                var rect = layouter.PutNextRectangle(size);
                rectangles.Any(r => r.IntersectsWith(rect)).Should().BeFalse();
                rectangles.Add(rect);
            }
        }

        [Test]
        [Timeout(5000)]
        public void CreateRectanglesNotTooLong()
        {
            var rectanglesCount = 10000;
            var layouter = new CircularCloudLayouter(center);
            var size = new Size(5, 5);
            for (var i = 1; i <= rectanglesCount; i++) layouter.PutNextRectangle(size);
        }
    }
}