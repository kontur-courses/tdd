using System;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {

        public CircularCloudLayouter(Point center)
        {
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void Test()
        {
        }
    }
}
