namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    internal class CircularCloudLayouterWorkerTests
    {
        [TestCase(0, 100)]
        [TestCase(-1, 100)]
        [TestCase(100, 0)]
        [TestCase(100, -1)]
        public void GetNextRectangleSize_ThrowsArgumentException_OnAnyNegativeOrZeroSize(int width, int height)
        {
            Assert.Throws<ArgumentException>(
                () => CircularCloudLayouterWorker.GetNextRectangleSize(width, width, height, height));
        }

        [TestCase(50, 25, 25, 50)]
        [TestCase(25, 50, 50, 25)]
        public void GetNextRectangleSize_ThrowsArgumentException_OnNonConsecutiveSizeValues(
            int minWidth,
            int maxWidth,
            int minHeight,
            int maxHeight)
        {
            Assert.Throws<ArgumentException>(
                () => CircularCloudLayouterWorker.GetNextRectangleSize(minWidth, maxWidth, minHeight, maxHeight));
        }
    }
}