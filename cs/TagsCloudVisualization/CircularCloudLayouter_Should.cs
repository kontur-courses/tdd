using NUnit.Framework;
using FluentAssertions;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private Point layouterCenter = new Point(0, 0);
        private CircularCloudLayouter layouter;
        
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(layouterCenter);
        }

        [Test]
        public void Should_AddFirstRectangleWithCorrectPos()
        {
            var rectSize = new Size(100, 10);
            var expectedRect = new Rectangle(layouterCenter, rectSize);
            layouter.PutNextRectangle(rectSize).Should().BeEquivalentTo(expectedRect);
        }
    }
}