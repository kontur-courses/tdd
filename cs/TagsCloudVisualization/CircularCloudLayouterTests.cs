using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private Point center;

        [SetUp]
        public void SetUp()
        {
            center = Point.Empty;
            layouter = new CircularCloudLayouter(center);
        }

        [Test]
        public void SaveCenterAfterCreation()
        {
            layouter.Center.Should().Be(center);
        }

        [Test]
        public void NoRectanglesAfterCreation()
        {
            layouter.Rectangles.Should().BeEmpty();
        }

        [TestCase(0, 0)]
        [TestCase(-1, -1)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void PutNextRectangle_ThrowArgumentException(int width, int height)
        {
            var size = new Size(width, height);
            Action act = () => layouter.PutNextRectangle(size);
            
            act.Should().Throw<ArgumentException>();
        }
        
        [Test]
        public void PutNextRectangle_CenterLocation_IfPutFirstRectangle()
        {
            var size = new Size(1, 1);
            
            layouter.PutNextRectangle(size).Location
                .Should().Be(center);
        }

        [Test]
        public void PutNextRectangle_DoesNotChangeSize()
        {
            var size = new Size(1, 1);

            layouter.PutNextRectangle(size).Size
                .Should().Be(size);
        }

        [TestCase(2)]
        [TestCase(5)]
        [TestCase(50)]
        public void SaveRectangleAfterPutNextRectangle(int count)
        {
            var rectSizes = Enumerable.Range(0, count)
                .Select(x => new Size(1, count))
                .ToArray();

            foreach (var size in rectSizes)
                layouter.PutNextRectangle(size);

            layouter.Rectangles.Select(x => x.Size)
                .Should().Contain(rectSizes);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        public void RectanglesShouldNotBeIntersected(int count)
        {
            var rectSizes = Enumerable.Range(1, count)
                .Select(x => new Size(x, x))
                .ToArray();
            var rects = new List<Rectangle>();
            
            foreach (var size in rectSizes)
                rects.Add(layouter.PutNextRectangle(size));

            Enumerable.Range(0, count - 1)
                .Select(idx => rects.Take(idx).IntersectsWith(rects[idx + 1]))
                .Any(x => x)
                .Should().BeFalse();
        }
    }
}