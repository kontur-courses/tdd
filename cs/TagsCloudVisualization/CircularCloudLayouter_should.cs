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
        
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(10, 10));
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
            var sizes = new List<Size>() {new Size(10, 5), 
                new Size(100, 10), new Size(10, 5), 
                new Size(15, 15)};
            
            foreach (var size in sizes)
                layouter.PutNextRectangle(size);

            layouter.Rectangles.Select(rect => rect.Size)
                .Should().BeEquivalentTo(sizes);
        }

    }
}