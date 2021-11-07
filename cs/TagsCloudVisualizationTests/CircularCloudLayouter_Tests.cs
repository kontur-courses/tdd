using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private List<Rectangle> rectangles;
        private CircularCloudLayouter layouter;
        private Point center;
        private SizeGenerator generator;

        [SetUp]
        public void Setup()
        {
            generator = new SizeGenerator(10, 25, 10, 25);
            center = Point.Empty;
            rectangles = new List<Rectangle>();
            layouter = new CircularCloudLayouter(center);
        }

        [TestCase(10, 0, TestName = "HeightIsZero")]
        [TestCase(0, 10, TestName = "WidthIsZero")]
        [TestCase(-10, 10, TestName = "WidthIsNegative")]
        [TestCase(10, -10, TestName = "HeightIsNegative")]
        public void PutNextRectangle_ThrowsException_WhenSizeIsInvalid(int width, int height)
        {
            var size = new Size(width, height);
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(size));
        }

        [Test]
        public void PutNextRectangle_LocationOfFirstRectangleIsCenter()
        {
            rectangles.Add(layouter.PutNextRectangle(new Size(10, 10)));

            rectangles.Should().ContainSingle(rect => rect.Location == center);
        }

        [TestCase(10, TestName = "WhenPut10Rectangles")]
        [TestCase(100, TestName = "WhenPut100Rectangles")]
        public void PutNextRectangle_HaveExactNumberOfRectangles(int count)
        {
            foreach (var size in generator.GenerateSizes(count))
            {
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            rectangles.Should().HaveCount(count);
        }

        [TestCase(50, TestName = "WhenPut50Rectangles")]
        [TestCase(100, TestName = "WhenPut100Rectangles")]
        public void PutNextRectangle_RectanglesDoNotIntersect(int count)
        {
            foreach (var size in generator.GenerateSizes(count))
            {
                rectangles.Add(layouter.PutNextRectangle(size));
            }
            
            rectangles.Any(rect => rectangles.Where(r => r != rect).Any(rect.IntersectsWith)).Should().BeFalse();
        }
    }
}