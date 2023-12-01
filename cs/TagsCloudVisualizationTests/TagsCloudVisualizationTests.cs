using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter circularCloudLayouter;
        
        [OneTimeSetUp]
        public void CircularCloudLayouterSetUp()
        {
            var point = new Point(0, 0); 
            
            circularCloudLayouter = new CircularCloudLayouter(point);
        }
        
        [Test]
        public void PutNextRectangle_Return_Rectangle()
        {
            circularCloudLayouter.PutNextRectangle().GetType().Should().Be(typeof(Rectangle));
        }
    }
}