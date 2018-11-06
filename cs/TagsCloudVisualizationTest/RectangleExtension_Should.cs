using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTest
{
    class RectangleExtension_Should
    {
        [Test]
        public void GetCenter_ReturnExactCenter_WhenItHasIntegerCoordinates()
        {
            Rectangle rectangle = new Rectangle(1, 1, 10, 10);
            Point expectedCenter = new Point(6, 6);

            Assert.AreEqual(expectedCenter, rectangle.GetCenter());
        }

        [Test]
        public void GetCenter_ReturnNearToCenter_WhenItDoesNotHasIntegerCoordinates()
        {
            Rectangle rectangle = new Rectangle(1, 1, 9, 9);
            PointF exactCenter = new PointF(5.5f, 5.5f);

            PointF actualCenter = rectangle.GetCenter();

            Assert.AreEqual(exactCenter.X, actualCenter.X, 1);
            Assert.AreEqual(exactCenter.Y, actualCenter.Y, 1);
        }
    }
}
