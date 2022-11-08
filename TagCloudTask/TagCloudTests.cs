using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagCloud
{
    public class TagCloudTests
    {
        [Test,Timeout(2000)]
        public void PerformanceTest()
        {
            var tagCloudBuilder = new TagCloudBuilder(new Size(1000, 1000));
            var layouter = new CircularCloudLayouter(new Point(500, 500), tagCloudBuilder);

            for (int i = 0; i < 500; i++)
                layouter.PutNextRectangle(new Size(100, 40));
        }
        
        public void AssertAddingRectangles()
        {
            var tagCloudBuilder = new TagCloudBuilder(new Size(1000, 1000));
            var layouter = new CircularCloudLayouter(new Point(500, 500), tagCloudBuilder);

            for (int i = 0; i < 500; i++)
                layouter.PutNextRectangle(new Size(100, 40));

            tagCloudBuilder.Rectangles.Count.Should().Be(500);
        }
        
        [Test]
        public void ThrowsOnBadSize()
        {
            var tagCloudBuilder = new TagCloudBuilder(new Size(1000, 1000));
            var layouter = new CircularCloudLayouter(new Point(500, 500), tagCloudBuilder);

            Action action = () => layouter.PutNextRectangle(new Size(0, -1));
            action.Should().Throw<ArgumentException>();
        }
    }
}