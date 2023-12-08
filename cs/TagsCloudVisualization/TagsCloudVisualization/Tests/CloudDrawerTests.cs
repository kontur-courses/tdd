using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;

namespace Tests
{
    [TestFixture]
    public class CloudDrawerTests
    {
        private static IEnumerable<TestCaseData> DrawArgumentException => new[]
        {
            new TestCaseData(new Cloud (new Point(0,0), new List<Rectangle>(){new Rectangle(1, 1, 1, 1)}), 0, 1)
            .SetName("WhenGivenNotPositiveWidth"),
            new TestCaseData(new Cloud (new Point(0,0), new List<Rectangle>(){new Rectangle(1, 1, 1, 1)}), 1, 0)
            .SetName("WhenGivenNotPositiveHeigth"),
            new TestCaseData(new Cloud (new Point(0,0), new List<Rectangle>(){new Rectangle(1, 1, 1, 1)}), 0, 0)
            .SetName("WhenGivenNotPositiveHeigthAndWidth"),
            new TestCaseData(new Cloud (new Point(0,0), new List<Rectangle>()), 1, 1)
            .SetName("WhenGivenCloudWithEmptyArray")
        };
 
        [TestCaseSource(nameof(DrawArgumentException))]
        public void Draw_ShouldThrowArgumentException(Cloud cloud, int width, int height) =>
            Assert.Throws<ArgumentException>(() => CloudDrawer.DrawCloud(cloud, width, height));

        private static IEnumerable<TestCaseData> DrawNoException => new[]
        {
            new TestCaseData(new Cloud (new Point(5,5), new List<Rectangle>(){new Rectangle(5, 5, 20, 3)}), 10, 10)
            .SetName("WhenCloudWidthIsGreaterThanImageWidth"),
            new TestCaseData(new Cloud (new Point(5,5), new List<Rectangle>(){new Rectangle(5, 5, 3, 20)}), 10, 10)
            .SetName("WhenCloudHeightIsGreaterThanImageHeight"),
            new TestCaseData(new Cloud (new Point(5,5), new List<Rectangle>(){new Rectangle(5, 5, 20, 20)}), 10, 10)
            .SetName("WhenCloudIsBiggerThanImage")
        };

        [TestCaseSource(nameof(DrawNoException))]
        public void Draw_ShouldNotThrow(Cloud cloud, int width, int height) =>
            Assert.DoesNotThrow(() => CloudDrawer.DrawCloud(cloud, width, height));
    }
}