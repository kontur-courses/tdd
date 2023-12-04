using NUnit.Framework;
using System.Drawing;
using TagsCloudVisualization;
using FluentAssertions;

namespace Tests
{
    [TestFixture]
    public class CloudLayoutDrawerTests
    {
        static IEnumerable<TestCaseData> DrawArgumentException => new[]
        {
            new TestCaseData(new []{new Rectangle(1, 1, 1, 1)}, 0, 1).SetName("WhenGivenNotPositiveWidth"),
            new TestCaseData(new []{new Rectangle(1, 1, 1, 1)}, 1, 0).SetName("WhenGivenNotPositiveHeigth"),
            new TestCaseData(new []{new Rectangle(1, 1, 1, 1)}, 0, 0).SetName("WhenGivenNotPositiveHeigthAndWidth"),
        };

        [TestCaseSource(nameof(DrawArgumentException))]
        public void Draw_ShouldThrowArgumentException(Rectangle[] rectangles, int width, int height) =>
            Assert.Throws<ArgumentException>(() => CloudLayoutDrawer.Draw(rectangles, width, height));

        static IEnumerable<TestCaseData> DrawArgumentNullException => new[]
        {
            new TestCaseData(null, 1, 1).SetName("WhenGivenNullArray"),
            new TestCaseData(new Rectangle[]{}, 1, 1).SetName("WhenGivenEmptyArray")
        };

        [TestCaseSource(nameof(DrawArgumentNullException))]
        public void Draw_ShouldThrowArgumentNullException(Rectangle[] rectangles, int width, int height) =>
            Assert.Throws<ArgumentNullException>(() => CloudLayoutDrawer.Draw(rectangles, width, height));
    }
}