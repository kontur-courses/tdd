using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;
using FluentAssertions;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class RectangleExtension_Should
    {
        [Test]
        public void ShiftRectangleToBottomLeftCorner_ReturnShiftedRectangle()
        {
            var defaultSize = new Size(200, 100);
            var actualRectangle = new Rectangle(new Point(0, 0), defaultSize);
            var expectedRectangle = new Rectangle(new Point(100, 50), defaultSize);
            actualRectangle.ShiftRectangleToBottomLeftCorner().Should().BeEquivalentTo(expectedRectangle);
        }
    }
}