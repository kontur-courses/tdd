using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public static class PointExtension
    {
        public static Point ShiftToLeftRectangleCorner(this Point point, Size size)
        {
            return new Point(point.X - size.Width / 2, point.Y - size.Height / 2);
        }
    }

    [TestFixture]
    public class PointExtension_Should
    {
        [Test]
        public void ShiftToLeftRectangleCorner_CorrectShift()
        {
            var centerRectanglePoint = new Point(100, 100);
            var rectangleSize = new Size(80, 40);
            centerRectanglePoint.ShiftToLeftRectangleCorner(rectangleSize)
                .Should().Be(new Point(60, 80));
        }
    }
}
