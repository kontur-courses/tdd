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
        private readonly Random _random = new Random();
        [Test] 
        public void CloudLayouterConstructorShouldWorkCorrectly() 
        { 
            var center = new Point(_random.Next(-100,100),  _random.Next(-100, 100)); 
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
