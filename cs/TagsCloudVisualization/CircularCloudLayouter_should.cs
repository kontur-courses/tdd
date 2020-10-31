using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_should
    {
        private CircularCloudLayouter layouter;
        private List<Size> sizes;
        
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(10, 10));
            sizes = new List<Size>() {new Size(10, 5), 
                new Size(100, 10), new Size(10, 5), 
                new Size(15, 15)};

        }
        
        [Test]
        [TestCase(15, 10)]
        public void Layouter_ShouldReturnRectangleWithSameSize_WhenPuttingNextRectangle(int width, int height)
        {
            var size = new Size(width, height);
            layouter.PutNextRectangle(size).Size
                .Should().BeEquivalentTo(size);
        }
        
        [Test]
        public void Layouter_ShouldContainsAllRectangles_WhenSomeRectangleSizesAdded()
        {
            fillLayouterWithSomeRectangles(layouter, sizes);

            layouter.Rectangles.Select(rect => rect.Size)
                .Should().BeEquivalentTo(sizes);
        }
        
        [Test]
        public void LayouterRectangles_ShouldNotIntersectEachOther_WhenSomeRectanglesAdded()
        {
            fillLayouterWithSomeRectangles(layouter, sizes);

            foreach (var rectangle in layouter.Rectangles)
            {
                foreach (var otherRectangle in layouter.Rectangles)
                {
                    if (rectangle != otherRectangle)
                        rectangle.IntersectsWith(otherRectangle)
                            .Should().BeFalse();
                }
            }
        }

        private void fillLayouterWithSomeRectangles(CircularCloudLayouter layouter,
            List<Size> rectangleSizes)
        {
            foreach (var size in rectangleSizes)
                layouter.PutNextRectangle(size);
        }
    }
}