using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter _circularCloudLayouter;

        [SetUp]
        public void StartUp()
        {
            _circularCloudLayouter = new CircularCloudLayouter(new Point());
        }
        
        [TestCase(0, 0, 2, 2, 1, 1)]
        public void GetRectangleCenter_ReturnsCorrectPoint(int x,
                                                            int y,
                                                            int width,
                                                            int height,
                                                            int centerX,
                                                            int centerY)
        {
            var rectangle = new Rectangle(x, y, width, height);
            var expectedCenter = new Point(centerX, centerY);
            var actualCenter = _circularCloudLayouter.GetRectangleCenter(rectangle);
            Assert.AreEqual(expectedCenter, actualCenter);
        }
    }
}