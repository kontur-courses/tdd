using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture] 
    public class CircularCloudLayouterConstructorTests
    { 
        [Test] 
        public void CloudLayouterConstructorShouldWorkCorrectly() 
        { 
            var center = new Point(15, 20); 
            Action creating = () => new CircularCloudLayouter(center);
            creating.Should().NotThrow();
        }

        [TestCase(-5, 10)]
        [TestCase(5, -5)]
        [TestCase(0, 0)]
        [TestCase(10, 10)]
        public void ShouldNotThrowExceptionWithAnySize(int width, int height)
        {
            var layouterCenter = new Point(width, height);
            Action creating = () => new CircularCloudLayouter(layouterCenter);
            creating.Should().NotThrow();
        }
    }
}
