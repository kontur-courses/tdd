using System.Drawing;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    internal class CircularCloudLayouterTests
    {
        [TestCase(0, 100)]
        [TestCase(-1, 100)]
        [TestCase(100, 0)]
        [TestCase(100, -1)]
        public void PutNextRectangle_ThrowsArgumentException_OnAnyNegativeOrZeroSize(
            int width,
            int height)
        {
            var size = new Size(width, height);
            Assert.Throws<ArgumentException>(
                () => new CircularCloudLayouter(new Point()).PutNextRectangle(size));
        }
    }
}