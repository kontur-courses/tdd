using System;
using System.Drawing;
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
    }
}