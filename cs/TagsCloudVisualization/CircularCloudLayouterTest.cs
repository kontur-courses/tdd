using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTest
    {
        private CircularCloudLayouter _layouter;
        
        [SetUp]
        public void SetUp()
        {
            Point center = new Point(0, 0);
            _layouter = new CircularCloudLayouter(center);
        }
        
        [Test]
        public void PutNextRectangle_EmptySize_ReturnEmptyRectangle()
        {
            _layouter.PutNextRectangle(Size.Empty).Should().Be(Rectangle.Empty);
        }
    }
}