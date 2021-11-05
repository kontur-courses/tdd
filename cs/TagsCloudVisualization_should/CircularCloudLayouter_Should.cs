using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class CircularCloudLayouter_Should
    {
        private readonly Point center = new Point(0, 0);

        [TestCase(0,3, TestName = "width is zero")]
        [TestCase(3,0, TestName = "height is zero")]
        [TestCase(-1,3, TestName = "width is negative")]
        [TestCase(3,-1, TestName = "height is negative")]
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
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(100)]
        public void CreateExpectedNumberOfRectangles(int number)
        {
            var layouter = new CircularCloudLayouter(center);
            var size = new Size(5, 5);
            for (int i = 1; i <= number; i++)
            {
                var rect = layouter.PutNextRectangle(size);
            }

            layouter.Rectangles.Count.Should().Be(number);
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(100)]
        public void PutNewRectangleNotIntersectedWithOthers(int number)
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            var size = new Size(5, 5);
            for (int i = 1; i <= number; i++)
            {
                var rect = layouter.PutNextRectangle(size);
                rectangles.Any(r => r.IntersectsWith(rect)).Should().BeFalse();
                rectangles.Add(rect);
            }
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(100)]
        public void PutRandomSizeRectanglesNotIntersectedWithOthers(int number)
        {
            var layouter = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            var rnd = new Random();
            for (int i = 1; i <= number; i++)
            {
                var size = new Size(rnd.Next(1, 10), rnd.Next(1,10));
                var rect = layouter.PutNextRectangle(size);
                rectangles.Any(r => r.IntersectsWith(rect)).Should().BeFalse();
                rectangles.Add(rect);
            }
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void CreateReactanglesNotTooLong(int number)
        {
            var layouter = new CircularCloudLayouter(center);
            var size = new Size(5, 5);
            Action act = () =>
            {
                for (int i = 1; i <= number; i++)
                {
                    layouter.PutNextRectangle(size);
                }
            };
            act.ExecutionTime().Should().BeLessOrEqualTo(new TimeSpan(0,0,0,5));
        }
    }
}