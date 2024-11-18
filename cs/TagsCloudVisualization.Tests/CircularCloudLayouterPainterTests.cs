using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    internal class CircularCloudLayouterPainterTests
    {
        private int _defaultPadding = 10;

        [Test]
        public void Draw_ThrowsArgumentException_OnEmptyRectangleList()
        {
            Assert.Throws<ArgumentException>(
                () => CircularCloudLayouterPainter.Draw(new List<Rectangle>()));
        }

        [TestCase(null, ExpectedResult = true)]
        [TestCase(100, ExpectedResult = true)]
        public bool Draw_CalculatesImageSizeCorrectly(int? padding)
        {
            var correctPadding = padding ?? _defaultPadding;
            var rectangles = new List<Rectangle>()
            {
                new Rectangle(new Point(0, 0), new Size(10, 10))
            };

            var image = CircularCloudLayouterPainter.Draw(rectangles, padding);
            return image.Height == rectangles[0].Height + 2 * correctPadding
                   && image.Width == rectangles[0].Width + 2 * correctPadding;
        }
    }
}