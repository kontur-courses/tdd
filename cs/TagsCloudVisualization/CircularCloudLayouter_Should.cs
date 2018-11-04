using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void Create()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new CircularCloudLayouter(Point.Empty);
        }

        [Test]
        public void NotThrow_WhenAddsRectangles()
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(Point.Empty).PutNextRectangle(Size.Empty));
        }
    }
}
